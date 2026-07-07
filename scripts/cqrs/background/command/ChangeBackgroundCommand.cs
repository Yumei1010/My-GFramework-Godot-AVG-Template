using GFrameworkTemplate.scripts.cqrs.background.command.input;
using GFrameworkTemplate.scripts.system.background_system;

namespace GFrameworkTemplate.scripts.cqrs.background.command;

/// <summary>
///     切换背景命令——通过 BackgroundSystem 驱动背景变换
/// </summary>
/// <param name="input">背景切换命令输入参数</param>
public sealed class ChangeBackgroundCommand(ChangeBackgroundCommandInput input): AbstractAsyncCommand<ChangeBackgroundCommandInput>(input)
{
    protected override async Task OnExecuteAsync(ChangeBackgroundCommandInput input)
    {
        await this.GetSystem<BackgroundSystem>().Change(input.FilePath, input.WaitTweenEnd, input.Delay);
    }
}
