using GFrameworkTemplate.scripts.cqrs.tachie.command;
using GFrameworkTemplate.scripts.enums.visualnovel;

namespace GFrameworkTemplate.scripts.system.tachie;

/// <summary>
///     立绘系统——纯 ISystem，通过 SendCommand 操作 Model
/// </summary>
[Log]
[ContextAware]
public sealed partial class TachieSystem : ISystem
{
    public event Action? Changed;

    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    public void Handle(TachieOperation type, string charName, string filePath)
    {
        this.SendCommand(new UpdateTachieCommand { Type = type, CharName = charName, FilePath = filePath });
        Changed?.Invoke();
    }
}
