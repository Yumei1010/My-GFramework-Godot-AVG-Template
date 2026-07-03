using GFramework.Core.extensions;
using GFrameworkTemplate.scripts.system.visualnovel;
using Godot;

namespace GFrameworkTemplate.scripts.menu;

public partial class VisualNovelTalkPage
{
    private void ConnectPageSignals()
    {
        ClickArea.GuiInput += args =>
        {
            if (args is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
            {
                this.GetUtility<StoryEngineSystem>()?.Advance();
            }
        };
    }
}
