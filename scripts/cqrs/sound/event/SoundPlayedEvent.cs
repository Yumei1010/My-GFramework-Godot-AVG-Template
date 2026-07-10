namespace GFrameworkTemplate.scripts.cqrs.sound.@event;

public sealed class SoundPlayedEvent
{
    public required string SoundType { get; init; }
    public required string FilePath { get; init; }
}
