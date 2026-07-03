using GFrameworkTemplate.scripts.cqrs.background.command;

namespace GFrameworkTemplate.scripts.system.background;

/// <summary>
///     背景系统——纯 ISystem，通过 SendCommand 操作 Model
/// </summary>
[Log]
[ContextAware]
public sealed partial class BackgroundSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    public void Change(string filePath) =>
        this.SendCommand(new SetBackgroundCommand { FilePath = filePath });
}
