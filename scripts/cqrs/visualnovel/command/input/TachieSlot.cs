using GFrameworkTemplate.scripts.enums.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.visualnovel.command.input;

/// <summary>
///     单个立绘槽位配置
/// </summary>
public sealed class TachieSlot
{
    public string FilePath { get; set; } = string.Empty;
    public TachieOperation Type { get; set; } = TachieOperation.Show;
}
