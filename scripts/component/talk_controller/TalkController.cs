using GFrameworkTemplate.scripts.cqrs.talk.command;
using GFrameworkTemplate.scripts.cqrs.talk.query;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.component.talk_controller;

/// <summary>
///     对话栏控制器——CanvasLayer 场景节点，渲染对话 UI
/// </summary>
[Log]
[ContextAware]
public partial class TalkController : CanvasLayer
{
    private RichTextLabel TalkerNameLabel => GetNode<RichTextLabel>("%TalkerNameLabel");
    private RichTextLabel TalkContentLabel => GetNode<RichTextLabel>("%TalkContentLabel");
    private TextureRect TalkNameBackgroundTextureRect => GetNode<TextureRect>("%TalkNameBackgroundTextureRect");
    private MarginContainer TalkBarContainer => GetNode<MarginContainer>("%TalkBarContainer");

    public override void _Ready()
    {
        TalkBarContainer.Visible = false;

        this.RegisterEvent<VisualNovelTalkPlayedEvent>(OnTalk).UnRegisterWhenNodeExitTree(this);
    }

    public override void _Process(double delta)
    {
        TalkBarContainer.Visible = this.SendQuery(new GetTalkVisibleQuery()).Visible;
    }

    private void OnTalk(VisualNovelTalkPlayedEvent e)
    {
        this.SendCommand(new SetTalkVisibleCommand { Visible = true });
        TalkContentLabel.Visible = true;

        if (e.IsCenter)
        {
            TalkerNameLabel.Visible = false;
            TalkNameBackgroundTextureRect.Visible = false;
            TalkContentLabel.Text = $"[center]{e.Content}[/center]";
        }
        else
        {
            TalkerNameLabel.Visible = true;
            TalkNameBackgroundTextureRect.Visible = true;
            TalkerNameLabel.Text = e.Talker ?? "";
            TalkContentLabel.Text = e.Content;
        }
    }
}
