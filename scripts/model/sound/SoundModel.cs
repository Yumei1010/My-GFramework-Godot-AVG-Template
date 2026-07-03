using GFramework.Core.model;

namespace GFrameworkTemplate.scripts.model.sound;

public class SoundModel : AbstractModel
{
    public string CurrentBgm { get; set; } = string.Empty;
    protected override void OnInit() { }
}
