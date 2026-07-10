namespace GFrameworkTemplate.scripts.cqrs.story.@event;

/// <summary>
///     StoryCustomEventFiredEvent —— 自定义事件触发事件
/// </summary>
public sealed class StoryCustomEventFiredEvent
{
    public required string EventName { get; init; }
}
