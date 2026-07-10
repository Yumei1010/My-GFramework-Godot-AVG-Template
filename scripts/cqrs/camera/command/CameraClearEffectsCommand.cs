using GFrameworkTemplate.scripts.model.camera;

namespace GFrameworkTemplate.scripts.cqrs.camera.command;

/// <summary>
///     清除所有相机效果
/// </summary>
/// <summary>
///     CameraClearEffectsCommand —— 清除所有相机效果
/// </summary>
public sealed class CameraClearEffectsCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<CameraModel>()!.Effects.Clear();
    }
}
