namespace GFrameworkTemplate.scripts.cqrs.audio.@event;

public sealed class AudioVolumeChangedEvent
{
    public string BusName { get; init; } = "";
    public float Volume { get; init; }
}
