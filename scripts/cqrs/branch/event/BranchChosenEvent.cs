namespace GFrameworkTemplate.scripts.cqrs.branch.@event;

/// <summary>
///     BranchChosenEvent —— 玩家已选择分支事件
/// </summary>
public sealed class BranchChosenEvent
{
    public required string OptionId { get; init; }
}
