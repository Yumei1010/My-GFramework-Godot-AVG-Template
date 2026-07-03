using GFramework.Core.Abstractions.enums;
using GFramework.Core.Abstractions.system;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.data.story;
using GFrameworkTemplate.scripts.entities.story_command_worker;

namespace GFrameworkTemplate.scripts.system.visualnovel;

/// <summary>
///     故事引擎系统——JSON 驱动视觉小说解释器，通过 ISystem 注册到 DI
/// </summary>
[Log]
[ContextAware]
public sealed partial class StoryEngineSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }
    private List<StoryCommand> _commands = new();
    private int _currentIndex;
    private readonly EngineContext _ctx;
    private static readonly Dictionary<string, IStoryCommandWorker> Workers = new()
    {
        ["talk"] = new TalkWorker(),
        ["background"] = new BackgroundWorker(),
        ["tachie"] = new TachieWorker(),
        ["sound"] = new SoundWorker(),
        ["branch"] = new BranchWorker(),
        ["goto"] = new GotoWorker(),
        ["event"] = new EventWorker()
    };

    public StoryEngineSystem() => _ctx = new EngineContext(this);

    public async Task LoadAndPlay(string logicName)
    {
        var jsonPath = StoryResourceMapper.ResolveJsonPath(logicName);
        if (string.IsNullOrEmpty(jsonPath))
        {
            _log.Error($"找不到脚本: {logicName}");
            return;
        }

        var json = await StoryResourceMapper.LoadJsonAsync(jsonPath);
        if (string.IsNullOrEmpty(json))
        {
            _log.Error($"加载脚本失败: {jsonPath}");
            return;
        }

        var script = StoryParser.ParseStory(json);
        _commands = script.Content;
        _currentIndex = 0;
        _ctx.IsPlaying = true;
        _ctx.PlayingJson = jsonPath;
        _ctx.PendingGoto = null;
        _ctx.TalkBranch.Clear();
        _ctx.CanNotChoose.Clear();

        this.SendEvent(new VisualNovelStoryLoadedEvent { CommandCount = _commands.Count });
        _log.Debug($"故事加载完成: {jsonPath} ({_commands.Count} 条命令)");

        await PlayLoop();
    }

    private async Task PlayLoop()
    {
        while (_ctx.IsPlaying && _currentIndex < _commands.Count)
        {
            var cmd = _commands[_currentIndex];
            _currentIndex++;

            if (!ShouldExecute(cmd))
                continue;

            if (cmd.HideLabels)
                this.SendEvent<VisualNovelAdvanceRequestedEvent>();

            if (Workers.TryGetValue(cmd.Type, out var worker))
                await worker.ExecuteAsync(cmd, _ctx);

            if (cmd.Wait.HasValue)
                await Task.Delay(TimeSpan.FromSeconds(cmd.Wait.Value));
        }

        if (_ctx.PendingGoto != null)
        {
            var target = _ctx.PendingGoto;
            _ctx.PendingGoto = null;
            await LoadAndPlay(target);
            return;
        }

        if (_currentIndex >= _commands.Count)
        {
            _ctx.IsPlaying = false;
            this.SendEvent<VisualNovelStoryFinishedEvent>();
            _log.Debug("故事播放结束");
        }
    }

    private bool ShouldExecute(StoryCommand cmd)
    {
        if (string.IsNullOrEmpty(cmd.Branch))
            return true;
        return _ctx.TalkBranch.Contains(cmd.Branch) && !_ctx.CanNotChoose.Contains(cmd.Branch);
    }

    public static void RegisterJson(string name, string path) => StoryResourceMapper.RegisterJson(name, path);

    public void Advance()
    {
        _ctx.WaitSource?.TrySetResult(true);
        this.SendEvent<VisualNovelAdvanceRequestedEvent>();
    }

    public void ChooseBranch(string optionId) =>
        this.SendEvent(new VisualNovelBranchChosenEvent { OptionId = optionId });

    public bool IsPlaying => _ctx.IsPlaying;
    public string CurrentJsonPath => _ctx.PlayingJson;
    public IReadOnlyList<string> TalkBranch => _ctx.TalkBranch;
    public IReadOnlyList<string> CanNotChoose => _ctx.CanNotChoose;

    public void SetAutoPlay(float? delay) => _ctx.AutoPlayDelay = delay;
    public void SetWordSpeed(float speed) => _ctx.WordSpeed = speed;
    public void AddCannotChoose(string id) => _ctx.CanNotChoose.Add(id);
    public void RemoveCannotChoose(string id) => _ctx.CanNotChoose.Remove(id);

    public void Stop()
    {
        _ctx.IsPlaying = false;
        _ctx.WaitSource?.TrySetResult(false);
    }
}
