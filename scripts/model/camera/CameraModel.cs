using GFramework.Core.model;
using GFrameworkTemplate.scripts.component.camera;

namespace GFrameworkTemplate.scripts.model.camera;

public class CameraModel : AbstractModel
{
    public List<CameraEffect> Effects { get; set; } = new();
    protected override void OnInit() { }
}
