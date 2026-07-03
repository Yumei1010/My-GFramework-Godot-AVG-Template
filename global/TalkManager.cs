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
    private TextureRect TalkNameBackgroundTextureRect => GetNode<TextureRect>("%TalkNameBackgroundTextureRect");
    private MarginContainer TalkBarContainer => GetNode<MarginContainer>("%TalkBarContainer");

    /// <summary>全局单例引用</summary>
    public static TalkManager? Instance { get; private set; }

    /// <summary>对话框是否可见</summary>
    public bool IsVisible => TalkBarContainer.Visible;

    public override void _Ready()
    {
        Instance = this;
        Hide();
        this.RegisterEvent<VisualNovelTalkTriggeredEvent>(OnTalk).UnRegisterWhenNodeExitTree(this);
    }

    /// <summary>切换对话框显隐（欣赏背景 CG）</summary>
    public void Toggle()
    {
        TalkBarContainer.Visible = !TalkBarContainer.Visible;
    }

    /// <summary>仅隐藏对话框（保留 CanvasLayer 活跃）</summary>
    public new void Hide()
    {
        if (TalkBarContainer != null)
            TalkBarContainer.Visible = false;
    }

    /// <summary>显示对话框</summary>
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
