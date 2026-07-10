using GFrameworkTemplate.scripts.component.tachie_slot;

namespace GFrameworkTemplate.scripts.cqrs.tachie.@event;

/// <summary>
///     TachieUpdatedEvent —— 立绘状态更新事件
/// </summary>
public sealed class TachieUpdatedEvent
{
    public required Dictionary<string, TachieSlot> Tachies { get; init; }
}
