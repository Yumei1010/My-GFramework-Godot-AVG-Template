namespace GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

public sealed class VisualNovelGotoNavigatedEvent
{
    public required string TargetFilePath { get; init; }
}
