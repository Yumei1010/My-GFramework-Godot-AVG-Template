using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.data.story;
using GFrameworkTemplate.scripts.entities.story_command_worker;
using GFrameworkTemplate.scripts.model.visualnovel;

namespace GFrameworkTemplate.scripts.system.visualnovel;

/// <summary>
///     故事引擎系统——JSON 驱动视觉小说解释器
///     读写 StoryStateModel，通过 Worker 分发命令执行
/// </summary>
[Log]
[ContextAware]
public sealed partial class StoryEngineSystem : ISystem
{
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

    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    private StoryStateModel Model => this.GetModel<StoryStateModel>()!;

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
        Model.Commands = script.Content;
        Model.CurrentIndex = 0;
        Model.IsPlaying = true;
        Model.PlayingJson = jsonPath;
        Model.PendingGoto = null;
        Model.TalkBranch.Clear();
        Model.CanNotChoose.Clear();

        this.SendEvent(new VisualNovelStoryLoadedEvent { CommandCount = Model.Commands.Count });
        _log.Debug($"故事加载完成: {jsonPath} ({Model.Commands.Count} 条命令)");

        await PlayLoop();
    }

    private async Task PlayLoop()
    {
        var model = Model;
        while (model.IsPlaying && model.CurrentIndex < model.Commands.Count)
        {
            var cmd = model.Commands[model.CurrentIndex];
            model.CurrentIndex++;

            if (!ShouldExecute(cmd)) continue;

            if (cmd.HideLabels)
                this.SendEvent<VisualNovelAdvanceRequestedEvent>();

            if (Workers.TryGetValue(cmd.Type, out var worker))
                await worker.ExecuteAsync(cmd, _ctx);

            if (cmd.Wait.HasValue)
                await Task.Delay(TimeSpan.FromSeconds(cmd.Wait.Value));
        }

        if (model.PendingGoto != null)
        {
            var target = model.PendingGoto;
            model.PendingGoto = null;
            await LoadAndPlay(target);
            return;
        }

        if (model.CurrentIndex >= model.Commands.Count)
        {
            model.IsPlaying = false;
            this.SendEvent<VisualNovelStoryFinishedEvent>();
            _log.Debug("故事播放结束");
        }
    }

    private bool ShouldExecute(StoryCommand cmd)
    {
        if (string.IsNullOrEmpty(cmd.Branch)) return true;
        var model = Model;
        return model.TalkBranch.Contains(cmd.Branch) && !model.CanNotChoose.Contains(cmd.Branch);
    }

    public static void RegisterJson(string name, string path) => StoryResourceMapper.RegisterJson(name, path);

    public void Advance()
    {
        _ctx.WaitSource?.TrySetResult(true);
        this.SendEvent<VisualNovelAdvanceRequestedEvent>();
    }

    public void ChooseBranch(string optionId) =>
        this.SendEvent(new VisualNovelBranchChosenEvent { OptionId = optionId });

    public bool IsPlaying => Model.IsPlaying;
    public string CurrentJsonPath => Model.PlayingJson;
    public IReadOnlyList<string> TalkBranch => Model.TalkBranch;
    public IReadOnlyList<string> CanNotChoose => Model.CanNotChoose;

    public void SetAutoPlay(float? delay) => Model.AutoPlayDelay = delay;
    public void SetWordSpeed(float speed) => Model.WordSpeed = speed;
    public void AddCannotChoose(string id) => Model.CanNotChoose.Add(id);
    public void RemoveCannotChoose(string id) => Model.CanNotChoose.Remove(id);

    public void Stop()
    {
        Model.IsPlaying = false;
        _ctx.WaitSource?.TrySetResult(false);
    }
}
