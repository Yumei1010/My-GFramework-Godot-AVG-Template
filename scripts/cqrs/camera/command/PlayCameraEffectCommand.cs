using GFramework.Core.Abstractions.command;
using GFrameworkTemplate.scripts.component.camera;
using GFrameworkTemplate.scripts.system.camera;

namespace GFrameworkTemplate.scripts.cqrs.camera.command;

/// <summary>
///     播放指定的相机效果
/// </summary>
public sealed class PlayCameraEffectCommand : AbstractCommand
{
    public required CameraEffect Effect { get; set; }

    protected override void OnExecute()
    {
        this.GetSystem<CameraSystem>().Play(Effect);
    }
}
