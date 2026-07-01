namespace GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

public sealed class VisualNovelStoryLoadedEvent
{
    public required int CommandCount { get; init; }
}
