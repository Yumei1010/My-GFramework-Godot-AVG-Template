using GFrameworkTemplate.scripts.cqrs.sound.command;
using GFrameworkTemplate.scripts.cqrs.sound.@event;
using GFrameworkTemplate.scripts.model.sound;

namespace GFrameworkTemplate.scripts.system.sound_system;

/// <summary>
///     音频系统——独立 ISystem，通过 ChangeSoundCommand 驱动
/// </summary>
[Log]
[ContextAware]
public sealed partial class SoundSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase)
    {
        _log.Debug("System initialized: SoundSystem");
    }

    public void Init()
    {
        
    }

    public void Destroy()
    {
        _log.Debug("System destroyed: SoundSystem");
    }

    /// <summary>
    ///     播放 BGM（防重复播放）
    /// </summary>
    public void PlayBgm(string filePath)
    {
        var model = this.GetModel<SoundModel>()!;
        if (model.CurrentBgm == filePath) return;

        model.CurrentBgm = filePath;
        this.SendEvent(new SoundPlayedEvent { SoundType = "bgm", FilePath = filePath });
    }

    /// <summary>
    ///     播放音效
    /// </summary>
    public void PlaySfx(string filePath)
    {
        this.SendEvent(new SoundPlayedEvent { SoundType = "oneSound", FilePath = filePath });
    }
}
