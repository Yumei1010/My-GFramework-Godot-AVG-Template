using GFrameworkTemplate.scripts.system.tachie_system;

namespace GFrameworkTemplate.scripts.cqrs.tachie.command;

/// <summary>
///     TachieSpotlightCommand —— 聚光灯独立显示角色
/// </summary>
public sealed class TachieSpotlightCommand : AbstractCommand
{
    public required string CharName { get; set; }
    public required string FilePath { get; set; }
    public string Slot { get; set; } = "Center";

    protected override void OnExecute()
    {
        this.GetSystem<TachieSystem>()!.Spotlight(CharName, FilePath, Slot);
    }
}
