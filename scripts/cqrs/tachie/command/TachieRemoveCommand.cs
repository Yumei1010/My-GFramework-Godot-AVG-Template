using GFrameworkTemplate.scripts.system.tachie_system;

namespace GFrameworkTemplate.scripts.cqrs.tachie.command;

/// <summary>
///     TachieRemoveCommand —— 移除角色立绘
/// </summary>
public sealed class TachieRemoveCommand : AbstractCommand
{
    public required string CharName { get; set; }
    public string Slot { get; set; } = "";

    protected override void OnExecute()
    {
        this.GetSystem<TachieSystem>()!.Remove(CharName, Slot);
    }
}
