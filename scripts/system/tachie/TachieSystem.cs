using GFrameworkTemplate.scripts.component.tachie_slot;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.enums.visualnovel;

namespace GFrameworkTemplate.scripts.system.tachie;

/// <summary>
///     立绘系统——纯 ISystem，管理角色数据和槽位分配逻辑
/// </summary>
[Log]
[ContextAware]
public sealed partial class TachieSystem : ISystem
{
    public readonly Dictionary<string, string> Chars = new();
    public readonly Dictionary<string, string> SlotToChar = new();
    public string? SpotlightChar { get; private set; }

    public event Action? Changed;

    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    public void Handle(TachieOperation type, string charName, string filePath)
    {
        switch (type)
        {
            case TachieOperation.Show: Show(charName, filePath); break;
            case TachieOperation.Change: Change(charName, filePath); break;
            case TachieOperation.Close: Close(charName); break;
            case TachieOperation.OnlyShow: OnlyShow(charName, filePath); break;
        }
        Changed?.Invoke();
    }

    private void Show(string name, string path)
    {
        if (Chars.ContainsKey(name)) return;
        Chars[name] = path;
        Reposition();
    }

    private void Change(string name, string path)
    {
        if (!Chars.ContainsKey(name))
        {
            Show(name, path);
            return;
        }
        Chars[name] = path;
    }

    private void Close(string name)
    {
        Chars.Remove(name);
        if (name == SpotlightChar) SpotlightChar = null;
        var slot = SlotToChar.FirstOrDefault(kv => kv.Value == name).Key;
        if (slot != null) SlotToChar.Remove(slot);
        Reposition();
    }

    private void OnlyShow(string name, string path)
    {
        Chars[name] = path;
        SpotlightChar = name;
        SlotToChar.Clear();
        SlotToChar["Center"] = name;
    }

    private void Reposition()
    {
        if (SpotlightChar != null) return;
        SlotToChar.Clear();
        var list = Chars.ToList();
        if (list.Count >= 1) SlotToChar["Left"] = list[0].Key;
        if (list.Count >= 2) SlotToChar["Right"] = list[1].Key;
    }
}
