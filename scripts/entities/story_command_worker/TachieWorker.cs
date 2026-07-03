using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.entities.story_command_worker;

public sealed class TachieWorker : FireAndForgetWorker<TachieCommand, VisualNovelTachieTriggeredEvent>
{
    protected override VisualNovelTachieTriggeredEvent CreateEvent(TachieCommand cmd) =>
        new() { Tachies = cmd.Tachies };
}
