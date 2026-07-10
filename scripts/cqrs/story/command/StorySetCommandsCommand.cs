using GFrameworkTemplate.scripts.model.visualnovel;
using GFrameworkTemplate.scripts.core.story;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

public sealed class StorySetCommandsCommand : AbstractCommand
{
    public required List<StoryCommand> Commands { get; set; }
    protected override void OnExecute() => this.GetModel<StoryStateModel>()!.Commands = Commands;
}
