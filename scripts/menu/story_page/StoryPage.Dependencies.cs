using GFrameworkTemplate.global;
using GFrameworkTemplate.scripts.component.background_controller;
using GFrameworkTemplate.scripts.component.branch_controller;
using GFrameworkTemplate.scripts.component.sound_controller;
using GFrameworkTemplate.scripts.component.tachie_controller;
using GFrameworkTemplate.scripts.component.talk_controller;

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
