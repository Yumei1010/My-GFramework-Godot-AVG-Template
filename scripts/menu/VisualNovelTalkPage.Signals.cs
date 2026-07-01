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
                // 使用者在此调用故事引擎的 Advance() 方法
            }
        };
    }
}
