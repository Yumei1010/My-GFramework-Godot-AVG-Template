using GFrameworkTemplate.scripts.cqrs.talk.command.input;
using GFrameworkTemplate.scripts.system.talk_system;

namespace GFrameworkTemplate.scripts.cqrs.talk.command;

public sealed class TalkPlayCommand(TalkPlayInput input) : AbstractAsyncCommand<TalkPlayInput>(input)
{
    protected override async Task OnExecuteAsync(TalkPlayInput input)
    {
        await this.GetSystem<TalkSystem>()!.PlayAsync(input.Talker, input.Content, input.IsCenter, input.RevealSpeed);
    }
}
