using GFrameworkTemplate.scripts.component.branch_option;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.system.branch;

/// <summary>
///     分支系统——纯 ISystem，管理分支选项和选择逻辑
/// </summary>
[Log]
[ContextAware]
public sealed partial class BranchSystem : ISystem
{
    private StoryEngineSystem _engine = null!;

    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { _engine = this.GetSystem<StoryEngineSystem>()!; }
    public void Destroy() { }

    public void Choose(string optionId)
    {
        _engine.ChooseBranch(optionId);
    }
}
