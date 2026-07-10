using GFrameworkTemplate.scripts.cqrs.background.@event;

namespace GFrameworkTemplate.scripts.system.background_system;

[Log]
[ContextAware]
/// <summary>
///     背景系统 —— 背景切换延迟与事件发送
/// </summary>
public sealed partial class BackgroundSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase)
    {
        _log.Debug("System initialized: BackgroundSystem");
    }

    public void Init()
    {
        
    }

    public void Destroy()
    {
        _log.Debug("System destroyed: BackgroundSystem");
    }

    public async Task Change(string filePath, bool waitTweenEnd, float delay)
    {
        if (delay > 0) await Task.Delay(TimeSpan.FromSeconds(delay));

        this.SendEvent(new BackgroundChangedEvent{ FilePath = filePath, WaitTweenEnd = waitTweenEnd, Delay = delay});

        if (waitTweenEnd) await Task.Delay(TimeSpan.FromSeconds(0.5f));
    }
}
