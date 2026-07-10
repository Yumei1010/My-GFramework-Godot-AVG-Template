using GFrameworkTemplate.scripts.component.branch_option;
using GFrameworkTemplate.scripts.system.branch_system;

namespace GFrameworkTemplate.scripts.cqrs.branch.command;

public sealed class BranchShowCommand : AbstractAsyncCommand
{
    public required Dictionary<string, BranchOption> Options { get; set; }
    protected override async Task OnExecuteAsync()
    {
        await this.GetSystem<BranchSystem>()!.ShowAsync(Options);
    }
}
