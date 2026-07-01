namespace GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

public sealed class VisualNovelBackgroundTriggeredEvent
{
    public required string FilePath { get; init; }
    public bool WaitTweenEnd { get; init; }
    public float Delay { get; init; }
}
