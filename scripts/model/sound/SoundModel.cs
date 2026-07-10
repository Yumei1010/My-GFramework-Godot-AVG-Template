using GFramework.Core.model;

namespace GFrameworkTemplate.scripts.model.sound;

/// <summary>
///     音频模型 —— 当前播放的 BGM 防重复
/// </summary>
public class SoundModel : AbstractModel
{
    public string CurrentBgm { get; set; } = string.Empty;
    protected override void OnInit() { }
}
