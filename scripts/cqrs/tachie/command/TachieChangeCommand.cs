using GFrameworkTemplate.scripts.system.tachie_system;

namespace GFrameworkTemplate.scripts.cqrs.tachie.command;

/// <summary>
///     TachieChangeCommand —— 切换角色立绘图片
/// </summary>
public sealed class TachieChangeCommand : AbstractCommand
{
    public required string CharName { get; set; }
    public required string FilePath { get; set; }

    protected override void OnExecute()
    {
        this.GetSystem<TachieSystem>()!.Change(CharName, FilePath);
    }
}
