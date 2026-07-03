using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.cqrs.talk.command;
using GFrameworkTemplate.scripts.cqrs.talk.query;
using GFrameworkTemplate.scripts.cqrs.talk.query.result;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.system.visualnovel;

/// <summary>
///     对话系统——状态管理 + 故事命令执行
/// </summary>
[Log]
[ContextAware]
public sealed partial class TalkSystem : ISystem, IStoryExecutionSystem
{
    public string CommandType => "talk";
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    public void Toggle() =>
        this.SendCommand(new SetTalkVisibleCommand
            { Visible = !this.SendQuery(new GetTalkVisibleQuery()).Visible });

    public void Show() => this.SendCommand(new SetTalkVisibleCommand { Visible = true });
    public void Hide() => this.SendCommand(new SetTalkVisibleCommand { Visible = false });
    public bool Visible => this.SendQuery(new GetTalkVisibleQuery()).Visible;

    async Task IStoryExecutionSystem.ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var t = (TalkCommand)cmd;
        ctx.SendEvent(new VisualNovelTalkTriggeredEvent { Talker = t.Talker, Content = t.TalkContent, IsCenter = t.IsCenter, AvatarPath = t.AvatarPath });
        await ctx.AdvanceAsync(t.TalkContent.Length * ctx.WordSpeed);
    }
}
