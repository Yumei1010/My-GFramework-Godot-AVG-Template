using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.entities.story_command_worker;

public sealed class SoundWorker : FireAndForgetWorker<SoundCommand, VisualNovelSoundTriggeredEvent>
{
    protected override VisualNovelSoundTriggeredEvent CreateEvent(SoundCommand cmd) =>
        new() { SoundType = cmd.SoundType, FilePath = cmd.FilePath ?? string.Empty };
}
