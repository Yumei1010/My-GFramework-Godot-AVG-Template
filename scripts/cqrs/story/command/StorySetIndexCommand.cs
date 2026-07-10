using GFrameworkTemplate.scripts.model.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

public sealed class StorySetIndexCommand : AbstractCommand
{
    public required int Index { get; set; }
    protected override void OnExecute() => this.GetModel<StoryStateModel>()!.CurrentIndex = Index;
}
