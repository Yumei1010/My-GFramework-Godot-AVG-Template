using GFrameworkTemplate.scripts.component.branch_option;
using GFrameworkTemplate.scripts.cqrs.story.command;
using GFrameworkTemplate.scripts.cqrs.story.query;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.system.branch_system;

/// <summary>
///     分支系统——独立 ISystem，通过 ChangeBranchCommand 驱动
/// </summary>
[Log]
[ContextAware]
public sealed partial class BranchSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    /// <summary>
    ///     显示分支选项，等待玩家选择，返回后自动更新 Model 的 TalkBranch
    /// </summary>
    public async Task ShowAsync(Dictionary<string, BranchOption> options)
    {
        this.SendEvent(new VisualNovelBranchShownEvent { Options = options });

        var tcs = new TaskCompletionSource<string?>();
        var sub = this.RegisterEvent<VisualNovelBranchChosenEvent>(e => tcs.TrySetResult(e.OptionId));
        var chosenId = await tcs.Task;
        sub.UnRegister();

        if (chosenId != null)
        {
            var state = this.SendQuery(new GetStoryStateQuery());
            var list = new List<string>(state.TalkBranch) { chosenId };
            this.SendCommand(new UpdateStoryStateCommand { TalkBranch = list });
        }
    }
}
