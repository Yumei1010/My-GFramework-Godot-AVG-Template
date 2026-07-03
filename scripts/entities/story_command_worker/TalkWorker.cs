using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.core.story;

namespace GFrameworkTemplate.scripts.entities.story_command_worker;

public sealed class TalkWorker : IStoryCommandWorker
{
    public async Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var talk = (TalkCommand)cmd;
        await ctx.AdvanceAsync(talk.TalkContent.Length * ctx.WordSpeed);
    }
}
