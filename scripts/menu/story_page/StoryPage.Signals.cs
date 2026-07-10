using GFrameworkTemplate.scripts.cqrs.story.command;

namespace GFrameworkTemplate.scripts.menu.story_page;

public partial class StoryPage
{
    private void ConnectSignal()
    {
        InputDetector.Pressed += OnInputDetectorPressed;
    }

    private void OnInputDetectorPressed()
    {
        this.SendCommand(new StoryAdvanceCommand());
    }
}
