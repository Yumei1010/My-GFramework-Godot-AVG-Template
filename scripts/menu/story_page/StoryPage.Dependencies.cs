using GFrameworkTemplate.global;
using GFrameworkTemplate.scripts.entities.background_view;
using GFrameworkTemplate.scripts.entities.branch_view;
using GFrameworkTemplate.scripts.entities.sound_view;
using GFrameworkTemplate.scripts.entities.tachie_view;
using GFrameworkTemplate.scripts.entities.talk_view;

namespace GFrameworkTemplate.scripts.menu.story_page;

public partial class StoryPage
{
    private Button InputDetector => GetNode<Button>("%InputDetector");
    private SoundView Sound => GetNode<SoundView>("%Sound");
    private BackgroundView Background => GetNode<BackgroundView>("%Background");
    private TachieView Tachie => GetNode<TachieView>("%Tachie");
    private TalkView Talk => GetNode<TalkView>("%Talk");
    private BranchView Branch => GetNode<BranchView>("%Branch");

    private async Task ReadyAsync()
    {
        await GameEntryPoint.Architecture.WaitUntilReadyAsync().ConfigureAwait(false);
    }
}
