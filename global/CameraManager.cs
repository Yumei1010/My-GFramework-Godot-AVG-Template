using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.component.camera;
using Godot;

namespace GFrameworkTemplate.global;

/// <summary>
///     相机管理器全局单例——优先级叠加式镜头效果系统
///     用法: CameraManager.Instance.Play(new EarthquakeEffect { Duration = 1.5f, Intensity = 25f });
/// </summary>
[Log]
[ContextAware]
public partial class CameraManager : CanvasLayer
{
    private Camera2D? _camera;
    private Vector2 _basePosition;
    private float _baseZoom = 1f;
    private float _baseRotation;
    private readonly List<CameraEffect> _effects = new();

    public static CameraManager? Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        _camera = GetNode<Camera2D>("Camera2D");
        _camera.MakeCurrent();
        _basePosition = _camera.Position;
        _baseZoom = _camera.Zoom.X;
        _baseRotation = _camera.Rotation;
        _log.Debug("CameraManager 就绪");
    }

    public override void _Process(double delta)
    {
        if (_camera == null) return;

        var dt = (float)delta;

        // 更新 + 清理过期效果
        for (var i = _effects.Count - 1; i >= 0; i--)
        {
            _effects[i].Elapsed += dt;
            if (_effects[i].IsExpired)
                _effects.RemoveAt(i);
        }

        // 按优先级降序
        _effects.Sort((a, b) => b.Priority.CompareTo(a.Priority));

        // 累积计算偏移（同类型只取最高优先级）
        var offset = Vector2.Zero;
        var zoom = _baseZoom;
        var rotation = _baseRotation;

        foreach (var fx in _effects)
        {
            var t = fx.Progress;
            offset += fx.GetOffset(t);
            zoom *= fx.GetZoom(t) - 1f + 1f; // 乘积叠加
            rotation += fx.GetRotation(t);
        }

        _camera.Position = _basePosition + offset;
        _camera.Zoom = new Vector2(zoom, zoom);
        _camera.Rotation = rotation;
    }

    /// <summary>添加并播放效果</summary>
    public void Play(CameraEffect effect)
    {
        _effects.Add(effect);
        _log.Debug($"相机效果: {effect.GetType().Name} 优先级={effect.Priority} 时长={effect.Duration}s");
    }

    /// <summary>按类型移除效果</summary>
    public void Stop<T>() where T : CameraEffect
    {
        _effects.RemoveAll(e => e is T);
    }

    /// <summary>清除所有效果</summary>
    public void Clear()
    {
        _effects.Clear();
        if (_camera != null)
        {
            _camera.Position = _basePosition;
            _camera.Zoom = new Vector2(_baseZoom, _baseZoom);
            _camera.Rotation = _baseRotation;
        }
    }
}
