using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.system.visualnovel;

/// <summary>
///     跳转系统——Goto 命令执行
/// </summary>
[Log][ContextAware]
public sealed partial class GotoSystem : ISystem, IStoryExecutionSystem
{
    public string CommandType => "goto";
    public void OnArchitecturePhase(ArchitecturePhase phase) { } public void Init() { } public void Destroy() { }

    async Task IStoryExecutionSystem.ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var g = (GotoCommand)cmd;
        if (!string.IsNullOrEmpty(g.FilePath))
        {
            ctx.SendEvent(new VisualNovelGotoNavigatedEvent { TargetFilePath = g.FilePath });
            ctx.IsPlaying = false;
            ctx.PendingGoto = g.FilePath;
        }
    }
}
