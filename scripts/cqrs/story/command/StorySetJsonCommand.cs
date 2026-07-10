using GFrameworkTemplate.scripts.model.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

public sealed class StorySetJsonCommand : AbstractCommand
{
    public required string JsonPath { get; set; }
    protected override void OnExecute() => this.GetModel<StoryStateModel>()!.PlayingJson = JsonPath;
}
