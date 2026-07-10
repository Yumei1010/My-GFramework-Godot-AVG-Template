using GFrameworkTemplate.scripts.model.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

/// <summary>
///     StorySetCannotChooseCommand —— 写入 CanNotChoose 禁用分支列表
/// </summary>
public sealed class StorySetCannotChooseCommand : AbstractCommand
{
    public required List<string> CannotChoose { get; set; }
    protected override void OnExecute() => this.GetModel<StoryStateModel>()!.CanNotChoose = CannotChoose;
}
