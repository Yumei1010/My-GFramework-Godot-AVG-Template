using GFrameworkTemplate.scripts.model.sound;

namespace GFrameworkTemplate.scripts.system.sound;

/// <summary>
///     音频系统——纯 ISystem，通过 SoundModel 管理 BGM
/// </summary>
[Log]
[ContextAware]
public sealed partial class SoundSystem : ISystem
{
    public event Action<string>? BgmRequested;
    public event Action<string>? SfxRequested;

    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    private SoundModel Model => this.GetModel<SoundModel>()!;

    public void PlayBgm(string logicalName)
    {
        if (Model.CurrentBgm == logicalName) return;
        Model.CurrentBgm = logicalName;
        BgmRequested?.Invoke(logicalName);
        _log.Debug($"BGM: {logicalName}");
    }

    public void PlaySfx(string logicalName) => SfxRequested?.Invoke(logicalName);
}
