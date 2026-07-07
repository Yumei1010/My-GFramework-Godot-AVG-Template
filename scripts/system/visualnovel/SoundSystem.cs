using GFrameworkTemplate.scripts.cqrs.sound.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.model.sound;

namespace GFrameworkTemplate.scripts.system.visualnovel;

/// <summary>
///     音频系统——独立 ISystem，通过 ChangeSoundCommand 驱动
/// </summary>
[Log]
[ContextAware]
public sealed partial class SoundSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    /// <summary>
    ///     播放 BGM（防重复播放）
    /// </summary>
    public void PlayBgm(string filePath)
    {
        var model = this.GetModel<SoundModel>()!;
        if (model.CurrentBgm == filePath) return;

        model.CurrentBgm = filePath;
        this.SendEvent(new VisualNovelSoundPlayedEvent { SoundType = "bgm", FilePath = filePath });
    }

    /// <summary>
    ///     播放音效
    /// </summary>
    public void PlaySfx(string filePath)
    {
        this.SendEvent(new VisualNovelSoundPlayedEvent { SoundType = "oneSound", FilePath = filePath });
    }
}
