using GFrameworkTemplate.scripts.cqrs.story.command;
using GFrameworkTemplate.scripts.system.goto_system;

namespace GFrameworkTemplate.scripts.cqrs.@goto.command;

/// <summary>
///     GotoNavigateCommand —— 跳转到另一个 JSON 脚本
/// </summary>
public sealed class GotoNavigateCommand : AbstractAsyncCommand
{
    public required string TargetPath { get; set; }

    protected override async Task OnExecuteAsync()
    {
        this.GetSystem<GotoSystem>()!.Navigate(TargetPath);
        this.SendCommand(new StorySetPlayingCommand { Playing = false });
        this.SendCommand(new StorySetGotoCommand { GotoTarget = TargetPath });
        await Task.CompletedTask;
    }
}
