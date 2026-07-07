using GFramework.Core.command;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.sound.command;

/// <summary>
///     播放音频命令——通过 SoundSystem 播放 BGM/音效
/// </summary>
public sealed class ChangeSoundCommand : AbstractCommand
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
