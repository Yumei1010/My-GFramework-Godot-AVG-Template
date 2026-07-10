using GFrameworkTemplate.scripts.component.tachie_slot;

namespace GFrameworkTemplate.scripts.entities.tachie_view;

[Log]
[ContextAware]
public partial class TachieView : CanvasLayer
{
    public override void _Ready()
    {
        _ = ReadyAsync();
        RegisterEvent();
    }

 private void AssignSlot(string charName, string slot)
    {
        RemoveFromSlot(charName);
        _slotChars[slot] = charName;
    }

    private void RemoveFromSlot(string charName)
    {
        var key = _slotChars.FirstOrDefault(kv => kv.Value == charName).Key;
        if (key != null) _slotChars.Remove(key);
    }

    private string NextAutoSlot()
    {
        if (!_slotChars.ContainsKey("Left")) return "Left";
        if (!_slotChars.ContainsKey("Right")) return "Right";
        return "Center";
    }

    private static string GetFilePath(string charName, Dictionary<string, TachieSlot> tachies)
    {
        if (tachies.TryGetValue(charName, out var slot) && !string.IsNullOrEmpty(slot.FilePath))
            return slot.FilePath;
        return charName;
    }

    private async Task CrossfadeSlot(TextureRect rect, Texture2D newTex)
    {
        HelperSlot.Texture = newTex;
        HelperSlot.GlobalPosition = rect.GlobalPosition;
        HelperSlot.Size = rect.Size;
        HelperSlot.Modulate = Colors.Transparent;
        HelperSlot.Visible = true;

        var tween = CreateTween();
        tween.TweenProperty(HelperSlot, "modulate", Colors.White, 0.3f);
        tween.Parallel().TweenProperty(rect, "modulate", Colors.Transparent, 0.3f);
        await ToSignal(tween, Tween.SignalName.Finished);

        rect.Texture = newTex;
        rect.Modulate = Colors.White;
        rect.Visible = true;
        HelperSlot.Visible = false;
    }
}
