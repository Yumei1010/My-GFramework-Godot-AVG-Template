using GFramework.Core.extensions;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.data.story;
using Godot;

namespace GFrameworkTemplate.global;

/// <summary>
///     音频管理器全局单例——双轨 BGM 交叉淡入淡出 + SFX 对象池
/// </summary>
[Log]
[ContextAware]
public partial class SoundManager : CanvasLayer
{
    private AudioStreamPlayer _bgmMain = null!;
    private AudioStreamPlayer _bgmHelper = null!;
    private Tween? _bgmTween;
    private string _currentBgmPath = string.Empty;

    private readonly List<AudioStreamPlayer> _sfxPool = new();
    private const int SfxPoolSize = 8;

    public override void _Ready()
    {
        Name = "SoundManager";

        _bgmMain = CreateBgmPlayer("BgmMain");
        _bgmHelper = CreateBgmPlayer("BgmHelper");
        _bgmHelper.VolumeDb = -80f;
        AddChild(_bgmMain);
        AddChild(_bgmHelper);

        for (var i = 0; i < SfxPoolSize; i++)
        {
            var sfx = new AudioStreamPlayer { Name = $"Sfx_{i}", Bus = "SFX" };
            AddChild(sfx);
            _sfxPool.Add(sfx);
        }

        this.RegisterEvent<VisualNovelSoundTriggeredEvent>(OnSound).UnRegisterWhenNodeExitTree(this);
        _log.Debug("SoundManager 就绪");
    }

    private void OnSound(VisualNovelSoundTriggeredEvent e)
    {
        if (e.SoundType == "bgm")
        {
            PlayBgm(e.FilePath);
        }
        else
        {
            PlaySfx(e.FilePath);
        }
    }

    private async void PlayBgm(string logicalName)
    {
        var path = StoryResourceMapper.ResolveTexturePath(logicalName);
        if (string.IsNullOrEmpty(path))
        {
            path = ResolveAudioPath(logicalName);
            if (string.IsNullOrEmpty(path)) return;
        }

        var stream = GD.Load<AudioStream>(path);
        if (stream == null) return;

        if (_currentBgmPath == path) return;
        _currentBgmPath = path;

        _bgmTween?.Kill();

        // 交叉淡入淡出
        _bgmHelper.Stream = stream;
        _bgmHelper.Play();
        _bgmHelper.VolumeDb = -80f;

        _bgmTween = CreateTween();
        _bgmTween.TweenProperty(_bgmHelper, "volume_db", 0f, 1f);
        _bgmTween.Parallel().TweenProperty(_bgmMain, "volume_db", -80f, 1f);

        await ToSignal(_bgmTween, Tween.SignalName.Finished);

        (_bgmMain, _bgmHelper) = (_bgmHelper, _bgmMain);
        _bgmHelper.Stop();
    }

    private void PlaySfx(string logicalName)
    {
        var path = ResolveAudioPath(logicalName);
        if (string.IsNullOrEmpty(path)) return;

        var stream = GD.Load<AudioStream>(path);
        if (stream == null) return;

        // 找一个空闲的 SFX 播放器
        var player = _sfxPool.Find(p => !p.Playing);
        if (player == null)
        {
            player = new AudioStreamPlayer { Name = $"Sfx_{_sfxPool.Count}", Bus = "SFX" };
            AddChild(player);
            _sfxPool.Add(player);
        }

        player.Stream = stream;
        player.Play();
    }

    private static string? ResolveAudioPath(string logicalName)
    {
        // 尝试常见音频路径
        var candidates = new[]
        {
            $"res://assets/sound/bgm/{logicalName}.ogg",
            $"res://assets/sound/sfx/{logicalName}.ogg",
            $"res://assets/sound/bgm/{logicalName}.mp3",
            $"res://assets/sound/sfx/{logicalName}.mp3",
            $"res://assets/sound/ambience/{logicalName}.ogg",
            $"res://assets/sound/voice/{logicalName}.ogg",
        };

        foreach (var c in candidates)
        {
            if (FileAccess.FileExists(c))
                return c;
        }

        return null;
    }

    private static AudioStreamPlayer CreateBgmPlayer(string name) => new()
    {
        Name = name,
        Bus = "Music",
        VolumeDb = 0f
    };
}
