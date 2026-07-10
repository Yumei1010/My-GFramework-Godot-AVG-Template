using GFrameworkTemplate.global;
using GFrameworkTemplate.scripts.utility;

namespace GFrameworkTemplate.scripts.entities.background_view;

public partial class BackgroundView
{
    private TextureRect MainBg => GetNode<TextureRect>("%MainBackgroundRect");
    private TextureRect HelperBg => GetNode<TextureRect>("%HelperBackgroundRect");
    private Tween? _tween;

    private async Task ReadyAsync()
    {
        await GameEntryPoint.Architecture.WaitUntilReadyAsync().ConfigureAwait(false);

        Layer = -2;
        HelperBg.Modulate = Colors.Transparent;
    }
}
