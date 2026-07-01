using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.system.visualnovel;

public sealed class GotoWorker : IStoryCommandWorker
{
    public Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var gt = (GotoCommand)cmd;
        var target = gt.FilePath;

        if (string.IsNullOrEmpty(target))
            return Task.CompletedTask;

        ctx.SendEvent(new VisualNovelGotoTriggeredEvent { TargetFilePath = target });
        ctx.IsPlaying = false;
        ctx.PendingGoto = target;

        return Task.CompletedTask;
    }
}
