using GFrameworkTemplate.scripts.model.visualnovel;
using GFrameworkTemplate.scripts.core.story;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

/// <summary>
///     StorySetCommandsCommand —— 写入 Commands 列表
/// </summary>
public sealed class StorySetCommandsCommand : AbstractCommand
{
    public required List<StoryCommand> Commands { get; set; }
    protected override void OnExecute() => this.GetModel<StoryStateModel>()!.Commands = Commands;
}
