using GFramework.Core.extensions;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.data.story;
using GFrameworkTemplate.scripts.enums.visualnovel;
using Godot;

namespace GFrameworkTemplate.global;

/// <summary>
///     立绘栏全局单例——3 槽位（左/中/右），自动分配，交叉淡入淡出切换表情
/// </summary>
[Log]
[ContextAware]
public partial class TachieManager : CanvasLayer
{
    private TextureRect LeftSlot => GetNode<TextureRect>("%LeftSlot");
    private TextureRect CenterSlot => GetNode<TextureRect>("%CenterSlot");
    private TextureRect RightSlot => GetNode<TextureRect>("%RightSlot");
    private TextureRect HelperSlot => GetNode<TextureRect>("%HelperSlot");

    /// <summary>角色名 → 文件路径（在场角色）</summary>
    private readonly Dictionary<string, string> _chars = new();
    /// <summary>当前槽位分配：槽位名 → 角色名</summary>
    private readonly Dictionary<string, string> _slotToChar = new();

    public override void _Ready()
    {
        this.RegisterEvent<VisualNovelTachieTriggeredEvent>(OnTachie).UnRegisterWhenNodeExitTree(this);
    }

    private void OnTachie(VisualNovelTachieTriggeredEvent e)
    {
        foreach (var (charName, slot) in e.Tachies)
        {
            switch (slot.Type)
            {
                case TachieOperation.Show:  ShowChar(charName, slot.FilePath); break;
                case TachieOperation.Change: ChangeChar(charName, slot.FilePath); break;
                case TachieOperation.Close:  CloseChar(charName); break;
            }
        }
    }

    private void ShowChar(string name, string path)
    {
        if (_chars.ContainsKey(name)) return;

        var tex = LoadTexture(path);
        if (tex == null) return;

        _chars[name] = path; // 暂存，Reposition 后正式分配槽位
        RepositionAll();
    }

    private async void ChangeChar(string name, string path)
    {
        if (!_chars.ContainsKey(name))
        {
            ShowChar(name, path);
            return;
        }

        _chars[name] = path;
        var slotName = _slotToChar.FirstOrDefault(kv => kv.Value == name).Key;
        if (slotName == null) return;

        var rect = GetSlotRect(slotName);
        var newTex = LoadTexture(path);
        if (newTex == null) return;

        HelperSlot.Texture = newTex;
        HelperSlot.Position = rect.Position;
        HelperSlot.Size = rect.Size;
        HelperSlot.Modulate = Colors.Transparent;
        HelperSlot.Visible = true;

        var tween = CreateTween();
        tween.TweenProperty(HelperSlot, "modulate", Colors.White, 0.3f);
        tween.Parallel().TweenProperty(rect, "modulate", Colors.Transparent, 0.3f);
        await ToSignal(tween, Tween.SignalName.Finished);

        rect.Texture = newTex;
        rect.Modulate = Colors.White;
        HelperSlot.Visible = false;
    }

    private void CloseChar(string name)
    {
        if (!_chars.Remove(name)) return;
        var slotName = _slotToChar.FirstOrDefault(kv => kv.Value == name).Key;
        if (slotName != null) _slotToChar.Remove(slotName);
        RepositionAll();
    }

    /// <summary>
    ///     重新分配槽位：1 人→居中，2 人→左右，移除→重排
    /// </summary>
    private void RepositionAll()
    {
        // 隐藏所有槽位
        foreach (var r in new[] { LeftSlot, CenterSlot, RightSlot })
            r.Visible = false;
        _slotToChar.Clear();

        var charList = _chars.ToList();
        var count = charList.Count;

        if (count == 1)
        {
            AssignSlot("Center", charList[0].Key, charList[0].Value);
        }
        else if (count >= 2)
        {
            AssignSlot("Left", charList[0].Key, charList[0].Value);
            AssignSlot("Right", charList[1].Key, charList[1].Value);
            if (count >= 3)
                AssignSlot("Center", charList[2].Key, charList[2].Value);
        }
    }

    private void AssignSlot(string slotName, string charName, string filePath)
    {
        var rect = GetSlotRect(slotName);
        var tex = LoadTexture(filePath);
        if (tex == null) return;

        rect.Texture = tex;
        rect.Visible = true;
        rect.Modulate = Colors.White;
        _slotToChar[slotName] = charName;
    }

    private TextureRect GetSlotRect(string slotName) => slotName switch
    {
        "Left" => LeftSlot,
        "Center" => CenterSlot,
        "Right" => RightSlot,
        _ => LeftSlot
    };

    private Texture2D? LoadTexture(string logicalName)
    {
        var p = StoryResourceMapper.ResolveTexturePath(logicalName);
        if (string.IsNullOrEmpty(p)) { _log.Warn($"立绘纹理未注册: {logicalName}"); return null; }
        return GD.Load<Texture2D>(p);
    }
}
