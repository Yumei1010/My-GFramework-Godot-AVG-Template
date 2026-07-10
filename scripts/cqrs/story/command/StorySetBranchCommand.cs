using GFrameworkTemplate.scripts.model.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

/// <summary>
///     StorySetBranchCommand —— 写入 TalkBranch 分支选择列表
/// </summary>
public sealed class StorySetBranchCommand : AbstractCommand
{
    public required List<string> Branches { get; set; }
    protected override void OnExecute() => this.GetModel<StoryStateModel>()!.TalkBranch = Branches;
}
