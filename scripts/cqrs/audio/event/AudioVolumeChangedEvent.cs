namespace GFrameworkTemplate.scripts.cqrs.audio.@event;

/// <summary>
///     AudioVolumeChangedEvent —— 音量变化事件
/// </summary>
public sealed class AudioVolumeChangedEvent
{
    public string BusName { get; init; } = "";
    public float Volume { get; init; }
}
