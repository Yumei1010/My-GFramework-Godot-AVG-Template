using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.entities.story_command_worker;

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

        await ctx.AdvanceAsync(talk.TalkContent.Length * ctx.WordSpeed);
    }
}
