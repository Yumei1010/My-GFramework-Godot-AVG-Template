using GFrameworkTemplate.scripts.component.tachie_slot;
using GFrameworkTemplate.scripts.cqrs.tachie.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.enums.visualnovel;

namespace GFrameworkTemplate.scripts.system.tachie_system;

/// <summary>
///     立绘系统——独立 ISystem，通过 ChangeTachieCommand 驱动
/// </summary>
[Log]
[ContextAware]
public sealed partial class TachieSystem : ISystem
{
    public event Action? Changed;
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    /// <summary>
    ///     外部直接操作立绘（非故事引擎路径）
    /// </summary>
    public void Handle(TachieOperation type, string charName, string filePath)
    {
        this.SendCommand(new UpdateTachieCommand { Type = type, CharName = charName, FilePath = filePath });
        Changed?.Invoke();
    }

    /// <summary>
    ///     故事引擎路径：发送立绘事件，由 TachieController 订阅处理
    /// </summary>
    public void Apply(Dictionary<string, TachieSlot> tachies)
    {
        this.SendEvent(new VisualNovelTachieUpdatedEvent { Tachies = tachies });
    }
}
