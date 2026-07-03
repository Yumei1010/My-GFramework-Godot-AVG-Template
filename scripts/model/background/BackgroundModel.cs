using GFramework.Core.model;

namespace GFrameworkTemplate.scripts.model.background;

public class BackgroundModel : AbstractModel
{
    public string CurrentPath { get; set; } = string.Empty;
    protected override void OnInit() { }
}
