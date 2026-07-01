namespace GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

public sealed class VisualNovelGotoTriggeredEvent
{
    public required string TargetFilePath { get; init; }
}
