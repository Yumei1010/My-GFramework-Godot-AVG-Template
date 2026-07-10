using GFrameworkTemplate.scripts.system.tachie_system;

namespace GFrameworkTemplate.scripts.cqrs.tachie.command;

public sealed class TachieChangeCommand : AbstractCommand
{
    public required string CharName { get; set; }
    public required string FilePath { get; set; }

    protected override void OnExecute()
    {
        this.GetSystem<TachieSystem>()!.Change(CharName, FilePath);
    }
}
