using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.background.command;
using GFrameworkTemplate.scripts.cqrs.sound.command;
using GFrameworkTemplate.scripts.cqrs.background.command.input;
using GFrameworkTemplate.scripts.cqrs.branch.command;
using GFrameworkTemplate.scripts.cqrs.@goto.command;
using GFrameworkTemplate.scripts.cqrs.tachie.command;
using GFrameworkTemplate.scripts.cqrs.talk.command;
using GFrameworkTemplate.scripts.cqrs.talk.command.input;
using GFrameworkTemplate.scripts.cqrs.story.command;
using GFrameworkTemplate.scripts.cqrs.story.query;
using GFrameworkTemplate.scripts.cqrs.story.query.result;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.data.story;

namespace GFrameworkTemplate.scripts.system.visualnovel;

/// <summary>
///     故事引擎系统——JSON 驱动视觉小说解释器
/// </summary>
[Log]
[ContextAware]
public sealed partial class StoryEngineSystem : ISystem
{
    private readonly EngineContext _ctx;

    public StoryEngineSystem() => _ctx = new EngineContext(this);

    public void OnArchitecturePhase(ArchitecturePhase phase) =>
        _log.Debug("System initialized: StoryEngineSystem");

    public void Init() { }
    public void Destroy() =>
        _log.Debug("System destroyed: StoryEngineSystem");

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
            var idx = state.CurrentIndex;
            var cmd = cmds.Commands[idx];
            this.SendCommand(new UpdateStoryStateCommand { CurrentIndex = idx + 1 });

            if (!ShouldExecute(cmd)) { state = State; continue; }

            if (cmd.HideLabels)
                this.SendEvent<VisualNovelAdvanceRequestedEvent>();

            switch (cmd.Type)
            {
                case "background":
                    var b = (BackgroundCommand)cmd;
                    await this.SendCommandAsync(new ChangeBackgroundCommand(
                        new ChangeBackgroundCommandInput
                        {
                            FilePath = b.FilePath ?? "", WaitTweenEnd = b.WaitTweenEnd, Delay = b.Delay
                        }));
                    break;
                case "tachie":
                    this.SendCommand(new ChangeTachieCommand { Tachies = ((TachieCommand)cmd).Tachies });
                    break;
                case "talk":
                    var t = (TalkCommand)cmd;
                    await this.SendCommandAsync(new ChangeTalkCommand(
                        new ChangeTalkCommandInput
                        {
                            Talker = t.Talker ?? "", Content = t.TalkContent,
                            IsCenter = t.IsCenter, AvatarPath = t.AvatarPath ?? ""
                        }));
                    break;
                case "branch":
                    await this.SendCommandAsync(new ChangeBranchCommand { Options = ((BranchCommand)cmd).Options });
                    break;
                case "goto":
                    await this.SendCommandAsync(new ChangeGotoCommand { TargetPath = ((GotoCommand)cmd).FilePath ?? "" });
                    break;
                case "sound":
                    var s = (SoundCommand)cmd;
                    this.SendCommand(new ChangeSoundCommand { SoundType = s.SoundType, FilePath = s.FilePath ?? "" });
                    break;
                case "event":
                    var ev = (EventCommand)cmd;
                    this.SendEvent(new VisualNovelCustomEventFiredEvent { EventName = ev.EventName });
                    await _ctx.WaitClickAsync();
                    break;
            }

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

    public async Task WaitForAdvance()
    {
        await _ctx.WaitClickAsync();
    }

    public bool IsPlaying => State.IsPlaying;
    public string CurrentJsonPath => State.PlayingJson;
    public IReadOnlyList<string> TalkBranch => State.TalkBranch;
    public IReadOnlyList<string> CanNotChoose => State.CanNotChoose;

    public void AddCannotChoose(string id)
    {
        var list = new List<string>(State.CanNotChoose) { id };
        this.SendCommand(new UpdateStoryStateCommand { CanNotChoose = list });
    }

    public void Stop()
    {
        this.SendCommand(new UpdateStoryStateCommand { IsPlaying = false });
        _ctx.WaitSource?.TrySetResult(false);
    }
}
