namespace GFrameworkTemplate.scripts.cqrs.talk.@event;

/// <summary>
///     TalkPlayedEvent —— 对话播放事件
/// </summary>
public sealed class TalkPlayedEvent
{
    public string? Talker { get; init; }
    public required string Content { get; init; }
    public bool IsCenter { get; init; }
    public bool Center { get; init; }
    public bool Code { get; init; }
    public float RevealSpeed { get; init; } = 0.04f;
}
