using GFrameworkTemplate.scripts.cqrs.visualnovel.command.input;

namespace GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

public sealed class VisualNovelBranchTriggeredEvent
{
    public required Dictionary<string, BranchOption> Options { get; init; }
}
