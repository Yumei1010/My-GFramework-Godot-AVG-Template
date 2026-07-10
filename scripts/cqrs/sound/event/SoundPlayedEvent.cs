namespace GFrameworkTemplate.scripts.cqrs.sound.@event;

/// <summary>
///     SoundPlayedEvent —— 音频播放事件
/// </summary>
public sealed class SoundPlayedEvent
{
    public required string SoundType { get; init; }
    public required string FilePath { get; init; }
}
