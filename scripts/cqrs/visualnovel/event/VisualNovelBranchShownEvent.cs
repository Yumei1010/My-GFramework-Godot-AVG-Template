using GFrameworkTemplate.scripts.component.branch_option;

namespace GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

public sealed class VisualNovelBranchShownEvent
{
    public required Dictionary<string, BranchOption> Options { get; init; }
}
