using GFrameworkTemplate.scripts.model.talk;

namespace GFrameworkTemplate.scripts.cqrs.talk.command;

/// <summary>
///     TalkSetRevealedCommand —— 设置对话文本完成状态
/// </summary>
public sealed class TalkSetRevealedCommand : AbstractCommand
{
    public required bool Revealed { get; set; }

    protected override void OnExecute()
    {
        this.GetModel<TalkModel>()!.Revealed = Revealed;
    }
}
