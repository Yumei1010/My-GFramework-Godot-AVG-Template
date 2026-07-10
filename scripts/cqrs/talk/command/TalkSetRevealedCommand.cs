using GFrameworkTemplate.scripts.model.talk;

namespace GFrameworkTemplate.scripts.cqrs.talk.command;

public sealed class TalkSetRevealedCommand : AbstractCommand
{
    public required bool Revealed { get; set; }

    protected override void OnExecute()
    {
        this.GetModel<TalkModel>()!.Revealed = Revealed;
    }
}
