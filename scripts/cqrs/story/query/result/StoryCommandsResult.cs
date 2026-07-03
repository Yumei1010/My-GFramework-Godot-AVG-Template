using GFrameworkTemplate.scripts.core.story;

namespace GFrameworkTemplate.scripts.cqrs.story.query.result;

/// <summary>
///     故事命令列表查询结果
/// </summary>
public sealed class StoryCommandsResult
{
    public required List<StoryCommand> Commands { get; init; }
}
