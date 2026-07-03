using GFrameworkTemplate.scripts.core.controller;
using GFrameworkTemplate.scripts.enums.input;

namespace GFrameworkTemplate.global;

/// <summary>
///     全局输入控制器，负责处理全局输入事件
/// </summary>
[Log]
public partial class GlobalInputController : GameInputController
{
    protected override bool AcceptPhase(InputPhase phase)
    {
        return phase is InputPhase.Global or InputPhase.Paused;
    }

    protected override void Handle(InputPhase phase, InputEvent @event)
    {
        if (!@event.IsActionPressed("ui_cancel"))
            return;

        GetViewport().SetInputAsHandled();
    }
}
