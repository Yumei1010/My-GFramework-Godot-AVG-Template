namespace GFrameworkTemplate.scripts.cqrs.background.@event;

public sealed class BackgroundChangedEvent
{
    public required string FilePath { get; init; }
    public bool WaitTweenEnd { get; init; }
    public float Delay { get; init; }
}
