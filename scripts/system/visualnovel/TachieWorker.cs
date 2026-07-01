using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.system.visualnovel;

public sealed class TachieWorker : IStoryCommandWorker
{
    public Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var tachie = (TachieCommand)cmd;
        ctx.SendEvent(new VisualNovelTachieTriggeredEvent { Tachies = tachie.Tachies });
        return Task.CompletedTask;
    }
}
