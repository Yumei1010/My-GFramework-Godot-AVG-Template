using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.background.command;
using GFrameworkTemplate.scripts.cqrs.background.command.input;
using GFrameworkTemplate.scripts.cqrs.branch.command;
using GFrameworkTemplate.scripts.cqrs.@goto.command;
using GFrameworkTemplate.scripts.cqrs.sound.command;
using GFrameworkTemplate.scripts.cqrs.tachie.command;
using GFrameworkTemplate.scripts.cqrs.talk.command;
using GFrameworkTemplate.scripts.cqrs.talk.command.input;
using GFrameworkTemplate.scripts.cqrs.story.command;
using GFrameworkTemplate.scripts.cqrs.story.query;
using GFrameworkTemplate.scripts.cqrs.story.query.result;
using GFrameworkTemplate.scripts.cqrs.story.@event;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.data.story;
using GFrameworkTemplate.scripts.system.talk_system;

namespace GFrameworkTemplate.global;

/// <summary>
///     故事引擎——JSON ↔ Command 转换边界，最高层调度器
/// </summary>
[Log]
[ContextAware]
public partial class StoryEngine : CanvasLayer, ISystem
{
    private TaskCompletionSource<bool>? _waitSource;

    public void OnArchitecturePhase(ArchitecturePhase phase) => _log.Debug("StoryEngine initialized");
    public void Init() { }
    public void Destroy() => _log.Debug("StoryEngine destroyed");

    private StoryStateResult State => this.SendQuery(new GetStoryStateQuery());

    public async Task LoadAndPlay(string logicName)
    {
        var jsonPath = StoryResourceMapper.ResolveJsonPath(logicName);
        if (string.IsNullOrEmpty(jsonPath)) { _log.Error($"json script not found: {logicName}"); return; }

        var json = await StoryResourceMapper.LoadJsonAsync(jsonPath);
        if (string.IsNullOrEmpty(json)) { _log.Error($"failed to load json script: {jsonPath}"); return; }

        var commands = StoryParser.Parse(json);
        this.SendCommand(new StorySetCommandsCommand { Commands = commands });
        this.SendCommand(new StorySetIndexCommand { Index = 0 });
        this.SendCommand(new StorySetPlayingCommand { Playing = true });
        this.SendCommand(new StorySetJsonCommand { JsonPath = jsonPath });
        this.SendCommand(new StorySetGotoCommand());
        this.SendCommand(new StorySetBranchCommand { Branches = new List<string>() });
        this.SendCommand(new StorySetCannotChooseCommand { CannotChoose = new List<string>() });

        this.SendEvent(new StoryLoadedEvent { CommandCount = commands.Count });
        _log.Debug($"Story loaded: {jsonPath} ({commands.Count} orders)");

        await PlayLoop();
    }

    private async Task PlayLoop()
    {
        var state = State;
        while (state.IsPlaying && state.CurrentIndex < state.CommandCount)
        {
            var cmds = this.SendQuery(new GetStoryCommandsQuery());
            var idx = state.CurrentIndex;
            var cmd = cmds.Commands[idx];
            this.SendCommand(new StorySetIndexCommand { Index = idx + 1 });

            if (!ShouldExecute(cmd)) { state = State; continue; }

            if (cmd.HideLabels)
                this.SendEvent<StoryAdvanceRequestedEvent>();

            switch (cmd.Type)
            {
                case "background":
                    var b = (BackgroundCommand)cmd;
                    await this.SendCommandAsync(new BackgroundChangeCommand(
                        new BackgroundChangeInput { FilePath = b.FilePath ?? "", WaitTweenEnd = b.WaitTweenEnd, Delay = b.Delay }));
                    break;
                case "tachie":
                    this.SendCommand(new TachieApplyCommand((TachieCommand)cmd));
                    break;
                case "talk":
                    var t = (TalkCommand)cmd;
                    await this.SendCommandAsync(new TalkPlayCommand(
                        new TalkPlayInput { Talker = t.Talker ?? "", Content = t.TalkContent, IsCenter = t.IsCenter, RevealSpeed = 0.04f }));
                    break;
                case "branch":
                    await this.SendCommandAsync(new BranchShowCommand { Options = ((BranchCommand)cmd).Options });
                    break;
                case "goto":
                    await this.SendCommandAsync(new GotoNavigateCommand { TargetPath = ((GotoCommand)cmd).FilePath ?? "" });
                    break;
                case "sound":
                    var s = (SoundCommand)cmd;
                    this.SendCommand(new SoundPlayCommand { SoundType = s.SoundType, FilePath = s.FilePath ?? "" });
                    break;
                case "event":
                    var ev = (EventCommand)cmd;
                    await this.SendCommandAsync(new StoryFireCustomEventCommand { EventName = ev.EventName });
                    break;
            }

            if (cmd.Wait.HasValue)
                await Task.Delay(TimeSpan.FromSeconds(cmd.Wait.Value));

            state = State;
        }

        if (state.PendingGoto != null)
        {
            var target = state.PendingGoto;
            this.SendCommand(new StorySetGotoCommand());
            await LoadAndPlay(target);
            return;
        }

        if (state.CurrentIndex >= state.CommandCount)
        {
            this.SendCommand(new StorySetPlayingCommand { Playing = false });
            this.SendEvent<StoryFinishedEvent>();
            _log.Debug("story finished");
        }
    }

    private bool ShouldExecute(StoryCommand cmd)
    {
        if (string.IsNullOrEmpty(cmd.Branch)) return true;
        var s = State;
        return s.TalkBranch.Contains(cmd.Branch) && !s.CanNotChoose.Contains(cmd.Branch);
    }

    public static void RegisterJson(string name, string path) => StoryResourceMapper.RegisterJson(name, path);

    public void Advance()
    {
        var talkSys = this.GetSystem<TalkSystem>();
        if (talkSys != null && !talkSys.IsTextRevealed)
        {
            talkSys.RevealAll();
            return;
        }
        _waitSource?.TrySetResult(true);
        this.SendEvent<StoryAdvanceRequestedEvent>();
    }

    public async Task WaitForAdvance()
    {
        _waitSource = new TaskCompletionSource<bool>();
        await _waitSource.Task;
        _waitSource = null;
    }

    public bool IsPlaying => State.IsPlaying;
    public string CurrentJsonPath => State.PlayingJson;
    public IReadOnlyList<string> TalkBranch => State.TalkBranch;
    public IReadOnlyList<string> CanNotChoose => State.CanNotChoose;

    public void AddCannotChoose(string id)
    {
        var list = new List<string>(State.CanNotChoose) { id };
        this.SendCommand(new StorySetCannotChooseCommand { CannotChoose = list });
    }

    public void Stop()
    {
        this.SendCommand(new StorySetPlayingCommand { Playing = false });
        _waitSource?.TrySetResult(false);
    }
}
