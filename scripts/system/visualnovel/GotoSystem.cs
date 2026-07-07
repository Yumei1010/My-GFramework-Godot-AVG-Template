using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.system.visualnovel;

/// <summary>
///     跳转系统——独立 ISystem，通过 ChangeGotoCommand 驱动
/// </summary>
[Log]
[ContextAware]
public sealed partial class GotoSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    /// <summary>
    ///     发送跳转导航事件
    /// </summary>
    public void Navigate(string targetPath)
    {
        this.SendEvent(new VisualNovelGotoNavigatedEvent { TargetFilePath = targetPath });
    }
}
