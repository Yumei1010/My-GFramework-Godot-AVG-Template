using GFrameworkTemplate.scripts.cqrs.sound.command;

namespace GFrameworkTemplate.scripts.system.visualnovel;

/// <summary>
///     音频系统——BGM/SFX 管理
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

    public void PlayBgm(string name)
    {
        this.SendCommand(new PlayBgmCommand { LogicalName = name });
        BgmRequested?.Invoke(name);
    }

    public void PlaySfx(string name) => SfxRequested?.Invoke(name);
}
