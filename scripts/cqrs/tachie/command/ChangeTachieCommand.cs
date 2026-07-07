using GFramework.Core.command;
using GFrameworkTemplate.scripts.component.tachie_slot;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.tachie.command;

/// <summary>
///     切换立绘命令——通过 TachieSystem 驱动立绘变换
/// </summary>
public sealed class ChangeTachieCommand : AbstractCommand
{
    public required Dictionary<string, TachieSlot> Tachies { get; set; }

    protected override void OnExecute()
    {
        this.GetSystem<TachieSystem>()!.Apply(Tachies);
    }
}
