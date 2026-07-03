using GFrameworkTemplate.scripts.model.talk;

namespace GFrameworkTemplate.scripts.system.talk;

/// <summary>
///     对话系统——纯 ISystem，通过 TalkModel 管理显隐
/// </summary>
[Log]
[ContextAware]
public sealed partial class TalkSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    private TalkModel Model => this.GetModel<TalkModel>()!;

    public void Toggle() => Model.Visible = !Model.Visible;
    public void Show() => Model.Visible = true;
    public void Hide() => Model.Visible = false;
    public bool Visible => Model.Visible;
}
