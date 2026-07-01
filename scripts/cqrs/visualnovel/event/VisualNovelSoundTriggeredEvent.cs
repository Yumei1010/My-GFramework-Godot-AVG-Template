namespace GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

public sealed class VisualNovelSoundTriggeredEvent
{
    public required string SoundType { get; init; }
    public required string FilePath { get; init; }
}
