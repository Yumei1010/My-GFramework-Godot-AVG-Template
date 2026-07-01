using GFrameworkTemplate.scripts.core.story;

namespace GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

public sealed class VisualNovelTachieTriggeredEvent
{
    public required Dictionary<string, TachieSlot> Tachies { get; init; }
}
