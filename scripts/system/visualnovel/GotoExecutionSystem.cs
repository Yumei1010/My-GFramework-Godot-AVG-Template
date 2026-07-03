using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;

namespace GFrameworkTemplate.scripts.system.visualnovel;

[Log][ContextAware]
public sealed partial class GotoExecutionSystem : ISystem, IStoryExecutionSystem
{
    public string CommandType => "goto";
    public void OnArchitecturePhase(ArchitecturePhase phase) { } public void Init() { } public void Destroy() { }

    public Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var g = (GotoCommand)cmd;
        if (!string.IsNullOrEmpty(g.FilePath))
        {
            ctx.SendEvent(new VisualNovelGotoTriggeredEvent { TargetFilePath = g.FilePath });
            ctx.IsPlaying = false;
            ctx.PendingGoto = g.FilePath;
        }
        return Task.CompletedTask;
    }
}
