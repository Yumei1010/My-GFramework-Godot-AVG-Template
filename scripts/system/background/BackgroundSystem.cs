namespace GFrameworkTemplate.scripts.system.background;

/// <summary>
///     背景系统——纯 ISystem，管理当前背景路径
/// </summary>
[Log]
[ContextAware]
public sealed partial class BackgroundSystem : ISystem
{
    public string CurrentPath { get; private set; } = string.Empty;

    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    public void Change(string filePath)
    {
        CurrentPath = filePath;
        _log.Debug($"背景切换: {filePath}");
    }
}
