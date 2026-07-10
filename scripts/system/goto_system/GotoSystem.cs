using GFrameworkTemplate.scripts.cqrs.@goto.@event;

namespace GFrameworkTemplate.scripts.system.goto_system;

/// <summary>
///     跳转系统——独立 ISystem，通过 ChangeGotoCommand 驱动
/// </summary>
[Log]
[ContextAware]
public sealed partial class GotoSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase)
    {
        _log.Debug("System initialized: GotoSystem");
    }

    public void Init()
    {
        
    }

    public void Destroy()
    {
        _log.Debug("System destroyed: GotoSystem");
    }

    /// <summary>
    ///     发送跳转导航事件
    /// </summary>
    public void Navigate(string targetPath)
    {
        this.SendEvent(new GotoNavigatedEvent { TargetFilePath = targetPath });
    }
}
