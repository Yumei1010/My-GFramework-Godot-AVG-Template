using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.component.sound_controller;

/// <summary>
///     音频控制器——CanvasLayer 场景节点，BGM 直接播放，SFX 对象池
/// </summary>
[Log]
[ContextAware]
public partial class SoundController : CanvasLayer
{
    private AudioStreamPlayer BgmPlayer => GetNode<AudioStreamPlayer>("%BgmPlayer");
    private readonly AudioStreamPlayer[] _sfxPool = new AudioStreamPlayer[8];

    public override void _Ready()
    {
        for (var i = 0; i < 8; i++)
            _sfxPool[i] = GetNode<AudioStreamPlayer>($"%SfxPlayer_{i}");

        this.RegisterEvent<VisualNovelSoundPlayedEvent>(e =>
        {
            if (e.SoundType == "bgm") OnBgmRequested(e.FilePath);
            else OnSfxRequested(e.FilePath);
        }).UnRegisterWhenNodeExitTree(this);
    }

    private void OnBgmRequested(string logicalName)
    {
        var path = ResolveAudioPath(logicalName);
        if (string.IsNullOrEmpty(path)) return;
        var stream = GD.Load<AudioStream>(path);
        if (stream == null) return;

        BgmPlayer.Stream = stream;
        BgmPlayer.Play();
    }

    private void OnSfxRequested(string logicalName)
    {
        var path = ResolveAudioPath(logicalName);
        if (string.IsNullOrEmpty(path)) return;
        var stream = GD.Load<AudioStream>(path);
        if (stream == null) return;

        var player = _sfxPool.FirstOrDefault(p => !p.Playing);
        if (player == null) return;
        player.Stream = stream;
        player.Play();
    }

    private static string? ResolveAudioPath(string logicalName)
    {
        foreach (var ext in new[] { ".ogg", ".mp3" })
        foreach (var dir in new[] { "bgm", "sfx", "ambience", "voice" })
        {
            var p = $"res://assets/sound/{dir}/{logicalName}{ext}";
            if (FileAccess.FileExists(p)) return p;
        }
        return null;
    }
}
