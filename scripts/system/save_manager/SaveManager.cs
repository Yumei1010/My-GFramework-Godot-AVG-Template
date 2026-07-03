using System.Text.Json;
using GFramework.Core.Abstractions.enums;
using GFramework.Core.Abstractions.system;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.system.visualnovel;
using Godot;

namespace GFrameworkTemplate.scripts.system.save_manager;

/// <summary>
///     存档管理器——5 槽位 JSON 持久化引擎状态，通过 ISystem 注册到 DI
/// </summary>
[Log]
[ContextAware]
public sealed partial class SaveManager : ISystem
{
    private StoryEngineSystem _engine = null!;
    private const string SaveDir = "user://saves";
    private const int MaxSlots = 5;

    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init()
    {
        _engine = this.GetSystem<StoryEngineSystem>()!;
        DirAccess.MakeDirAbsolute(SaveDir);
        _log.Debug("SaveManager 初始化");
    }
    public void Destroy() { }

    /// <summary>存档到指定槽位</summary>
    public void Save(int slot)
    {
        if (!_engine.IsPlaying) return;

        var data = new SaveData
        {
            PlayingJson = _engine.CurrentJsonPath,
            TalkBranch = _engine.TalkBranch.ToList(),
            CanNotChoose = _engine.CanNotChoose.ToList(),
            SaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            Slot = slot
        };

        var json = JsonSerializer.Serialize(data);
        using var file = FileAccess.Open(SlotPath(slot), FileAccess.ModeFlags.Write);
        file?.StoreString(json);

        _log.Debug($"存档: 槽位 {slot} ({data.PlayingJson} 进度)");
    }

    /// <summary>从指定槽位读档</summary>
    public SaveData? Load(int slot)
    {
        var path = SlotPath(slot);
        if (!FileAccess.FileExists(path)) return null;

        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        var json = file?.GetAsText();
        if (string.IsNullOrEmpty(json)) return null;

        var data = JsonSerializer.Deserialize<SaveData>(json);
        _log.Debug($"读档: 槽位 {slot} ({data?.PlayingJson})");
        return data;
    }

    /// <summary>恢复存档数据到引擎并重新播放</summary>
    public async Task Restore(int slot)
    {
        var data = Load(slot);
        if (data == null) return;

        // 恢复分支状态
        _engine.Stop();
        foreach (var b in data.CanNotChoose)
            _engine.AddCannotChoose(b);

        await _engine.LoadAndPlay(data.PlayingJson);
    }

    /// <summary>获取所有存档信息（无数据返回 null）</summary>
    public SaveData?[] ListSlots()
    {
        var slots = new SaveData?[MaxSlots];
        for (var i = 0; i < MaxSlots; i++)
            slots[i] = Load(i);
        return slots;
    }

    /// <summary>删除存档</summary>
    public void Delete(int slot)
    {
        var path = SlotPath(slot);
        if (FileAccess.FileExists(path))
        {
            DirAccess.RemoveAbsolute(path);
            _log.Debug($"删除存档: 槽位 {slot}");
        }
    }

    private static string SlotPath(int slot) => $"{SaveDir}/slot_{slot}.json";
}

/// <summary>存档数据结构</summary>
public sealed class SaveData
{
    public string PlayingJson { get; set; } = string.Empty;
    public List<string> TalkBranch { get; set; } = new();
    public List<string> CanNotChoose { get; set; } = new();
    public string SaveTime { get; set; } = string.Empty;
    public int Slot { get; set; }
    public override string ToString() => $"[槽{Slot}] {PlayingJson} ({SaveTime})";
}
