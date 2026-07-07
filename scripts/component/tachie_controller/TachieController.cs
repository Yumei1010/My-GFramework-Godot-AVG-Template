using GFrameworkTemplate.scripts.cqrs.tachie.command;
using GFrameworkTemplate.scripts.cqrs.tachie.query;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.data.story;
using GFrameworkTemplate.scripts.enums.visualnovel;

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

    public override void _Ready()
    {
        this.RegisterEvent<VisualNovelTachieUpdatedEvent>(e =>
        {
            var oldSlots = this.SendQuery(new GetTachieStateQuery())!.SlotToChar;
            foreach (var (name, slot) in e.Tachies)
                this.SendCommand(new UpdateTachieCommand { Type = slot.Type, CharName = name, FilePath = slot.FilePath, Slot = slot.Slot });
            Render(oldSlots);
        }).UnRegisterWhenNodeExitTree(this);
    }

    private async void Render(IReadOnlyDictionary<string, string> oldSlotMap)
    {
        var model = this.SendQuery(new GetTachieStateQuery())!;

        foreach (var r in new[] { LeftSlot, CenterSlot, RightSlot })
            r.Visible = false;

        foreach (var (slotName, charName) in model.SlotToChar)
        {
            if (!model.Chars.TryGetValue(charName, out var path)) continue;
            var tex = LoadTexture(path);
            if (tex == null) continue;

            var rect = GetSlot(slotName);

            if (oldSlotMap.TryGetValue(slotName, out var oldChar) && oldChar == charName)
            {
                // 同一角色已在同一槽位显示过，直接恢复
                rect.Texture = tex;
                rect.Modulate = Colors.White;
                rect.Visible = true;
            }
            else if (oldSlotMap.ContainsKey(slotName))
            {
                // 同一槽位切换不同角色，交叉淡入淡出
                await CrossfadeSlot(rect, tex);
            }
            else
            {
                // 新槽位，直接显示
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
        rect.Visible = true;
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
