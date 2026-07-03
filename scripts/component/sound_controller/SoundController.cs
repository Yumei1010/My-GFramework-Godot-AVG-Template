using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.system.sound;

namespace GFrameworkTemplate.scripts.component.sound_controller;

/// <summary>
///     音频控制器——CanvasLayer 场景节点，AudioStreamPlayer 播放
/// </summary>
[Log]
[ContextAware]
public partial class SoundController : CanvasLayer
{
    private AudioStreamPlayer BgmPlayer => GetNode<AudioStreamPlayer>("%BgmPlayer");
    private AudioStreamPlayer BgmHelper => GetNode<AudioStreamPlayer>("%BgmHelper");
    private readonly AudioStreamPlayer[] _sfxPool = new AudioStreamPlayer[8];
    private Tween? _bgmTween;

    public override void _Ready()
    {
        for (var i = 0; i < 8; i++)
            _sfxPool[i] = GetNode<AudioStreamPlayer>($"%SfxPlayer_{i}");

        BgmHelper.VolumeDb = -80f;

        var system = this.GetSystem<SoundSystem>()!;
        system.BgmRequested += OnBgmRequested;
        system.SfxRequested += OnSfxRequested;

        this.RegisterEvent<VisualNovelSoundTriggeredEvent>(e =>
        {
            if (e.SoundType == "bgm") system.PlayBgm(e.FilePath);
            else system.PlaySfx(e.FilePath);
        }).UnRegisterWhenNodeExitTree(this);
    }

    private async void OnBgmRequested(string logicalName)
    {
        var path = ResolveAudioPath(logicalName);
        if (string.IsNullOrEmpty(path)) return;
        var stream = GD.Load<AudioStream>(path);
        if (stream == null) return;

        _bgmTween?.Kill();
        BgmHelper.Stream = stream;
        BgmHelper.Play();
        BgmHelper.VolumeDb = -80f;

        _bgmTween = CreateTween();
        _bgmTween.TweenProperty(BgmHelper, "volume_db", 0f, 1f);
        _bgmTween.Parallel().TweenProperty(BgmPlayer, "volume_db", -80f, 1f);
        await ToSignal(_bgmTween, Tween.SignalName.Finished);

        (BgmPlayer.Stream, BgmHelper.Stream) = (BgmHelper.Stream, BgmPlayer.Stream);
        BgmPlayer.VolumeDb = 0f;
        BgmHelper.Stop();
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
