using GFrameworkTemplate.scripts.cqrs.talk.@event;

namespace GFrameworkTemplate.scripts.entities.talk_view;

public partial class TalkView
{
    private void RegisterEvent()
    {
        this.RegisterEvent<TalkPlayedEvent>(e =>
        {
            OnTalkPlayedEvent(e);
        }).UnRegisterWhenNodeExitTree(this);

        this.RegisterEvent<TalkTextRevealedEvent>(_ =>
        {
            OnTextRevealedEvent();
        }).UnRegisterWhenNodeExitTree(this);
    }

    private void OnTalkPlayedEvent(TalkPlayedEvent e)
    {
        TalkBarContainer.Visible = true;
        KillTypewriter();

        var content = e.IsCenter ? $"[center]{e.Content}[/center]" : e.Content;

        if (e.IsCenter)
        {
            TalkerNameLabel.Visible = false;
            TalkNameBackgroundTex.Visible = false;
        }
        else
        {
            TalkerNameLabel.Visible = true;
            TalkNameBackgroundTex.Visible = true;
            TalkerNameLabel.Text = e.Talker ?? "";
        }

        TalkContentLabel.Text = content;
        StartTypewriter(content.Length * e.RevealSpeed, content.Length);
    }

    private void OnTextRevealedEvent()
    {
        KillTypewriter();
        TalkContentLabel.VisibleCharacters = -1;
    }
}
