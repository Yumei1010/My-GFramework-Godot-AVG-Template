using GFrameworkTemplate.scripts.component.tachie_slot;

namespace GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

public sealed class VisualNovelTachieUpdatedEvent
{
    public required Dictionary<string, TachieSlot> Tachies { get; init; }
}
