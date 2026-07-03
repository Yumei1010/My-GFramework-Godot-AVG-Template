using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.data.story;
using GFrameworkTemplate.scripts.cqrs.tachie.query;
using GFrameworkTemplate.scripts.system.tachie;

namespace GFrameworkTemplate.scripts.component.tachie_controller;

/// <summary>
///     立绘控制器——CanvasLayer 场景节点，渲染立绘 TextureRect
/// </summary>
[Log]
[ContextAware]
public partial class TachieController : CanvasLayer
{
    private TextureRect LeftSlot => GetNode<TextureRect>("%LeftSlot");
    private TextureRect CenterSlot => GetNode<TextureRect>("%CenterSlot");
    private TextureRect RightSlot => GetNode<TextureRect>("%RightSlot");
    private TextureRect HelperSlot => GetNode<TextureRect>("%HelperSlot");
    private TachieSystem _system = null!;

    public override void _Ready()
    {
        _system = this.GetSystem<TachieSystem>()!;
        _system.Changed += Render;
        this.RegisterEvent<VisualNovelTachieTriggeredEvent>(e =>
        {
            foreach (var (name, slot) in e.Tachies)
                _system.Handle(slot.Type, name, slot.FilePath);
        }).UnRegisterWhenNodeExitTree(this);
    }

    private async void Render()
    {
        var model = this.SendQuery(new GetTachieStateQuery())!;
        var oldSlots = new Dictionary<string, string?>(model.SlotToChar);

        foreach (var r in new[] { LeftSlot, CenterSlot, RightSlot })
            r.Visible = false;

        foreach (var (slotName, charName) in model.SlotToChar)
        {
            if (!model.Chars.TryGetValue(charName, out var path)) continue;
            var tex = LoadTexture(path);
            if (tex == null) continue;

            var rect = GetSlot(slotName);
            var wasVisible = oldSlots.ContainsKey(slotName);

            if (wasVisible)
            {
                await CrossfadeSlot(rect, tex);
            }
            else
            {
                rect.Texture = tex;
                rect.Modulate = Colors.White;
                rect.Visible = true;
            }
        }
    }

    private async Task CrossfadeSlot(TextureRect rect, Texture2D newTex)
    {
        HelperSlot.Texture = newTex;
        HelperSlot.Position = rect.Position;
        HelperSlot.Size = rect.Size;
        HelperSlot.Modulate = Colors.Transparent;
        HelperSlot.Visible = true;

        var tween = CreateTween();
        tween.TweenProperty(HelperSlot, "modulate", Colors.White, 0.3f);
        tween.Parallel().TweenProperty(rect, "modulate", Colors.Transparent, 0.3f);
        await ToSignal(tween, Tween.SignalName.Finished);

        rect.Texture = newTex;
        rect.Modulate = Colors.White;
        HelperSlot.Visible = false;
    }

    private TextureRect GetSlot(string name) => name switch
    {
        "Left" => LeftSlot,
        "Center" => CenterSlot,
        "Right" => RightSlot,
        _ => LeftSlot
    };

    private static Texture2D? LoadTexture(string logicalName)
    {
        var p = StoryResourceMapper.ResolveTexturePath(logicalName);
        return string.IsNullOrEmpty(p) ? null : GD.Load<Texture2D>(p);
    }
}
