using GFrameworkTemplate.scripts.model.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

public sealed class StorySetBranchCommand : AbstractCommand
{
    public required List<string> Branches { get; set; }
    protected override void OnExecute() => this.GetModel<StoryStateModel>()!.TalkBranch = Branches;
}
