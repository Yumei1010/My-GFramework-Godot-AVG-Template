using GFrameworkTemplate.scripts.model.tachie;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.cqrs.tachie.@event;
using GFrameworkTemplate.scripts.component.tachie_slot;
using GFrameworkTemplate.scripts.enums.visualnovel;

namespace GFrameworkTemplate.scripts.system.tachie_system;

/// <summary>
///     立绘系统——管理立绘槽位状态，统一 JSON 路径与程序化路径
/// </summary>
[Log]
[ContextAware]
public sealed partial class TachieSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase)
    {
        _log.Debug("System initialized: TachieSystem");
    }

    public void Init()
    {
        
    }

    public void Destroy()
    {
        _log.Debug("System destroyed: TachieSystem");
    }

    /// <summary>JSON 路径：批量处理并发事件通知 Controller</summary>
    public void Apply(TachieCommand cmd)
    {
        if (cmd.Tachies.Count == 0) return;

        foreach (var (charName, slot) in cmd.Tachies)
        {
            var path = slot.FilePath;
            switch (slot.Type)
            {
                case TachieOperation.Close:    RemoveChar(charName, slot.Slot); break;
                case TachieOperation.OnlyShow: SpotlightChar(charName, path, slot.Slot); break;
                case TachieOperation.Change:   ChangeTexture(charName, path); break;
                default:                       AddChar(charName, path, slot.Slot); break;
            }
        }

        this.SendEvent(new TachieUpdatedEvent { Tachies = cmd.Tachies });
    }

    /// <summary>程序化路径：添加角色立绘</summary>
    public void Add(string name, string filePath, string slot = "")
    {
        AddChar(name, filePath, slot);
        Notify(name, filePath, TachieOperation.Show, slot);
    }

    /// <summary>程序化路径：切换立绘图片</summary>
    public void Change(string name, string filePath)
    {
        ChangeTexture(name, filePath);
        Notify(name, filePath, TachieOperation.Change);
    }

    /// <summary>程序化路径：移除立绘</summary>
    public void Remove(string name, string slot = "")
    {
        RemoveChar(name, slot);
        Notify(name, "", TachieOperation.Close, slot);
    }

    /// <summary>程序化路径：聚光灯显示</summary>
    public void Spotlight(string name, string filePath, string slot = "Center")
    {
        SpotlightChar(name, filePath, slot);
        Notify(name, filePath, TachieOperation.OnlyShow, slot);
    }

    private void AddChar(string name, string filePath, string slot)
    {
        var model = this.GetModel<TachieModel>()!;
        if (model.Chars.ContainsKey(name)) return;
        model.Chars[name] = filePath;
        if (!string.IsNullOrEmpty(slot))
            model.SlotToChar[slot] = name;
        else
            Reposition();
    }

    private void ChangeTexture(string name, string filePath)
    {
        this.GetModel<TachieModel>()!.Chars[name] = filePath;
    }

    private void RemoveChar(string name, string slot)
    {
        var model = this.GetModel<TachieModel>()!;
        model.Chars.Remove(name);
        if (name == model.SpotlightChar) model.SpotlightChar = string.Empty;

        if (!string.IsNullOrEmpty(slot))
            model.SlotToChar.Remove(slot);
        else
        {
            var key = model.SlotToChar.FirstOrDefault(kv => kv.Value == name).Key;
            if (key != null) model.SlotToChar.Remove(key);
        }

        if (string.IsNullOrEmpty(slot)) Reposition();
    }

    private void SpotlightChar(string name, string filePath, string slot)
    {
        var model = this.GetModel<TachieModel>()!;
        model.Chars[name] = filePath;
        model.SpotlightChar = name;
        model.SlotToChar.Clear();
        model.SlotToChar[slot ?? "Center"] = name;
    }

    private void Reposition()
    {
        var model = this.GetModel<TachieModel>()!;
        if (!string.IsNullOrEmpty(model.SpotlightChar)) return;
        model.SlotToChar.Clear();
        var list = model.Chars.ToList();
        if (list.Count >= 1) model.SlotToChar["Left"] = list[0].Key;
        if (list.Count >= 2) model.SlotToChar["Right"] = list[1].Key;
    }

    private void Notify(string name, string filePath, TachieOperation op, string slot = "")
    {
        var dict = new Dictionary<string, TachieSlot>
        {
            [name] = new TachieSlot { Type = op, FilePath = filePath, Slot = slot }
        };
        this.SendEvent(new TachieUpdatedEvent { Tachies = dict });
    }
}
