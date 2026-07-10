using GFrameworkTemplate.scripts.cqrs.sound.@event;

namespace GFrameworkTemplate.scripts.entities.sound_view;

public partial class SoundView
{
    private void RegisterEvent()
    {
        this.RegisterEvent<SoundPlayedEvent>(e =>
        {
             OnSoundPlayedEvent(e.SoundType, e.FilePath);
        }).UnRegisterWhenNodeExitTree(this);
    }

    private void OnSoundPlayedEvent(string soundType, string filePath)
    {
        if (soundType == "bgm")
            PlayBgm(filePath);
        else
            PlaySfx(filePath);
    }
}
