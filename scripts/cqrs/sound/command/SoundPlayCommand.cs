using GFrameworkTemplate.scripts.system.sound_system;

namespace GFrameworkTemplate.scripts.cqrs.sound.command;

/// <summary>
///     SoundPlayCommand —— 播放 BGM 或音效
/// </summary>
public sealed class SoundPlayCommand : AbstractCommand
{
    public required string SoundType { get; set; }
    public required string FilePath { get; set; }

    protected override void OnExecute()
    {
        if (SoundType == "bgm")
            this.GetSystem<SoundSystem>()!.PlayBgm(FilePath);
        else
            this.GetSystem<SoundSystem>()!.PlaySfx(FilePath);
    }
}
