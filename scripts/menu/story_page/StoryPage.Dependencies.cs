using GFrameworkTemplate.global;
using GFrameworkTemplate.scripts.data.story;
using GFrameworkTemplate.scripts.entities.background_view;
using GFrameworkTemplate.scripts.entities.branch_view;
using GFrameworkTemplate.scripts.entities.sound_view;
using GFrameworkTemplate.scripts.entities.tachie_view;
using GFrameworkTemplate.scripts.entities.talk_view;
using GFrameworkTemplate.global;

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

        RegisterResources();
        _ = AutoPlayTestStory();
    }

    private static void RegisterResources()
    {
        StoryEngine.RegisterJson("test_prologue", "res://assets/story/test_prologue.json");
        StoryEngine.RegisterJson("test_path_help", "res://assets/story/test_path_help.json");
        StoryEngine.RegisterJson("test_path_doubt", "res://assets/story/test_path_doubt.json");
        StoryEngine.RegisterJson("test_path_leave", "res://assets/story/test_path_leave.json");
    }

    private async Task AutoPlayTestStory()
    {
        await this.GetSystem<StoryEngine>().LoadAndPlay("test_prologue");
    }
}
