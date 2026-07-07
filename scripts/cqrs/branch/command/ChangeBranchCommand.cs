using GFrameworkTemplate.scripts.component.branch_option;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.branch.command;

/// <summary>
///     显示分支选项命令——通过 BranchSystem 等待玩家选择
/// </summary>
public sealed class ChangeBranchCommand : AbstractAsyncCommand
{
    public required Dictionary<string, BranchOption> Options { get; set; }

    protected override async Task OnExecuteAsync()
    {
        await this.GetSystem<BranchSystem>()!.ShowAsync(Options);
    }
}
