using GFrameworkTemplate.scripts.system.sound_system;

namespace GFrameworkTemplate.scripts.cqrs.sound.command;

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
