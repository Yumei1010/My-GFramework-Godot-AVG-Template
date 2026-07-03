using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.background.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.system.visualnovel;

/// <summary>
///     背景系统——路径管理 + 故事命令执行
/// </summary>
[Log]
[ContextAware]
public sealed partial class BackgroundSystem : ISystem, IStoryExecutionSystem
{
    public string CommandType => "background";
    public void OnArchitecturePhase(ArchitecturePhase phase) { } public void Init() { } public void Destroy() { }

    public void Change(string filePath) =>
        this.SendCommand(new SetBackgroundCommand { FilePath = filePath });

    async Task IStoryExecutionSystem.ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var b = (BackgroundCommand)cmd;
        if (b.Delay > 0) await Task.Delay(TimeSpan.FromSeconds(b.Delay));
        ctx.SendEvent(new VisualNovelBackgroundTriggeredEvent { FilePath = b.FilePath ?? "", WaitTweenEnd = b.WaitTweenEnd, Delay = b.Delay });
        if (b.WaitTweenEnd) await Task.Delay(TimeSpan.FromSeconds(0.5f));
    }
}
