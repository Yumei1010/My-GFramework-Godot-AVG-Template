using GFrameworkTemplate.scripts.enums.visualnovel;
using GFrameworkTemplate.scripts.model.tachie;

namespace GFrameworkTemplate.scripts.cqrs.tachie.command;

/// <summary>
///     更新立绘数据（Show/Change/Close/OnlyShow）
/// </summary>
public sealed class UpdateTachieCommand : AbstractCommand
{
    public required TachieOperation Type { get; set; }
    public required string CharName { get; set; }
    public required string FilePath { get; set; }

    protected override void OnExecute()
    {
        var model = this.GetModel<TachieModel>()!;

        switch (Type)
        {
            case TachieOperation.Show:
                if (model.Chars.ContainsKey(CharName)) return;
                model.Chars[CharName] = FilePath;
                Reposition(model);
                break;
            case TachieOperation.Change:
                if (!model.Chars.ContainsKey(CharName))
                    model.Chars[CharName] = FilePath;
                else
                    model.Chars[CharName] = FilePath;
                break;
            case TachieOperation.Close:
                model.Chars.Remove(CharName);
                if (CharName == model.SpotlightChar) model.SpotlightChar = null;
                var slot = model.SlotToChar.FirstOrDefault(kv => kv.Value == CharName).Key;
                if (slot != null) model.SlotToChar.Remove(slot);
                Reposition(model);
                break;
            case TachieOperation.OnlyShow:
                model.Chars[CharName] = FilePath;
                model.SpotlightChar = CharName;
                model.SlotToChar.Clear();
                model.SlotToChar["Center"] = CharName;
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
