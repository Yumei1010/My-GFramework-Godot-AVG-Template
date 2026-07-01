namespace GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

public sealed class VisualNovelTalkTriggeredEvent
{
    public string? Talker { get; init; }
    public required string Content { get; init; }
    public bool IsCenter { get; init; }
    public string? AvatarPath { get; init; }
}
