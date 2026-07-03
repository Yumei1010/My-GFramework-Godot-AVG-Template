using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;

namespace GFrameworkTemplate.scripts.entities.story_command_worker;

public sealed class EventWorker : IStoryCommandWorker
{
    public async Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var evt = (EventCommand)cmd;
        await ctx.AdvanceAsync(0.1f);
    }
}
