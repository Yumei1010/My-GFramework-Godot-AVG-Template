using GFrameworkTemplate.scripts.utility;

namespace GFrameworkTemplate.scripts.entities.background_view;

[Log]
[ContextAware]
/// <summary>
///     背景 View —— 双 TextureRect 交叉淡入淡出
/// </summary>
public partial class BackgroundView : CanvasLayer
{
    public override void _Ready()
    {
        _ = ReadyAsync();
    }

    public async Task Change(string filePath = "", bool waitTweenEnd = true)
    {
        var tex = this.GetUtility<IGodotTextureRegistry>()!.Get(filePath) as Texture2D;
        if (tex == null) return;

        if (_tween != null && _tween.IsRunning()) _tween.Kill();

        if (waitTweenEnd)
        {
            HelperBg.Texture = tex;
            HelperBg.Modulate = Colors.Transparent;
            _tween = CreateTween();
            _tween.TweenProperty(HelperBg, "modulate", Colors.White, 0.5f);
            _tween.TweenCallback(Callable.From(() =>
            {
                MainBg.Texture = tex;
                HelperBg.Modulate = Colors.Transparent;
            }));
        }
        else
        {
            MainBg.Texture = tex;
        }
    }
}
