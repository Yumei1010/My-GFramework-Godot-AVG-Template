using GFrameworkTemplate.scripts.enums.visualnovel;

namespace GFrameworkTemplate.scripts.component.tachie_slot;

/// <summary>
///     单个立绘槽位配置
/// </summary>
public sealed class TachieSlot
{
    public string FilePath { get; set; } = string.Empty;
    public TachieOperation Type { get; set; } = TachieOperation.Show;

    /// <summary>显式指定槽位: Left / Center / Right，null 则自动分配</summary>
    public string? Slot { get; set; }
}
