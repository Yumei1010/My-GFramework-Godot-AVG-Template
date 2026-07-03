namespace GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

public sealed class VisualNovelStoryLoadStartedEvent
{
    public required string FilePath { get; init; }
}
