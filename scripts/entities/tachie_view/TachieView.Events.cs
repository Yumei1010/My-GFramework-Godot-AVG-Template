using GFrameworkTemplate.scripts.component.tachie_slot;
using GFrameworkTemplate.scripts.cqrs.tachie.@event;
using GFrameworkTemplate.scripts.enums.visualnovel;
using GFrameworkTemplate.scripts.utility;

namespace GFrameworkTemplate.scripts.entities.tachie_view;

public partial class TachieView
{
    private void RegisterEvent()
    {
        this.RegisterEvent<TachieUpdatedEvent>(e =>
        {
            OnTachieUpdatedEvent(e.Tachies);
        })
        .UnRegisterWhenNodeExitTree(this);
    }

    private async void OnTachieUpdatedEvent(Dictionary<string, TachieSlot> tachies)
    {
        var oldSlots = new Dictionary<string, string>(_slotChars);

        foreach (var (charName, slot) in tachies)
        {
            switch (slot.Type)
            {
                case TachieOperation.Close:    RemoveFromSlot(charName); break;
                case TachieOperation.OnlyShow: AssignSlot(charName, slot.Slot ?? "Center"); break;
                default:                       AssignSlot(charName, slot.Slot ?? NextAutoSlot()); break;
            }
        }

        foreach (var rect in _slotMap.Values) rect.Visible = false;

        foreach (var (slotName, charName) in _slotChars)
        {
            if (!_slotMap.TryGetValue(slotName, out var rect)) continue;
            var filePath = GetFilePath(charName, tachies);
            var tex = this.GetUtility<IGodotTextureRegistry>()!.Get(filePath) as Texture2D;
            if (tex == null) continue;

            var same = oldSlots.TryGetValue(slotName, out var oldChar) && oldChar == charName;
            if (same) { rect.Texture = tex; rect.Modulate = Colors.White; rect.Visible = true; }
            else await CrossfadeSlot(rect, tex);
        }
    }
}
