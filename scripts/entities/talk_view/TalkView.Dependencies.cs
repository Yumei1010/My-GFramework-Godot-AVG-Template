using GFrameworkTemplate.global;

namespace GFrameworkTemplate.scripts.entities.talk_view;

public partial class TalkView
{
    private RichTextLabel TalkerNameLabel => GetNode<RichTextLabel>("%TalkerNameLabel");
    private RichTextLabel TalkContentLabel => GetNode<RichTextLabel>("%TalkContentLabel");
    private TextureRect TalkNameBackgroundTex => GetNode<TextureRect>("%TalkNameBackgroundTextureRect");
    private MarginContainer TalkBarContainer => GetNode<MarginContainer>("%TalkBarContainer");
    private Tween? _typewriter;

    private async Task ReadyAsync()
    {
        await GameEntryPoint.Architecture.WaitUntilReadyAsync().ConfigureAwait(false);

        Layer = 1;
        TalkBarContainer.Visible = false;
    }
}
