using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.story.command;
using GFrameworkTemplate.scripts.cqrs.story.query;
using GFrameworkTemplate.scripts.cqrs.story.query.result;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.data.story;

namespace GFrameworkTemplate.scripts.system.visualnovel;

/// <summary>
///     故事引擎系统——JSON 驱动视觉小说解释器
///     通过 SendCommand 写模型，SendQuery 读模型
/// </summary>
[Log]
[ContextAware]
public sealed partial class StoryEngineSystem : ISystem
{
    private readonly EngineContext _ctx;
    private readonly Dictionary<string, IStoryExecutionSystem> _executors = new();

    public StoryEngineSystem() => _ctx = new EngineContext(this);

    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init()
    {
        foreach (var sys in new IStoryExecutionSystem[]
        {
            this.GetSystem<TalkSystem>()!,
            this.GetSystem<BackgroundSystem>()!,
            this.GetSystem<TachieSystem>()!,
            this.GetSystem<SoundSystem>()!,
            this.GetSystem<BranchSystem>()!,
            this.GetSystem<GotoSystem>()!,
            this.GetSystem<EventSystem>()!
        })
        _executors[sys.CommandType] = sys;
    }
    public void Destroy() { }

    private StoryStateResult State => this.SendQuery(new GetStoryStateQuery());

    public async Task LoadAndPlay(string logicName)
    {
        var jsonPath = StoryResourceMapper.ResolveJsonPath(logicName);
        if (string.IsNullOrEmpty(jsonPath))
        { 
            _log.Error($"json script not found: {logicName}");
            return; 
        }

        var json = await StoryResourceMapper.LoadJsonAsync(jsonPath);
        if (string.IsNullOrEmpty(json)) 
        { 
            _log.Error($"failed to load json script: {jsonPath}");
            return; 
        }

        var script = StoryParser.ParseStory(json);
        this.SendCommand(new UpdateStoryStateCommand
        {
            Commands = script.Content, CurrentIndex = 0, IsPlaying = true,
            PlayingJson = jsonPath, PendingGoto = null,
            TalkBranch = new List<string>(), CanNotChoose = new List<string>()
        });

        this.SendEvent(new VisualNovelStoryLoadedEvent { CommandCount = script.Content.Count });
        _log.Debug($"Story loaded: {jsonPath} ({script.Content.Count} orders)");

        await PlayLoop();
    }

    private async Task PlayLoop()
    {
        var state = State;
        while (state.IsPlaying && state.CurrentIndex < state.CommandCount)
        {
            var cmds = this.SendQuery(new GetStoryCommandsQuery());
            var cmdsList = cmds.Commands;
            var idx = state.CurrentIndex;
            var cmd = cmdsList[idx];
            this.SendCommand(new UpdateStoryStateCommand { CurrentIndex = idx + 1 });

            if (!ShouldExecute(cmd)) { state = State; continue; }

            if (cmd.HideLabels)
                this.SendEvent<VisualNovelAdvanceRequestedEvent>();

            if (_executors.TryGetValue(cmd.Type, out var executor))
                await executor.ExecuteAsync(cmd, _ctx);

            if (cmd.Wait.HasValue)
                await Task.Delay(TimeSpan.FromSeconds(cmd.Wait.Value));

            state = State;
        }

        if (state.PendingGoto != null)
        {
            var target = state.PendingGoto;
            this.SendCommand(new UpdateStoryStateCommand { PendingGoto = null });
            await LoadAndPlay(target);
            return;
        }

        if (state.CurrentIndex >= state.CommandCount)
        {
            this.SendCommand(new UpdateStoryStateCommand { IsPlaying = false });
            this.SendEvent<VisualNovelStoryFinishedEvent>();
            _log.Debug("story finished");
        }
    }

    private bool ShouldExecute(StoryCommand cmd)
    {
        if (string.IsNullOrEmpty(cmd.Branch)) 
            return true;
        var s = State;
        return s.TalkBranch.Contains(cmd.Branch) && !s.CanNotChoose.Contains(cmd.Branch);
    }

    public static void RegisterJson(string name, string path) => StoryResourceMapper.RegisterJson(name, path);

    public void Advance()
    {
        _ctx.WaitSource?.TrySetResult(true);
        this.SendEvent<VisualNovelAdvanceRequestedEvent>();
    }

    public void ChooseBranch(string optionId) =>
        this.SendEvent(new VisualNovelBranchChosenEvent { OptionId = optionId });

    public bool IsPlaying => State.IsPlaying;
    public string CurrentJsonPath => State.PlayingJson;
    public IReadOnlyList<string> TalkBranch => State.TalkBranch;
    public IReadOnlyList<string> CanNotChoose => State.CanNotChoose;

    public void SetAutoPlay(float? delay) =>
        this.SendCommand(new UpdateStoryStateCommand { AutoPlayDelay = delay });

    public void SetWordSpeed(float speed) =>
        this.SendCommand(new UpdateStoryStateCommand { WordSpeed = speed });

    public void AddCannotChoose(string id)
    {
        var list = new List<string>(State.CanNotChoose) { id };
        this.SendCommand(new UpdateStoryStateCommand { CanNotChoose = list });
    }

    public void RemoveCannotChoose(string id)
    {
        var list = new List<string>(State.CanNotChoose);
        list.Remove(id);
        this.SendCommand(new UpdateStoryStateCommand { CanNotChoose = list });
    }

    public void Stop()
    {
        this.SendCommand(new UpdateStoryStateCommand { IsPlaying = false });
        _ctx.WaitSource?.TrySetResult(false);
    }
}
