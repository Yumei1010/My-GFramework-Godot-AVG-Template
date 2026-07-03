using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;

namespace GFrameworkTemplate.scripts.system.visualnovel;

[Log][ContextAware]
public sealed partial class TachieExecutionSystem : ISystem, IStoryExecutionSystem
{
    public string CommandType => "tachie";
    public void OnArchitecturePhase(ArchitecturePhase phase) { } public void Init() { } public void Destroy() { }

    public Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var t = (TachieCommand)cmd;
        ctx.SendEvent(new VisualNovelTachieTriggeredEvent { Tachies = t.Tachies });
        return Task.CompletedTask;
    }
}
