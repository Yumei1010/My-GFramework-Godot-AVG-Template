using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.system.visualnovel;

public sealed class EventWorker : IStoryCommandWorker
{
    public async Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var evt = (EventCommand)cmd;
        ctx.SendEvent(new VisualNovelCustomEventTriggeredEvent { EventName = evt.EventName });
        await EngineAwait.Advance(0.1f, ctx);
    }
}
