using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.cqrs.tachie.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.enums.visualnovel;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.system.tachie;

/// <summary>
///     立绘系统——状态管理 + 故事命令执行
/// </summary>
[Log]
[ContextAware]
public sealed partial class TachieSystem : ISystem, IStoryExecutionSystem
{
    public string CommandType => "tachie";
    public event Action? Changed;
    public void OnArchitecturePhase(ArchitecturePhase phase) { } public void Init() { } public void Destroy() { }

    public void Handle(TachieOperation type, string charName, string filePath)
    {
        this.SendCommand(new UpdateTachieCommand { Type = type, CharName = charName, FilePath = filePath });
        Changed?.Invoke();
    }

    async Task IStoryExecutionSystem.ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var t = (TachieCommand)cmd;
        ctx.SendEvent(new VisualNovelTachieTriggeredEvent { Tachies = t.Tachies });
    }
}
