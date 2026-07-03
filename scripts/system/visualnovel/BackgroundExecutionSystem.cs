using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;

namespace GFrameworkTemplate.scripts.system.visualnovel;

[Log][ContextAware]
public sealed partial class BackgroundExecutionSystem : ISystem, IStoryExecutionSystem
{
    public string CommandType => "background";
    public void OnArchitecturePhase(ArchitecturePhase phase) { } public void Init() { } public void Destroy() { }

    public async Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var b = (BackgroundCommand)cmd;
        if (b.Delay > 0) await Task.Delay(TimeSpan.FromSeconds(b.Delay));
        ctx.SendEvent(new VisualNovelBackgroundTriggeredEvent { FilePath = b.FilePath ?? "", WaitTweenEnd = b.WaitTweenEnd, Delay = b.Delay });
        if (b.WaitTweenEnd) await Task.Delay(TimeSpan.FromSeconds(0.5f));
    }
}
