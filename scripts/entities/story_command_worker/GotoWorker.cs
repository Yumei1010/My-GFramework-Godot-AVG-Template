using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;

namespace GFrameworkTemplate.scripts.entities.story_command_worker;

public sealed class GotoWorker : IStoryCommandWorker
{
    public Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var gt = (GotoCommand)cmd;
        var target = gt.FilePath;

        if (string.IsNullOrEmpty(target))
            return Task.CompletedTask;

        ctx.IsPlaying = false;
        ctx.PendingGoto = target;

        return Task.CompletedTask;
    }
}
