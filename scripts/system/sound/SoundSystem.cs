namespace GFrameworkTemplate.scripts.system.sound;

/// <summary>
///     音频系统——纯 ISystem，管理 BGM 和 SFX 播放请求
/// </summary>
[Log]
[ContextAware]
public sealed partial class SoundSystem : ISystem
{
    public string CurrentBgm { get; private set; } = string.Empty;

    public event Action<string>? BgmRequested;
    public event Action<string>? SfxRequested;

    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    public void PlayBgm(string logicalName)
    {
        if (CurrentBgm == logicalName) return;
        CurrentBgm = logicalName;
        BgmRequested?.Invoke(logicalName);
        _log.Debug($"BGM: {logicalName}");
    }

    public void PlaySfx(string logicalName) => SfxRequested?.Invoke(logicalName);
}
