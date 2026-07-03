using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.data.story;

namespace GFrameworkTemplate.scripts.system.sound;

/// <summary>
///     音频管理器全局单例——双轨 BGM 交叉淡入淡出 + SFX 对象池
/// </summary>
[Log]
[ContextAware]
public partial class SoundSystem : CanvasLayer, ISystem
    public static SoundSystem? Instance { get; private set; }
{
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    private AudioStreamPlayer BgmPlayer => GetNode<AudioStreamPlayer>("%BgmPlayer");
    private AudioStreamPlayer BgmHelper => GetNode<AudioStreamPlayer>("%BgmHelper");
    private Tween? _bgmTween;
    private string _currentBgmPath = string.Empty;

    private readonly AudioStreamPlayer[] _sfxPool = new AudioStreamPlayer[8];

    public override void _Ready()
        Instance = this;
    {
        for (var i = 0; i < 8; i++)
            _sfxPool[i] = GetNode<AudioStreamPlayer>($"%SfxPlayer_{i}");

        BgmHelper.VolumeDb = -80f;
        this.RegisterEvent<VisualNovelSoundTriggeredEvent>(OnSound).UnRegisterWhenNodeExitTree(this);
        _log.Debug("SoundSystem 就绪");
    }

    private void OnSound(VisualNovelSoundTriggeredEvent e)
    {
        if (e.SoundType == "bgm")
            PlayBgm(e.FilePath);
        else
            PlaySfx(e.FilePath);
    }

    private async void PlayBgm(string logicalName)
    {
        var path = StoryResourceMapper.ResolveTexturePath(logicalName)
                   ?? ResolveAudioPath(logicalName);
        if (string.IsNullOrEmpty(path)) return;

        var stream = GD.Load<AudioStream>(path);
        if (stream == null || _currentBgmPath == path) return;
        _currentBgmPath = path;

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

    private void PlaySfx(string logicalName)
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
        var candidates = new[]
        {
            $"res://assets/sound/bgm/{logicalName}.ogg",
            $"res://assets/sound/sfx/{logicalName}.ogg",
            $"res://assets/sound/bgm/{logicalName}.mp3",
            $"res://assets/sound/sfx/{logicalName}.mp3",
            $"res://assets/sound/ambience/{logicalName}.ogg",
            $"res://assets/sound/voice/{logicalName}.ogg",
        };
        return candidates.FirstOrDefault(FileAccess.FileExists);
    }
}
