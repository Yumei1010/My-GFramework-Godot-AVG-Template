using GFrameworkTemplate.scripts.model.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

public sealed class StorySetCannotChooseCommand : AbstractCommand
{
    public required List<string> CannotChoose { get; set; }
    protected override void OnExecute() => this.GetModel<StoryStateModel>()!.CanNotChoose = CannotChoose;
}
