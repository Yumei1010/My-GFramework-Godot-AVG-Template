namespace GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

public sealed class VisualNovelTextRevealProgressEvent
{
    public required int RevealedChars { get; init; }
    public required int TotalChars { get; init; }
}
