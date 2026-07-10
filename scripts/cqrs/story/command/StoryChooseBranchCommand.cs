using GFrameworkTemplate.scripts.cqrs.branch.@event;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

public sealed class StoryChooseBranchCommand : AbstractCommand
{
    public required string OptionId { get; set; }

    protected override void OnExecute()
    {
        this.SendEvent(new BranchChosenEvent { OptionId = OptionId });
    }
}
