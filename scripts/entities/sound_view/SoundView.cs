using GFrameworkTemplate.scripts.utility;

namespace GFrameworkTemplate.scripts.entities.sound_view;

[Log]
[ContextAware]
/// <summary>
///     音频 View —— BGM 与 SFX 对象池播放
/// </summary>
public partial class SoundView : CanvasLayer
{
    public override void _Ready()
    {
        _ = ReadyAsync();
        RegisterEvent();
    }

    private void PlayBgm(string logicalName)
    {
        var stream = this.GetUtility<IGodotAudioRegistry>()!.Get(logicalName);
        if (stream == null) return;
        BgmPlayer.Stream = stream;
        BgmPlayer.Play();
    }

    private void PlaySfx(string logicalName)
    {
        var stream = this.GetUtility<IGodotAudioRegistry>()!.Get(logicalName);
        if (stream == null) return;
        var player = _sfxPool.FirstOrDefault(p => !p.Playing);
        if (player == null) return;
        player.Stream = stream;
        player.Play();
    }
}
