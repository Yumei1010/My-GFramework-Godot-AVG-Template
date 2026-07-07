using GFrameworkTemplate.global;
using GFrameworkTemplate.scripts.entities.background_controller;
using GFrameworkTemplate.scripts.entities.branch_controller;
using GFrameworkTemplate.scripts.entities.sound_controller;
using GFrameworkTemplate.scripts.entities.tachie_controller;
using GFrameworkTemplate.scripts.entities.talk_controller;

namespace GFrameworkTemplate.scripts.menu.story_page;

public partial class StoryPage
{
    private Button InputDetector => GetNode<Button>("%InputDetector");
    private SoundController Sound => GetNode<SoundController>("%Sound");
    private BackgroundController Background => GetNode<BackgroundController>("%Background");
    private TachieController Tachie => GetNode<TachieController>("%Tachie");
    private TalkController Talk => GetNode<TalkController>("%Talk");
    private BranchController Branch => GetNode<BranchController>("%Branch");

    private async Task ReadyAsync()
    {
        await GameEntryPoint.Architecture.WaitUntilReadyAsync().ConfigureAwait(false);
    }
}
