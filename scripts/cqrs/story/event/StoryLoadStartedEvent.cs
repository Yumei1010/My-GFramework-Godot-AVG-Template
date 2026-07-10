namespace GFrameworkTemplate.scripts.cqrs.story.@event;

/// <summary>
///     StoryLoadStartedEvent —— 故事开始加载事件
/// </summary>
public sealed class StoryLoadStartedEvent
{
    public required string FilePath { get; init; }
}
