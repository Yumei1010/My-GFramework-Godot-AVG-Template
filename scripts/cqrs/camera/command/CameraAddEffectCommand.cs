using GFrameworkTemplate.scripts.component.camera;
using GFrameworkTemplate.scripts.model.camera;

namespace GFrameworkTemplate.scripts.cqrs.camera.command;

/// <summary>
///     添加相机效果
/// </summary>
public sealed class CameraAddEffectCommand : AbstractCommand
{
    public required CameraEffect Effect { get; set; }

    protected override void OnExecute()
    {
        this.GetModel<CameraModel>()!.Effects.Add(Effect);
    }
}
