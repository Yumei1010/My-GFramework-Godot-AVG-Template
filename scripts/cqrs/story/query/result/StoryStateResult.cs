namespace GFrameworkTemplate.scripts.cqrs.story.query.result;

/// <summary>
///     故事状态查询结果
/// </summary>
public sealed class StoryStateResult
{
    public required bool IsPlaying { get; init; }
    public required string PlayingJson { get; init; }
    public required int CurrentIndex { get; init; }
    public required int CommandCount { get; init; }
    public required IReadOnlyList<string> TalkBranch { get; init; }
    public required IReadOnlyList<string> CanNotChoose { get; init; }
    public string? PendingGoto { get; init; }
    public float? AutoPlayDelay { get; init; }
    public float WordSpeed { get; init; }
}
