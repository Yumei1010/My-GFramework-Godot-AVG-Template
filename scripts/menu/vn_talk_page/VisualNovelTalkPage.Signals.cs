using GFrameworkTemplate.scripts.cqrs.story.command;

namespace GFrameworkTemplate.scripts.menu;

public partial class VisualNovelTalkPage
{
    private void ConnectPageSignals()
    {
        ClickArea.GuiInput += args =>
        {
            if (args is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
                this.SendCommand(new AdvanceStoryCommand());
        };
    }
}
