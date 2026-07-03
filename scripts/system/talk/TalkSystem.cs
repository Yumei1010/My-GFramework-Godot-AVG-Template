namespace GFrameworkTemplate.scripts.system.talk;

/// <summary>
///     对话系统——纯 ISystem，管理对话框显隐状态
/// </summary>
[Log]
[ContextAware]
public sealed partial class TalkSystem : ISystem
{
    public bool Visible { get; private set; }

    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    public void Toggle() => Visible = !Visible;
    public void Show() => Visible = true;
    public void Hide() => Visible = false;
}
