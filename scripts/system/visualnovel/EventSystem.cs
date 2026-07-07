using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.system.visualnovel;

/// <summary>
///     事件系统——自定义事件命令执行
/// </summary>
[Log][ContextAware]
public sealed partial class EventSystem : ISystem, IStoryExecutionSystem
{
    public string CommandType => "event";
    public void OnArchitecturePhase(ArchitecturePhase phase) { } public void Init() { } public void Destroy() { }

    async Task IStoryExecutionSystem.ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var e = (EventCommand)cmd;
        ctx.SendEvent(new VisualNovelCustomEventFiredEvent { EventName = e.EventName });
        await ctx.WaitClickAsync();
    }
}
