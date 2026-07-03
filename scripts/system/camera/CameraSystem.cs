using GFrameworkTemplate.scripts.component.camera;
using GFrameworkTemplate.scripts.model.camera;

namespace GFrameworkTemplate.scripts.system.camera;

/// <summary>
///     相机效果系统——纯 ISystem，通过 Model 管理效果
/// </summary>
[Log]
[ContextAware]
public sealed partial class CameraSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    private CameraModel Model => this.GetModel<CameraModel>()!;

    public void Play(CameraEffect effect)
    {
        Model.Effects.Add(effect);
        _log.Debug($"相机效果: {effect.GetType().Name}");
    }

    public void Stop<T>() where T : CameraEffect => Model.Effects.RemoveAll(e => e is T);
    public void Clear() => Model.Effects.Clear();

    public CameraFrameData GetFrameData(float delta)
    {
        var effects = Model.Effects;
        for (var i = effects.Count - 1; i >= 0; i--)
        {
            effects[i].Elapsed += delta;
            if (effects[i].IsExpired) effects.RemoveAt(i);
        }

        if (effects.Count == 0) return CameraFrameData.Identity;

        effects.Sort((a, b) => b.Priority.CompareTo(a.Priority));

        var offset = Vector2.Zero; var zoom = 1f; var rotation = 0f;
        foreach (var fx in effects)
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
    public Vector2 Offset; public float Zoom; public float Rotation;
    public static CameraFrameData Identity => new() { Zoom = 1f };
}
