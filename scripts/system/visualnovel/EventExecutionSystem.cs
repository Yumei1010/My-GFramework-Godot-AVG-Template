using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;

namespace GFrameworkTemplate.scripts.system.visualnovel;

[Log][ContextAware]
public sealed partial class EventExecutionSystem : ISystem, IStoryExecutionSystem
{
    public string CommandType => "event";
    public void OnArchitecturePhase(ArchitecturePhase phase) { } public void Init() { } public void Destroy() { }

    public async Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var e = (EventCommand)cmd;
        ctx.SendEvent(new VisualNovelCustomEventTriggeredEvent { EventName = e.EventName });
        await ctx.AdvanceAsync(0.1f);
    }
}
