using GFramework.Core.extensions;
using GFrameworkTemplate.global;
using Godot;

namespace GFrameworkTemplate.scripts.menu;

public partial class VisualNovelTalkPage
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
        _log.Debug("VisualNovelTalkPage 初始化完成");
    }
}
