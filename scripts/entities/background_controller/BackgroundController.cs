using GFrameworkTemplate.scripts.data.story;

namespace GFrameworkTemplate.scripts.entities.background_controller;

/// <summary>
///     背景控制器，双 TextureRect 交叉淡入淡出
/// </summary>
[Log]
[ContextAware]
public partial class BackgroundController : CanvasLayer
{
    private TextureRect MainBackgroundRect => GetNode<TextureRect>("%MainBackgroundRect");
    private TextureRect HelperBackgroundRect => GetNode<TextureRect>("%HelperBackgroundRect");
    private Tween? _tween;

    public override void _Ready()
    {
        HelperBackgroundRect.Modulate = Colors.Transparent;
    }

    public async Task Change(string filePath = "", bool waitTweenEnd = true)
    {
        var path = StoryResourceMapper.ResolveTexturePath(filePath);
        if (string.IsNullOrEmpty(path)) return;

        var texture = GD.Load<Texture2D>(path);
        if (texture == null) return;

        _tween?.Kill();

        if (waitTweenEnd)
        {
            HelperBackgroundRect.Texture = texture;
            HelperBackgroundRect.Modulate = Colors.Transparent;
            _tween = CreateTween();
            _tween.TweenProperty(HelperBackgroundRect, "modulate", Colors.White, 0.5f);
            _tween.TweenCallback(Callable.From(() =>
            {
                MainBackgroundRect.Texture = texture;
                HelperBackgroundRect.Modulate = Colors.Transparent;
            }));
        }
        else
        {
            MainBackgroundRect.Texture = texture;
        }
    }
}
