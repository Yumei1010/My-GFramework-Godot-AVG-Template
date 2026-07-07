using GFrameworkTemplate.scripts.cqrs.story.command;
using GFrameworkTemplate.scripts.system.goto_system;

namespace GFrameworkTemplate.scripts.cqrs.@goto.command;

/// <summary>
///     跳转命令——通过 GotoSystem 导航到目标脚本
/// </summary>
public sealed class ChangeGotoCommand : AbstractAsyncCommand
{
    public required string TargetPath { get; set; }

    protected override async Task OnExecuteAsync()
    {
        this.GetSystem<GotoSystem>()!.Navigate(TargetPath);
        this.SendCommand(new UpdateStoryStateCommand
        {
            IsPlaying = false,
            PendingGoto = TargetPath
        });
        await Task.CompletedTask;
    }
}
