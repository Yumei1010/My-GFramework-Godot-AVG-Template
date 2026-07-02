using GFramework.Core.extensions;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using Godot;

namespace GFrameworkTemplate.global;

/// <summary>
///     对话栏全局单例——自动加载，响应 VN 对话事件
/// </summary>
[Log]
[ContextAware]
public partial class TalkManager : CanvasLayer
{
    private RichTextLabel TalkerNameLabel => GetNode<RichTextLabel>("%TalkerNameLabel");
    private RichTextLabel TalkContentLabel => GetNode<RichTextLabel>("%TalkContentLabel");
    private Control CenterTextContainer => GetNode<Control>("%CenterTextContainer");
    private RichTextLabel CenterTextContentLabel => GetNode<RichTextLabel>("%CenterTextContentLabel");
    private TextureRect TalkNameBackgroundTextureRect => GetNode<TextureRect>("%TalkNameBackgroundTextureRect");

    public override void _Ready()
    {
        Hide();
        this.RegisterEvent<VisualNovelTalkTriggeredEvent>(OnTalk).UnRegisterWhenNodeExitTree(this);
    }

    private void OnTalk(VisualNovelTalkTriggeredEvent e)
    {
        Show();

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
            TalkerNameLabel.Text = e.Talker ?? "";
            TalkContentLabel.Text = e.Content;
        }
    }

    public new void Hide()
    {
        base.Hide();
        CenterTextContainer.Visible = false;
    }
}
