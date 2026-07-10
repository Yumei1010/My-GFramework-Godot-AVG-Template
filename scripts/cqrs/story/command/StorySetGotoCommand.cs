using GFrameworkTemplate.scripts.model.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

public sealed class StorySetGotoCommand : AbstractCommand
{
    public string? GotoTarget { get; set; }
    protected override void OnExecute() => this.GetModel<StoryStateModel>()!.PendingGoto = GotoTarget;
}
