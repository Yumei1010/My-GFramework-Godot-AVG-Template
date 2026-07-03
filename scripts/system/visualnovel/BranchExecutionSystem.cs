using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;

namespace GFrameworkTemplate.scripts.system.visualnovel;

[Log][ContextAware]
public sealed partial class BranchExecutionSystem : ISystem, IStoryExecutionSystem
{
    public string CommandType => "branch";
    public void OnArchitecturePhase(ArchitecturePhase phase) { } public void Init() { } public void Destroy() { }

    public async Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
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
