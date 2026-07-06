using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.system.visualnovel;

/// <summary>
///     背景系统——独立 ISystem，通过 ChangeBackgroundCommand 驱动
/// </summary>
[Log]
[ContextAware]
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
        if (delay > 0)
            await Task.Delay(TimeSpan.FromSeconds(delay));

        this.SendEvent(new VisualNovelBackgroundChangedEvent
        {
            FilePath = filePath,
            WaitTweenEnd = waitTweenEnd,
            Delay = delay
        });

        if (waitTweenEnd)
            await Task.Delay(TimeSpan.FromSeconds(0.5f));
    }
}
