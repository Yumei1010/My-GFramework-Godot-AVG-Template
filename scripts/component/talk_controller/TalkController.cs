using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.system.talk;

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

    private TalkSystem _system = null!;

    public override void _Ready()
    {
        _system = this.GetSystem<TalkSystem>()!;
        TalkBarContainer.Visible = false;

        this.RegisterEvent<VisualNovelTalkTriggeredEvent>(OnTalk).UnRegisterWhenNodeExitTree(this);
    }

    public override void _Process(double delta)
    {
        TalkBarContainer.Visible = _system.Visible;
    }

    private void OnTalk(VisualNovelTalkTriggeredEvent e)
    {
        _system.Show();
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
