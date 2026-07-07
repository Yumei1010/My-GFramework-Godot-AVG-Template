using GFramework.Core.command;
using GFrameworkTemplate.scripts.cqrs.talk.command.input;
using GFrameworkTemplate.scripts.system.talk_system;

namespace GFrameworkTemplate.scripts.cqrs.talk.command;

/// <summary>
///     播放对话命令——通过 TalkSystem 显示对话并等待玩家推进
/// </summary>
/// <param name="input">对话命令输入参数</param>
public sealed class ChangeTalkCommand(ChangeTalkCommandInput input)
    : AbstractAsyncCommand<ChangeTalkCommandInput>(input)
{
    protected override async Task OnExecuteAsync(ChangeTalkCommandInput input)
    {
        await this.GetSystem<TalkSystem>()!.PlayAsync(
            input.Talker, input.Content, input.IsCenter, input.AvatarPath);
    }
}
