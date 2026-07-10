namespace GFrameworkTemplate.scripts.cqrs.background.@event;

/// <summary>
///     BackgroundChangedEvent —— 背景切换完成事件
/// </summary>
public sealed class BackgroundChangedEvent
{
    public required string FilePath { get; init; }
    public bool WaitTweenEnd { get; init; }
    public float Delay { get; init; }
}
