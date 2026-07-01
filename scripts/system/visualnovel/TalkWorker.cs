using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.system.visualnovel;

public sealed class TalkWorker : IStoryCommandWorker
{
    public async Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var talk = (TalkCommand)cmd;

        ctx.SendEvent(new VisualNovelTalkTriggeredEvent
        {
            Talker = talk.Talker,
            Content = talk.TalkContent,
            IsCenter = talk.IsCenter,
            AvatarPath = talk.AvatarPath
        });

        ctx.SendEvent(new VisualNovelTextRevealProgressEvent
        {
            RevealedChars = 0,
            TotalChars = talk.TalkContent.Length
        });

        await EngineAwait.Advance(talk.TalkContent.Length * ctx.WordSpeed, ctx);
    }
}
