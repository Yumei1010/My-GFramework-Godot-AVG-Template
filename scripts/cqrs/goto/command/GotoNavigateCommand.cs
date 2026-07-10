using GFrameworkTemplate.scripts.cqrs.story.command;
using GFrameworkTemplate.scripts.system.goto_system;

namespace GFrameworkTemplate.scripts.cqrs.@goto.command;

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
