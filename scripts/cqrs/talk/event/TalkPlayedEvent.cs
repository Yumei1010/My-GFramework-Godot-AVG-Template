namespace GFrameworkTemplate.scripts.cqrs.talk.@event;

public sealed class TalkPlayedEvent
{
    public string? Talker { get; init; }
    public required string Content { get; init; }
    public bool IsCenter { get; init; }
    public float RevealSpeed { get; init; } = 0.04f;
}
