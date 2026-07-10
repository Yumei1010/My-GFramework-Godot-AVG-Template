using GFrameworkTemplate.scripts.model.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

/// <summary>
///     StorySetGotoCommand —— 写入 PendingGoto 跳转目标
/// </summary>
public sealed class StorySetGotoCommand : AbstractCommand
{
    public string? GotoTarget { get; set; }
    protected override void OnExecute() => this.GetModel<StoryStateModel>()!.PendingGoto = GotoTarget;
}
