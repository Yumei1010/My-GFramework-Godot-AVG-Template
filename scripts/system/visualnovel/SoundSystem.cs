using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.cqrs.sound.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.system.visualnovel;

/// <summary>
///     音频系统——BGM/SFX 管理 + 故事命令执行
/// </summary>
[Log]
[ContextAware]
public sealed partial class SoundSystem : ISystem, IStoryExecutionSystem
{
    public string CommandType => "sound";
    public event Action<string>? BgmRequested;
    public event Action<string>? SfxRequested;
    public void OnArchitecturePhase(ArchitecturePhase phase) { } public void Init() { } public void Destroy() { }

    public void PlayBgm(string name) { this.SendCommand(new PlayBgmCommand { LogicalName = name }); BgmRequested?.Invoke(name); }
    public void PlaySfx(string name) => SfxRequested?.Invoke(name);

    async Task IStoryExecutionSystem.ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var s = (SoundCommand)cmd;
        ctx.SendEvent(new VisualNovelSoundTriggeredEvent { SoundType = s.SoundType, FilePath = s.FilePath ?? "" });
    }
}
