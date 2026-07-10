using GFrameworkTemplate.scripts.cqrs.talk.command;
using GFrameworkTemplate.global;
using GFrameworkTemplate.scripts.cqrs.talk.@event;
using GFrameworkTemplate.scripts.model.talk;

namespace GFrameworkTemplate.scripts.system.talk_system;

/// <summary>
///     对话系统——管理对话播放与打字机效果
/// </summary>
[Log]
[ContextAware]
/// <summary>
///     对话系统 —— 打字机状态管理与播放控制
/// </summary>
public sealed partial class TalkSystem : ISystem
{
    public bool Visible => this.GetModel<TalkModel>()!.Visible;
    public bool IsTextRevealed => this.GetModel<TalkModel>()!.Revealed;

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

    public void Toggle()
    {
        this.SendCommand(new TalkSetVisibleCommand { Visible = !this.GetModel<TalkModel>()!.Visible });
    }

    public void Show()
    {
        this.SendCommand(new TalkSetVisibleCommand { Visible = true });
    }

    public void Hide()
    {
        this.SendCommand(new TalkSetVisibleCommand { Visible = false });
    }

    /// <summary>播放对话，启动打字机并等待玩家点击推进</summary>
    public async Task PlayAsync(string talker, string content, bool isCenter, float revealSpeed)
    {
        this.GetModel<TalkModel>()!.Revealed = false;
        this.SendEvent(new TalkPlayedEvent
        {
            Talker = talker, Content = content, IsCenter = isCenter, RevealSpeed = revealSpeed
        });

        await this.GetSystem<StoryEngine>()!.WaitForAdvance();
    }

    /// <summary>跳过打字机效果，立即显示全部文本</summary>
    public void RevealAll()
    {
        if (this.GetModel<TalkModel>()!.Revealed) return;
        this.GetModel<TalkModel>()!.Revealed = true;
        this.SendEvent<TalkTextRevealedEvent>();
    }
}
