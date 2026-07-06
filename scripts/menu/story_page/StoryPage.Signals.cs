using GFrameworkTemplate.scripts.cqrs.player.command;

namespace GFrameworkTemplate.scripts.menu.story_page;

public partial class StoryPage
{
    private void ConnectSignal()
    {
        InputDetector.Pressed += OnInputDetectorPressed;
    }

    private void OnInputDetectorPressed()
    {
        this.SendCommand(new PlayerClickCommand());
    }
}
