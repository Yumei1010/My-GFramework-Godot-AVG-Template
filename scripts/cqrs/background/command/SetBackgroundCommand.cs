using GFrameworkTemplate.scripts.model.background;

namespace GFrameworkTemplate.scripts.cqrs.background.command;

public sealed class SetBackgroundCommand : AbstractCommand
{
    public required string FilePath { get; set; }

    protected override void OnExecute()
    {
        this.GetModel<BackgroundModel>()!.CurrentPath = FilePath;
    }
}
