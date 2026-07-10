namespace GFrameworkTemplate.scripts.cqrs.story.@event;

public sealed class StoryLoadStartedEvent
{
    public required string FilePath { get; init; }
}
