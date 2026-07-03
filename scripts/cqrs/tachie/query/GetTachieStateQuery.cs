using GFrameworkTemplate.scripts.cqrs.tachie.query.result;
using GFrameworkTemplate.scripts.model.tachie;

namespace GFrameworkTemplate.scripts.cqrs.tachie.query;

/// <summary>
///     查询当前立绘状态
/// </summary>
public sealed class GetTachieStateQuery : AbstractQuery<TachieStateResult>
{
    protected override TachieStateResult OnDo()
    {
        var model = this.GetModel<TachieModel>()!;
        return new TachieStateResult
        {
            Chars = new Dictionary<string, string>(model.Chars),
            SlotToChar = new Dictionary<string, string>(model.SlotToChar),
            SpotlightChar = model.SpotlightChar
        };
    }
}
