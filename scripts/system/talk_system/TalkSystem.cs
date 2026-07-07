using GFrameworkTemplate.scripts.cqrs.talk.command;
using GFrameworkTemplate.scripts.system.story_engine_system;
using GFrameworkTemplate.scripts.cqrs.talk.query;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.system.talk_system;

/// <summary>
///     对话系统——独立 ISystem，通过 ChangeTalkCommand 驱动
/// </summary>
[Log]
[ContextAware]
public sealed partial class TalkSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase)
    {
        _log.Debug("System initialized: TalkSystem");
    }
    public void Init()
    {
        
    }
    public void Destroy()
    {
        _log.Debug("System destroyed: TalkSystem");
    }

    public void Toggle() => this.SendCommand(new SetTalkVisibleCommand { Visible = !this.SendQuery(new GetTalkVisibleQuery()).Visible });
    public void Show() => this.SendCommand(new SetTalkVisibleCommand { Visible = true });
    public void Hide() => this.SendCommand(new SetTalkVisibleCommand { Visible = false });
    public bool Visible => this.SendQuery(new GetTalkVisibleQuery()).Visible;

    public async Task PlayAsync(string talker, string content, bool isCenter, string avatarPath)
    {
        this.SendEvent(new VisualNovelTalkPlayedEvent{Talker = talker, Content = content, IsCenter = isCenter, AvatarPath = avatarPath});

        await this.GetSystem<StoryEngineSystem>()!.WaitForAdvance();
    }
}
