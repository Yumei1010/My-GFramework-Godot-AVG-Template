using GFrameworkTemplate.scripts.component.branch_option;
using GFrameworkTemplate.scripts.system.branch_system;

namespace GFrameworkTemplate.scripts.cqrs.branch.command;

/// <summary>
///     BranchShowCommand —— 显示分支选项并等待玩家选择
/// </summary>
public sealed class BranchShowCommand : AbstractAsyncCommand
{
    public required Dictionary<string, BranchOption> Options { get; set; }
    protected override async Task OnExecuteAsync()
    {
        await this.GetSystem<BranchSystem>()!.ShowAsync(Options);
    }
}
