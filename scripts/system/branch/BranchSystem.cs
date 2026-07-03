using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.system.branch;

/// <summary>
///     分支系统——分支选择 + 故事命令执行
/// </summary>
[Log]
[ContextAware]
public sealed partial class BranchSystem : ISystem, IStoryExecutionSystem
{
    public string CommandType => "branch";
    private StoryEngineSystem _engine = null!;
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { _engine = this.GetSystem<StoryEngineSystem>()!; }
    public void Destroy() { }

    public void Choose(string optionId) => _engine.ChooseBranch(optionId);

    async Task IStoryExecutionSystem.ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var b = (BranchCommand)cmd;
        ctx.SendEvent(new VisualNovelBranchTriggeredEvent { Options = b.Options });
        var tcs = new TaskCompletionSource<string?>();
        var sub = ctx.RegisterEvent<VisualNovelBranchChosenEvent>(e => tcs.TrySetResult(e.OptionId));
        var chosenId = await tcs.Task;
        sub.UnRegister();
        if (chosenId != null) ctx.TalkBranch.Add(chosenId);
    }
}
