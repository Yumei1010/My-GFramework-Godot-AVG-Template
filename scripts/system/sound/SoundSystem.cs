using GFrameworkTemplate.scripts.cqrs.sound.command;

namespace GFrameworkTemplate.scripts.system.sound;

/// <summary>
///     音频系统——纯 ISystem，通过 SendCommand 操作 Model
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

    public void PlayBgm(string logicalName)
    {
        this.SendCommand(new PlayBgmCommand { LogicalName = logicalName });
        BgmRequested?.Invoke(logicalName);
    }

    public void PlaySfx(string logicalName) => SfxRequested?.Invoke(logicalName);
}
