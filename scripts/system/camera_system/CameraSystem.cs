using GFrameworkTemplate.scripts.component.camera;
using GFrameworkTemplate.scripts.cqrs.camera.command;
using GFrameworkTemplate.scripts.cqrs.camera.query;
using GFrameworkTemplate.scripts.model.camera;

namespace GFrameworkTemplate.scripts.system.camera_system;

/// <summary>
///     相机效果系统——管理效果的增删与逐帧更新
/// </summary>
[Log]
[ContextAware]
public sealed partial class CameraSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase)
    {
        _log.Debug("System initialized: CameraSystem");
    }

    public void Init()
    {
        
    }

    public void Destroy()
    {
        _log.Debug("System destroyed: CameraSystem");
    }

    public void Play(CameraEffect effect) =>
        this.SendCommand(new CameraAddEffectCommand { Effect = effect });

    public void Clear() =>
        this.SendCommand(new CameraClearEffectsCommand());

    /// <summary>逐帧更新效果并返回复合变换数据</summary>
    public CameraFrameDataResult Update(float delta)
    {
        var model = this.GetModel<CameraModel>()!;

        for (var i = model.Effects.Count - 1; i >= 0; i--)
        {
            model.Effects[i].Elapsed += delta;
            if (model.Effects[i].IsExpired) model.Effects.RemoveAt(i);
        }

        if (model.Effects.Count == 0) return CameraFrameDataResult.Identity;

        model.Effects.Sort((a, b) => b.Priority.CompareTo(a.Priority));

        var offset = Vector2.Zero;
        var zoom = 1f;
        var rotation = 0f;
        foreach (var fx in model.Effects)
        {
            var t = fx.Progress;
            offset += fx.GetOffset(t);
            zoom *= fx.GetZoom(t);
            rotation += fx.GetRotation(t);
        }

        return new CameraFrameDataResult { Offset = offset, Zoom = zoom, Rotation = rotation };
    }
}
