using GFrameworkTemplate.scripts.component.camera;
using GFrameworkTemplate.scripts.cqrs.camera.command;
using GFrameworkTemplate.scripts.cqrs.camera.query;

namespace GFrameworkTemplate.scripts.system.visualnovel;

/// <summary>
///     相机效果系统——纯 ISystem，通过 SendCommand/SendQuery 操作 Model
/// </summary>
[Log]
[ContextAware]
public sealed partial class CameraSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    public void Play(CameraEffect effect) =>
        this.SendCommand(new AddCameraEffectCommand { Effect = effect });

    public void Clear() =>
        this.SendCommand(new ClearCameraEffectsCommand());

    public CameraFrameDataResult GetFrameData(float delta) =>
        this.SendQuery(new GetCameraFrameDataQuery { Delta = delta });
}
