using GFrameworkTemplate.scripts.model.talk;

namespace GFrameworkTemplate.scripts.cqrs.talk.command;

/// <summary>
///     TalkSetVisibleCommand —— 设置对话栏可见性
/// </summary>
public sealed class TalkSetVisibleCommand : AbstractCommand
{
    public required bool Visible { get; set; }

    protected override void OnExecute()
    {
        this.GetModel<TalkModel>()!.Visible = Visible;
    }
}
