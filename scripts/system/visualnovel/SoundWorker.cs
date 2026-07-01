using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.system.visualnovel;

public sealed class SoundWorker : IStoryCommandWorker
{
    public Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var sound = (SoundCommand)cmd;
        ctx.SendEvent(new VisualNovelSoundTriggeredEvent
        {
            SoundType = sound.SoundType,
            FilePath = sound.FilePath ?? string.Empty
        });
        return Task.CompletedTask;
    }
}
