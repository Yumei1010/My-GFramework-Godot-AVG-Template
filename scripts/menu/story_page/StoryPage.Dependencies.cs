using GFrameworkTemplate.global;

namespace GFrameworkTemplate.scripts.menu.story_page;

public partial class StoryPage
{
    private Label TalkerName => GetNode<Label>("%TalkerName");
    private Label TalkContent => GetNode<Label>("%TalkContent");
    private Label CenterContent => GetNode<Label>("%CenterContent");
    private TextureRect Avatar => GetNode<TextureRect>("%Avatar");
    private Control ClickArea => GetNode<Control>("%ClickArea");
    private Tween? _typewriterTween;

    private async Task ReadyAsync()
    {
        await GameEntryPoint.Architecture.WaitUntilReadyAsync().ConfigureAwait(false);
        _log.Debug("StoryPage 初始化完成");
    }
}
