using GFrameworkTemplate.scripts.cqrs.background.command.input;
using GFrameworkTemplate.scripts.system.background_system;

namespace GFrameworkTemplate.scripts.cqrs.background.command;

/// <summary>
///     BackgroundChangeCommand —— 切换场景背景
/// </summary>
public sealed class BackgroundChangeCommand(BackgroundChangeInput input) : AbstractAsyncCommand<BackgroundChangeInput>(input)
{
    protected override async Task OnExecuteAsync(BackgroundChangeInput input)
    {
        await this.GetSystem<BackgroundSystem>().Change(input.FilePath, input.WaitTweenEnd, input.Delay);
    }
}
