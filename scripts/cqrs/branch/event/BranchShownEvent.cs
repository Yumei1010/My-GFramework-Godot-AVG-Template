using GFrameworkTemplate.scripts.component.branch_option;

namespace GFrameworkTemplate.scripts.cqrs.branch.@event;

/// <summary>
///     BranchShownEvent —— 分支选项已显示事件
/// </summary>
public sealed class BranchShownEvent
{
    public required Dictionary<string, BranchOption> Options { get; init; }
}
