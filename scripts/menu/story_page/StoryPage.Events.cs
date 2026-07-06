using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.menu.story_page;

public partial class StoryPage
{
    private void RegisterEvents()
    {
        this.RegisterEvent<VisualNovelTalkPlayedEvent>(e =>
        {
            TalkerName.Text = e.Talker ?? "";
            TalkerName.Visible = !e.IsCenter;
            Avatar.Visible = !e.IsCenter;
            CenterContent.Visible = e.IsCenter;

            if (e.IsCenter)
                PlayTypewriter(CenterContent, e.Content);
            else
                PlayTypewriter(TalkContent, e.Content);
        }).UnRegisterWhenNodeExitTree(this);

        this.RegisterEvent<VisualNovelBackgroundChangedEvent>(e =>
            _log.Debug($"背景切换: {e.FilePath}")
        ).UnRegisterWhenNodeExitTree(this);

        this.RegisterEvent<VisualNovelBranchShownEvent>(e =>
            _log.Debug($"分支选项: {e.Options.Count} 个选项")
        ).UnRegisterWhenNodeExitTree(this);

        this.RegisterEvent<VisualNovelAdvanceRequestedEvent>(_ =>
        {
            if (_typewriterTween?.IsRunning() == true)
                SkipTypewriter();
        }).UnRegisterWhenNodeExitTree(this);
    }

    private void PlayTypewriter(Label label, string content)
    {
        _typewriterTween?.Kill();
        label.Text = content;
        label.VisibleRatio = 0f;
        _typewriterTween = CreateTween();
        _typewriterTween.TweenProperty(label, "visible_ratio", 1.0f, content.Length * 0.02f);
    }

    private void SkipTypewriter()
    {
        _typewriterTween?.Kill();
        _typewriterTween = null;
        TalkContent.VisibleRatio = 1f;
        CenterContent.VisibleRatio = 1f;
    }
}
