using GFrameworkTemplate.scripts.system.tachie_system;

namespace GFrameworkTemplate.scripts.cqrs.tachie.command;

/// <summary>
///     TachieAddCommand —— 添加角色立绘到场景
/// </summary>
public sealed class TachieAddCommand : AbstractCommand
{
    public required string CharName { get; set; }
    public required string FilePath { get; set; }
    public string Slot { get; set; } = "";

    protected override void OnExecute()
    {
        this.GetSystem<TachieSystem>()!.Add(CharName, FilePath, Slot);
    }
}
