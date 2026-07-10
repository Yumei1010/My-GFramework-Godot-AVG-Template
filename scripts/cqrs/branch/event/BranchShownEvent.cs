using GFrameworkTemplate.scripts.component.branch_option;

namespace GFrameworkTemplate.scripts.cqrs.branch.@event;

public sealed class BranchShownEvent
{
    public required Dictionary<string, BranchOption> Options { get; init; }
}
