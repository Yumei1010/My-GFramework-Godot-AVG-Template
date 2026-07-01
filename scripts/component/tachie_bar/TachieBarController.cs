using GFramework.Core.extensions;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.component.tachie_slot;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.data.story;
using GFrameworkTemplate.scripts.enums.visualnovel;
using Godot;

namespace GFrameworkTemplate.scripts.component.tachie_bar;

/// <summary>
///     立绘栏控制器——响应 VN 立绘事件，管理角色 Sprite 的显示/切换/隐藏
///     使用对象池 + 槽位映射 + 交叉淡入淡出
/// </summary>
[Log]
[ContextAware]
public partial class TachieBarController : CanvasLayer
{
    /// <summary>槽位映射：角色名 → Sprite2D</summary>
    private readonly Dictionary<string, Sprite2D> _slots = new();

    /// <summary>空闲 Sprite 池</summary>
    private readonly List<Sprite2D> _pool = new();

    /// <summary>水平排列顺序缓存</summary>
    private readonly List<string> _order = new();

    private int _poolSize = 4;

    public override void _Ready()
    {
        // 预创建对象池
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
                case TachieOperation.Show:
                    ShowSlot(slotName, slot.FilePath);
                    break;
                case TachieOperation.Change:
                    ChangeSlot(slotName, slot.FilePath);
                    break;
                case TachieOperation.Close:
                    CloseSlot(slotName);
                    break;
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
        sprite.Position = Vector2.Zero;
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

        // 交叉淡入淡出
        var tween = CreateTween();
        tween.TweenProperty(newSprite, "modulate:a", 1.0f, 0.3f);
        tween.Parallel().TweenProperty(oldSprite, "modulate:a", 0.0f, 0.3f);
        tween.TweenCallback(Callable.From(() =>
        {
            oldSprite.Visible = false;
            _pool.Add(oldSprite);
        }));

        _slots[name] = newSprite;
    }

    private void CloseSlot(string name)
    {
        if (!_slots.Remove(name, out var sprite)) return;
        _order.Remove(name);
        sprite.Visible = false;
        _pool.Add(sprite);
    }

    /// <summary>按顺序水平均分屏幕宽度</summary>
    private void RepositionAll()
    {
        var viewport = GetViewport().GetVisibleRect().Size;
        var count = _order.Count;
        if (count == 0) return;

        var spacing = viewport.X / (count + 1);
        for (var i = 0; i < count; i++)
        {
            if (_slots.TryGetValue(_order[i], out var sprite))
            {
                var texSize = sprite.Texture?.GetSize() ?? Vector2.Zero;
                var target = new Vector2(spacing * (i + 1) - texSize.X / 2, viewport.Y - texSize.Y);
                sprite.Position = target;
            }
        }
    }

    private Texture2D? LoadTexture(string logicalName)
    {
        var path = StoryResourceMapper.ResolveTexturePath(logicalName);
        if (string.IsNullOrEmpty(path))
        {
            _log.Warn($"纹理未注册: {logicalName}");
            return null;
        }
        return GD.Load<Texture2D>(path);
    }

    private Sprite2D CreateNewSprite()
    {
        var sprite = new Sprite2D { Name = $"TachiePool_{_poolSize++}", Centered = false };
        AddChild(sprite);
        return sprite;
    }
}
