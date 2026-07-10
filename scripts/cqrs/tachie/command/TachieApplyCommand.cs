using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.system.tachie_system;

namespace GFrameworkTemplate.scripts.cqrs.tachie.command;

/// <summary>
///     TachieApplyCommand —— 批量处理立绘 JSON 数据
/// </summary>
public sealed class TachieApplyCommand(TachieCommand cmd) : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<TachieSystem>().Apply(cmd);
    }
}
