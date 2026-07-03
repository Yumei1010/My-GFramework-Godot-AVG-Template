using GFrameworkTemplate.scripts.model.talk;

namespace GFrameworkTemplate.scripts.cqrs.talk.command;

public sealed class SetTalkVisibleCommand : AbstractCommand
{
    public required bool Visible { get; set; }

    protected override void OnExecute()
    {
        this.GetModel<TalkModel>()!.Visible = Visible;
    }
}
