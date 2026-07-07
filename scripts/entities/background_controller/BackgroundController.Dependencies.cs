using GFrameworkTemplate.global;

namespace GFrameworkTemplate.scripts.entities.background_controller;

public partial class BackgroundController : CanvasLayer
{
    private TextureRect MainBackgroundRect => GetNode<TextureRect>("%MainBackgroundRect");
    private TextureRect HelperBackgroundRect => GetNode<TextureRect>("%HelperBackgroundRect");
    private Tween _tween;

    private async Task ReadyAsync()
    {
        await GameEntryPoint.Architecture.WaitUntilReadyAsync().ConfigureAwait(false);

        HelperBackgroundRect.Modulate = Colors.Transparent;
    }
}