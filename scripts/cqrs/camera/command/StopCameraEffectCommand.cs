using GFramework.Core.Abstractions.command;
using GFrameworkTemplate.scripts.system.camera;

namespace GFrameworkTemplate.scripts.cqrs.camera.command;

/// <summary>
///     停止指定类型的相机效果
/// </summary>
public sealed class StopCameraEffectCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<CameraSystem>().Clear();
    }
}
