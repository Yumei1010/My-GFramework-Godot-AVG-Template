using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.system.event_system;

/// <summary>
///     事件系统——自定义事件触发
/// </summary>
[Log]
[ContextAware]
public sealed partial class EventSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }
}
