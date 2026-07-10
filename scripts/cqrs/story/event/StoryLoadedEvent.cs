namespace GFrameworkTemplate.scripts.cqrs.story.@event;

public sealed class StoryLoadedEvent
{
    public required int CommandCount { get; init; }
}
