using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.system.talk;

/// <summary>
///     对话栏——响应 VN 对话事件，通过 ISystem 注册到 GF 框架
/// </summary>
[Log]
[ContextAware]
public partial class TalkSystem : CanvasLayer, ISystem
    public static TalkSystem? Instance { get; private set; }
{
    private RichTextLabel TalkerNameLabel => GetNode<RichTextLabel>("%TalkerNameLabel");
    private RichTextLabel TalkContentLabel => GetNode<RichTextLabel>("%TalkContentLabel");
    private TextureRect TalkNameBackgroundTextureRect => GetNode<TextureRect>("%TalkNameBackgroundTextureRect");
    private MarginContainer TalkBarContainer => GetNode<MarginContainer>("%TalkBarContainer");

    public bool IsVisible => TalkBarContainer.Visible;

    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    public override void _Ready()
        Instance = this;
    {
        Hide();
        this.RegisterEvent<VisualNovelTalkTriggeredEvent>(OnTalk).UnRegisterWhenNodeExitTree(this);
    }

    public void Toggle()
    {
        TalkBarContainer.Visible = !TalkBarContainer.Visible;
    }

    public new void Hide()
    {
        if (TalkBarContainer != null)
            TalkBarContainer.Visible = false;
    }

    public new void Show()
    {
        if (TalkBarContainer != null)
            TalkBarContainer.Visible = true;
    }

    private void OnTalk(VisualNovelTalkTriggeredEvent e)
    {
        Show();
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
