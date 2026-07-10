using GFrameworkTemplate.scripts.cqrs.talk.command.input;
using GFrameworkTemplate.scripts.system.talk_system;

namespace GFrameworkTemplate.scripts.cqrs.talk.command;

/// <summary>
///     TalkPlayCommand —— 播放对话并等待玩家推进
/// </summary>
public sealed class TalkPlayCommand(TalkPlayInput input) : AbstractAsyncCommand<TalkPlayInput>(input)
{
    protected override async Task OnExecuteAsync(TalkPlayInput input)
    {
        await this.GetSystem<TalkSystem>()!.PlayAsync(input.Talker, input.Content, input.IsCenter, input.RevealSpeed);
    }
}
