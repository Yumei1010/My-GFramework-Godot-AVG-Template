namespace GFrameworkTemplate.scripts.cqrs.story.@event;

/// <summary>
///     StoryLoadedEvent —— 故事加载完成事件
/// </summary>
public sealed class StoryLoadedEvent
{
    public required int CommandCount { get; init; }
}
