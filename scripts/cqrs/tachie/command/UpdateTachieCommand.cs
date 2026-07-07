using GFrameworkTemplate.scripts.enums.visualnovel;
using GFrameworkTemplate.scripts.model.tachie;

namespace GFrameworkTemplate.scripts.cqrs.tachie.command;

/// <summary>
///     更新立绘数据（Show/Change/Close/OnlyShow），支持显式槽位指定
/// </summary>
public sealed class UpdateTachieCommand : AbstractCommand
{
    public required TachieOperation Type { get; set; }
    public required string CharName { get; set; }
    public required string FilePath { get; set; }

    /// <summary>显式指定槽位，null 则自动分配</summary>
    public string? Slot { get; set; }

    protected override void OnExecute()
    {
        var model = this.GetModel<TachieModel>()!;

        switch (Type)
        {
            case TachieOperation.Show:
                if (model.Chars.ContainsKey(CharName)) return;
                model.Chars[CharName] = FilePath;
                if (Slot != null)
                    model.SlotToChar[Slot] = CharName;
                else
                    Reposition(model);
                break;

            case TachieOperation.Change:
                model.Chars[CharName] = FilePath;
                break;

            case TachieOperation.Close:
                model.Chars.Remove(CharName);
                if (CharName == model.SpotlightChar) model.SpotlightChar = null;
                if (Slot != null)
                    model.SlotToChar.Remove(Slot);
                else
                {
                    var s = model.SlotToChar.FirstOrDefault(kv => kv.Value == CharName).Key;
                    if (s != null) model.SlotToChar.Remove(s);
                }
                if (Slot == null) Reposition(model);
                break;

            case TachieOperation.OnlyShow:
                model.Chars[CharName] = FilePath;
                model.SpotlightChar = CharName;
                model.SlotToChar.Clear();
                model.SlotToChar[Slot ?? "Center"] = CharName;
                break;
        }
    }

    private static void Reposition(TachieModel model)
    {
        if (model.SpotlightChar != null) return;
        model.SlotToChar.Clear();
        var list = model.Chars.ToList();
        if (list.Count >= 1) model.SlotToChar["Left"] = list[0].Key;
        if (list.Count >= 2) model.SlotToChar["Right"] = list[1].Key;
    }
}
