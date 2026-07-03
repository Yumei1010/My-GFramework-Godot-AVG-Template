using GFrameworkTemplate.scripts.model.background;

namespace GFrameworkTemplate.scripts.system.background;

/// <summary>
///     背景系统——纯 ISystem，通过 BackgroundModel 管理当前路径
/// </summary>
[Log]
[ContextAware]
public sealed partial class BackgroundSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    private BackgroundModel Model => this.GetModel<BackgroundModel>()!;

    public void Change(string filePath)
    {
        Model.CurrentPath = filePath;
        _log.Debug($"背景切换: {filePath}");
    }
}
