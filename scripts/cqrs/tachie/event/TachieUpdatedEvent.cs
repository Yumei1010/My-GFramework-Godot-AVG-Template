using GFrameworkTemplate.scripts.component.tachie_slot;

namespace GFrameworkTemplate.scripts.cqrs.tachie.@event;

public sealed class TachieUpdatedEvent
{
    public required Dictionary<string, TachieSlot> Tachies { get; init; }
}
