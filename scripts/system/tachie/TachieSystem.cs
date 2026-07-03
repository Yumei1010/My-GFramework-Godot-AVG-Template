using GFrameworkTemplate.scripts.enums.visualnovel;
using GFrameworkTemplate.scripts.model.tachie;

namespace GFrameworkTemplate.scripts.system.tachie;

/// <summary>
///     立绘系统——纯 ISystem，通过 TachieModel 管理角色数据
/// </summary>
[Log]
[ContextAware]
public sealed partial class TachieSystem : ISystem
{
    public event Action? Changed;

    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    private TachieModel Model => this.GetModel<TachieModel>()!;

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
        if (Model.Chars.ContainsKey(name)) return;
        Model.Chars[name] = path;
        Reposition();
    }

    private void Change(string name, string path)
    {
        if (!Model.Chars.ContainsKey(name)) { Show(name, path); return; }
        Model.Chars[name] = path;
    }

    private void Close(string name)
    {
        Model.Chars.Remove(name);
        if (name == Model.SpotlightChar) Model.SpotlightChar = null;
        var slot = Model.SlotToChar.FirstOrDefault(kv => kv.Value == name).Key;
        if (slot != null) Model.SlotToChar.Remove(slot);
        Reposition();
    }

    private void OnlyShow(string name, string path)
    {
        Model.Chars[name] = path;
        Model.SpotlightChar = name;
        Model.SlotToChar.Clear();
        Model.SlotToChar["Center"] = name;
    }

    private void Reposition()
    {
        if (Model.SpotlightChar != null) return;
        Model.SlotToChar.Clear();
        var list = Model.Chars.ToList();
        if (list.Count >= 1) Model.SlotToChar["Left"] = list[0].Key;
        if (list.Count >= 2) Model.SlotToChar["Right"] = list[1].Key;
    }
}
