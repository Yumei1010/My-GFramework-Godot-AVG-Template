using GFramework.Core.extensions;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using Godot;

namespace GFrameworkTemplate.scripts.component.talk_bar;

/// <summary>
///     对话栏控制器——响应 VN 对话事件，驱动说话人、内容、居中旁白的显示
/// </summary>
[Log]
[ContextAware]
public partial class TalkBarController : CanvasLayer
{
    private RichTextLabel TalkerNameLabel => GetNode<RichTextLabel>("%TalkerNameLabel");
    private RichTextLabel TalkContentLabel => GetNode<RichTextLabel>("%TalkContentLabel");
    private Control CenterTextContainer => GetNode<Control>("%CenterTextContainer");
    private RichTextLabel CenterTextContentLabel => GetNode<RichTextLabel>("%CenterTextContentLabel");
    private TextureRect TalkNameBackgroundTextureRect => GetNode<TextureRect>("%TalkNameBackgroundTextureRect");

    private Tween? _typewriterTween;

    public override void _Ready()
    {
        this.RegisterEvent<VisualNovelTalkTriggeredEvent>(OnTalk).UnRegisterWhenNodeExitTree(this);
        CenterTextContainer.Visible = false;
    }

    private void OnTalk(VisualNovelTalkTriggeredEvent e)
    {
        _typewriterTween?.Kill();

        if (e.IsCenter)
        {
            TalkContentLabel.Visible = false;
            TalkerNameLabel.Visible = false;
            TalkNameBackgroundTextureRect.Visible = false;
            CenterTextContainer.Visible = true;
            CenterTextContentLabel.Text = e.Content;
        }
        else
        {
            CenterTextContainer.Visible = false;
            TalkContentLabel.Visible = true;
            TalkerNameLabel.Visible = true;
            TalkNameBackgroundTextureRect.Visible = true;
            TalkerNameLabel.Text = $"[center]{e.Talker ?? ""}[/center]";
            TalkContentLabel.Text = e.Content;
        }
    }
}
