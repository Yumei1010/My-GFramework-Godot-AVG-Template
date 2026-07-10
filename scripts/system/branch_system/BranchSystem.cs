using GFrameworkTemplate.scripts.component.branch_option;
using GFrameworkTemplate.scripts.cqrs.branch.@event;
using GFrameworkTemplate.scripts.cqrs.story.command;
using GFrameworkTemplate.scripts.cqrs.story.query;

namespace GFrameworkTemplate.scripts.system.branch_system;

/// <summary>
///     分支系统——显示选项，等待玩家选择，更新分支状态
/// </summary>
[Log]
[ContextAware]
public sealed partial class BranchSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase)
    {
        _log.Debug("System initialized: BranchSystem");
    }

    public void Init()
    {
        
    }

    public void Destroy()
    {
        _log.Debug("System destroyed: BranchSystem");
    }

    public async Task ShowAsync(Dictionary<string, BranchOption> options)
    {
        this.SendEvent(new BranchShownEvent { Options = options });

        var tcs = new TaskCompletionSource<string?>();
        var sub = this.RegisterEvent<BranchChosenEvent>(e => tcs.TrySetResult(e.OptionId));
        var chosenId = await tcs.Task;
        sub.UnRegister();

        if (chosenId != null)
        {
            var state = this.SendQuery(new GetStoryStateQuery());
            var list = new List<string>(state.TalkBranch) { chosenId };
            this.SendCommand(new StorySetBranchCommand { Branches = list });
        }
    }
}
