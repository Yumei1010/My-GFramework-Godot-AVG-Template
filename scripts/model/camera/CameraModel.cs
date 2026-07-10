using GFramework.Core.model;
using GFrameworkTemplate.scripts.component.camera;

namespace GFrameworkTemplate.scripts.model.camera;

/// <summary>
///     相机模型 —— 当前活跃的相机效果列表
/// </summary>
public class CameraModel : AbstractModel
{
    public List<CameraEffect> Effects { get; set; } = new();
    protected override void OnInit() { }
}
