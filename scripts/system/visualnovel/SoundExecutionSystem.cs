using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;

namespace GFrameworkTemplate.scripts.system.visualnovel;

[Log][ContextAware]
public sealed partial class SoundExecutionSystem : ISystem, IStoryExecutionSystem
{
    public string CommandType => "sound";
    public void OnArchitecturePhase(ArchitecturePhase phase) { } public void Init() { } public void Destroy() { }

    public Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var s = (SoundCommand)cmd;
        ctx.SendEvent(new VisualNovelSoundTriggeredEvent { SoundType = s.SoundType, FilePath = s.FilePath ?? "" });
        return Task.CompletedTask;
    }
}
