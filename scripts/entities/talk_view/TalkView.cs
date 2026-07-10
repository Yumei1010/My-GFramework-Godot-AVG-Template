using GFrameworkTemplate.scripts.cqrs.talk.command;

namespace GFrameworkTemplate.scripts.entities.talk_view;

[Log]
[ContextAware]
/// <summary>
///     对话 View —— 打字机效果与对话 UI 渲染
/// </summary>
public partial class TalkView : CanvasLayer
{
    public override void _Ready()
    {
        _ = ReadyAsync();
        RegisterEvent();
    }

    private void StartTypewriter(float duration, int totalChars)
    {
        TalkContentLabel.VisibleCharacters = 0;
        _typewriter = CreateTween();
        _typewriter.TweenProperty(TalkContentLabel, "visible_characters", totalChars, duration);
        _typewriter.TweenCallback(Callable.From(() =>
        {
            this.SendCommand(new TalkSetRevealedCommand { Revealed = true });
            _typewriter = null;
        }));
    }

    private void KillTypewriter()
    {
        _typewriter?.Kill();
        _typewriter = null;
    }
}
