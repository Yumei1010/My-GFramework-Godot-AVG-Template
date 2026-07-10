using GFrameworkTemplate.global;

namespace GFrameworkTemplate.scripts.entities.sound_view;

public partial class SoundView
{
    private AudioStreamPlayer BgmPlayer => GetNode<AudioStreamPlayer>("%BgmPlayer");
    private readonly AudioStreamPlayer[] _sfxPool = new AudioStreamPlayer[8];

    private async Task ReadyAsync()
    {
        await GameEntryPoint.Architecture.WaitUntilReadyAsync().ConfigureAwait(false);

        for (var i = 0; i < 8; i++)
            _sfxPool[i] = GetNode<AudioStreamPlayer>($"%SfxPlayer_{i}");
    }
}
