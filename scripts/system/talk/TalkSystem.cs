using GFrameworkTemplate.scripts.cqrs.talk.command;
using GFrameworkTemplate.scripts.cqrs.talk.query;
using GFrameworkTemplate.scripts.cqrs.talk.query.result;

namespace GFrameworkTemplate.scripts.system.talk;

/// <summary>
///     对话系统——纯 ISystem，通过 SendCommand/SendQuery 操作 Model
/// </summary>
[Log]
[ContextAware]
public sealed partial class TalkSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    public void Toggle() =>
        this.SendCommand(new SetTalkVisibleCommand
            { Visible = !this.SendQuery(new GetTalkVisibleQuery()).Visible });

    public void Show() => this.SendCommand(new SetTalkVisibleCommand { Visible = true });
    public void Hide() => this.SendCommand(new SetTalkVisibleCommand { Visible = false });
    public bool Visible => this.SendQuery(new GetTalkVisibleQuery()).Visible;
}
