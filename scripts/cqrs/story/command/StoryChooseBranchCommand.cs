using GFrameworkTemplate.scripts.cqrs.branch.@event;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

/// <summary>
///     StoryChooseBranchCommand —— 玩家选择了某个分支选项
/// </summary>
public sealed class StoryChooseBranchCommand : AbstractCommand
{
    public required string OptionId { get; set; }

    protected override void OnExecute()
    {
        this.SendEvent(new BranchChosenEvent { OptionId = OptionId });
    }
}
