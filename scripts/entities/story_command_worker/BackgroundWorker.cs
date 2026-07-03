using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.entities.story_command_worker;

public sealed class BackgroundWorker : IStoryCommandWorker
{
    public async Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var bg = (BackgroundCommand)cmd;
        if (bg.Delay > 0) await Task.Delay(TimeSpan.FromSeconds(bg.Delay));

        ctx.SendEvent(new VisualNovelBackgroundTriggeredEvent
        {
            FilePath = bg.FilePath ?? string.Empty,
            WaitTweenEnd = bg.WaitTweenEnd,
            Delay = bg.Delay
        });

        if (bg.WaitTweenEnd) await Task.Delay(TimeSpan.FromSeconds(0.5f));
    }
}
