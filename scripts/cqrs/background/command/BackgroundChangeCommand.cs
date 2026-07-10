using GFrameworkTemplate.scripts.cqrs.background.command.input;
using GFrameworkTemplate.scripts.system.background_system;

namespace GFrameworkTemplate.scripts.cqrs.background.command;

public sealed class BackgroundChangeCommand(BackgroundChangeInput input) : AbstractAsyncCommand<BackgroundChangeInput>(input)
{
    protected override async Task OnExecuteAsync(BackgroundChangeInput input)
    {
        await this.GetSystem<BackgroundSystem>().Change(input.FilePath, input.WaitTweenEnd, input.Delay);
    }
}
