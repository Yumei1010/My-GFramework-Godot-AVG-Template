using GFrameworkTemplate.global;

namespace GFrameworkTemplate.scripts.entities.tachie_view;

public partial class TachieView
{
    private TextureRect LeftSlot => GetNode<TextureRect>("%LeftSlot");
    private TextureRect CenterSlot => GetNode<TextureRect>("%CenterSlot");
    private TextureRect RightSlot => GetNode<TextureRect>("%RightSlot");
    private TextureRect HelperSlot => GetNode<TextureRect>("%HelperSlot");

    private readonly Dictionary<string, TextureRect> _slotMap = new();
    private readonly Dictionary<string, string> _slotChars = new();

    private async Task ReadyAsync()
    {
        await GameEntryPoint.Architecture.WaitUntilReadyAsync().ConfigureAwait(false);

        Layer = -1;
        _slotMap["Left"] = LeftSlot;
        _slotMap["Center"] = CenterSlot;
        _slotMap["Right"] = RightSlot;

        foreach (var r in new[] { LeftSlot, CenterSlot, RightSlot, HelperSlot })
            r.Visible = false;
    }
}
