using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;

namespace GFrameworkTemplate.scripts.system.visualnovel;

[Log][ContextAware]
public sealed partial class TalkExecutionSystem : ISystem, IStoryExecutionSystem
{
    public string CommandType => "talk";
    public void OnArchitecturePhase(ArchitecturePhase phase) { } public void Init() { } public void Destroy() { }

    public async Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var t = (TalkCommand)cmd;
        ctx.SendEvent(new VisualNovelTalkTriggeredEvent { Talker = t.Talker, Content = t.TalkContent, IsCenter = t.IsCenter, AvatarPath = t.AvatarPath });
        await ctx.AdvanceAsync(t.TalkContent.Length * ctx.WordSpeed);
    }
}
