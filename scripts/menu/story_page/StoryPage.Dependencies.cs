using GFrameworkTemplate.global;
using GFrameworkTemplate.scripts.component.background_controller;

namespace GFrameworkTemplate.scripts.menu.story_page;

public partial class StoryPage
{
    private Button InputDetector => GetNode<Button>("%InputDetector");
    private BackgroundController Background => GetNode<BackgroundController>("%Background");

    private async Task ReadyAsync()
    {
        await GameEntryPoint.Architecture.WaitUntilReadyAsync().ConfigureAwait(false);
        _log.Debug("StoryPage Initialized");
    }
}
