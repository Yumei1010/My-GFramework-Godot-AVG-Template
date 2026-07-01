using GFramework.Core.extensions;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.component.tachie_slot;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.data.story;
using GFrameworkTemplate.scripts.enums.visualnovel;
using Godot;

namespace GFrameworkTemplate.global;

/// <summary>
///     立绘栏全局单例——自动加载，管理角色 Sprite 的显示/切换/隐藏
/// </summary>
[Log]
[ContextAware]
public partial class TachieBarController : CanvasLayer
{
    private readonly Dictionary<string, Sprite2D> _slots = new();
    private readonly List<Sprite2D> _pool = new();
    private readonly List<string> _order = new();
    private int _poolSize = 4;

    public override void _Ready()
    {
        for (var i = 0; i < _poolSize; i++)
        {
            var sprite = new Sprite2D { Name = $"TachiePool_{i}", Centered = false };
            AddChild(sprite);
            _pool.Add(sprite);
        }

        this.RegisterEvent<VisualNovelTachieTriggeredEvent>(OnTachie).UnRegisterWhenNodeExitTree(this);
    }

    private void OnTachie(VisualNovelTachieTriggeredEvent e)
    {
        foreach (var (slotName, slot) in e.Tachies)
        {
            switch (slot.Type)
            {
                case TachieOperation.Show:  ShowSlot(slotName, slot.FilePath); break;
                case TachieOperation.Change: ChangeSlot(slotName, slot.FilePath); break;
                case TachieOperation.Close:  CloseSlot(slotName); break;
            }
        }
        RepositionAll();
    }

    private void ShowSlot(string name, string path)
    {
        if (_slots.ContainsKey(name)) return;
        var sprite = _pool.Count > 0 ? _pool[^1] : CreateNewSprite();
        if (_pool.Count > 0) _pool.RemoveAt(_pool.Count - 1);
        sprite.Texture = LoadTexture(path);
        sprite.Visible = true;
        sprite.Modulate = Colors.White;
        _slots[name] = sprite;
        _order.Add(name);
    }

    private void ChangeSlot(string name, string path)
    {
        if (!_slots.TryGetValue(name, out var oldSprite)) return;
        var newSprite = _pool.Count > 0 ? _pool[^1] : CreateNewSprite();
        if (_pool.Count > 0) _pool.RemoveAt(_pool.Count - 1);
        newSprite.Texture = LoadTexture(path);
        newSprite.Position = oldSprite.Position;
        newSprite.Visible = true;
        newSprite.Modulate = Colors.White;

        var tween = CreateTween();
        tween.TweenProperty(newSprite, "modulate:a", 1.0f, 0.3f);
        tween.Parallel().TweenProperty(oldSprite, "modulate:a", 0.0f, 0.3f);
        tween.TweenCallback(Callable.From(() => { oldSprite.Visible = false; _pool.Add(oldSprite); }));
        _slots[name] = newSprite;
    }

    private void CloseSlot(string name)
    {
        if (!_slots.Remove(name, out var sprite)) return;
        _order.Remove(name);
        sprite.Visible = false;
        _pool.Add(sprite);
    }

    private void RepositionAll()
    {
        var vp = GetViewport().GetVisibleRect().Size;
        var count = _order.Count;
        if (count == 0) return;
        var spacing = vp.X / (count + 1);
        for (var i = 0; i < count; i++)
        {
            if (_slots.TryGetValue(_order[i], out var sprite))
            {
                var texSize = sprite.Texture?.GetSize() ?? Vector2.Zero;
                sprite.Position = new Vector2(spacing * (i + 1) - texSize.X / 2, vp.Y - texSize.Y);
            }
        }
    }

    private Texture2D? LoadTexture(string logicalName)
    {
        var path = StoryResourceMapper.ResolveTexturePath(logicalName);
        if (string.IsNullOrEmpty(path)) { _log.Warn($"纹理未注册: {logicalName}"); return null; }
        return GD.Load<Texture2D>(path);
    }

    private Sprite2D CreateNewSprite()
    {
        var s = new Sprite2D { Name = $"TachiePool_{_poolSize++}", Centered = false };
        AddChild(s);
        return s;
    }
}
