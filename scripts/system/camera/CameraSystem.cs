using GFrameworkTemplate.scripts.component.camera;

namespace GFrameworkTemplate.scripts.system.camera;

/// <summary>
///     相机效果系统——纯 ISystem，管理效果列表、优先级排序、帧偏移计算
/// </summary>
[Log]
[ContextAware]
public sealed partial class CameraSystem : ISystem
{
    private readonly List<CameraEffect> _effects = new();

    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    public void Play(CameraEffect effect)
    {
        _effects.Add(effect);
        _log.Debug($"相机效果: {effect.GetType().Name} 优先级={effect.Priority}");
    }

    public void Stop<T>() where T : CameraEffect => _effects.RemoveAll(e => e is T);
    public void Clear() => _effects.Clear();

    public CameraFrameData GetFrameData(float delta)
    {
        for (var i = _effects.Count - 1; i >= 0; i--)
        {
            _effects[i].Elapsed += delta;
            if (_effects[i].IsExpired) _effects.RemoveAt(i);
        }

        if (_effects.Count == 0) return CameraFrameData.Identity;

        _effects.Sort((a, b) => b.Priority.CompareTo(a.Priority));

        var offset = Vector2.Zero;
        var zoom = 1f;
        var rotation = 0f;

        foreach (var fx in _effects)
        {
            var t = fx.Progress;
            offset += fx.GetOffset(t);
            zoom *= fx.GetZoom(t);
            rotation += fx.GetRotation(t);
        }

        return new CameraFrameData { Offset = offset, Zoom = zoom, Rotation = rotation };
    }
}

public struct CameraFrameData
{
    public Vector2 Offset;
    public float Zoom;
    public float Rotation;
    public static CameraFrameData Identity => new() { Zoom = 1f };
}
