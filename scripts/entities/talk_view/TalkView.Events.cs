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
        CenterTextContainer.Visible = false;
        KillTypewriter();

        if (e.Code)
        {
            var codeContent = $"[center][bgcolor=#1a1a2e][code][color=#7ec8e3]{e.Content}[/color][/code][/bgcolor][/center]";
            TalkBarContainer.Visible = false;
            CenterTextContainer.Visible = true;
            CenterTextLabel.Text = codeContent;
            StartTypewriter(CenterTextLabel, e.Content.Length * e.RevealSpeed, e.Content.Length);
            return;
        }

        if (e.Center)
        {
            TalkBarContainer.Visible = false;
            CenterTextContainer.Visible = true;
            CenterTextLabel.Text = e.Content;
            StartTypewriter(CenterTextLabel, e.Content.Length * e.RevealSpeed, e.Content.Length);
            return;
        }

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
        StartTypewriter(TalkContentLabel, content.Length * e.RevealSpeed, content.Length);
    }

    private void OnTextRevealedEvent()
    {
        KillTypewriter();
        TalkContentLabel.VisibleCharacters = -1;
        CenterTextLabel.VisibleCharacters = -1;
    }
}
