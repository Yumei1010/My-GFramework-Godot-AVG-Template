using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.data.story;

namespace GFrameworkTemplate.scripts.component.background_controller;

/// <summary>
///     背景控制器——CanvasLayer 场景节点，双 TextureRect 交叉淡入淡出
/// </summary>
[Log]
[ContextAware]
public partial class BackgroundController : CanvasLayer
{
    private TextureRect MainBg => GetNode<TextureRect>("%MainBg");
    private TextureRect HelperBg => GetNode<TextureRect>("%HelperBg");
    private Tween? _tween;

    public override void _Ready()
    {
        HelperBg.Modulate = Colors.Transparent;
        this.RegisterEvent<VisualNovelBackgroundChangedEvent>(OnBackground).UnRegisterWhenNodeExitTree(this);
    }

    private async void OnBackground(VisualNovelBackgroundChangedEvent e)
    {
        var path = StoryResourceMapper.ResolveTexturePath(e.FilePath);
        if (string.IsNullOrEmpty(path)) return;

        var texture = GD.Load<Texture2D>(path);
        if (texture == null) return;

        _tween?.Kill();

        if (e.Delay > 0)
            await Task.Delay(TimeSpan.FromSeconds(e.Delay));

        if (e.WaitTweenEnd)
        {
            HelperBg.Texture = texture;
            HelperBg.Modulate = Colors.Transparent;
            _tween = CreateTween();
            _tween.TweenProperty(HelperBg, "modulate", Colors.White, 0.5f);
            await ToSignal(_tween, Tween.SignalName.Finished);
            MainBg.Texture = texture;
            HelperBg.Modulate = Colors.Transparent;
        }
        else
        {
            MainBg.Texture = texture;
        }
    }
}
