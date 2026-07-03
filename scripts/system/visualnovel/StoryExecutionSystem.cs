using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.system.visualnovel;

/// <summary>
///     故事执行系统——ISystem，负责将 StoryCommand 翻译为引擎事件和控制播放节奏
///     内部包含 7 个执行策略（对应 7 种命令类型）
/// </summary>
[Log]
[ContextAware]
public sealed partial class StoryExecutionSystem : ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }

    /// <summary>
    ///     分发命令到对应的执行策略
    /// </summary>
    public async Task Dispatch(StoryCommand cmd, EngineContext ctx)
    {
        switch (cmd)
        {
            case TalkCommand t: await ExecuteTalk(t, ctx); break;
            case BackgroundCommand b: await ExecuteBackground(b, ctx); break;
            case TachieCommand tc: ExecuteTachie(tc, ctx); break;
            case SoundCommand s: ExecuteSound(s, ctx); break;
            case BranchCommand br: await ExecuteBranch(br, ctx); break;
            case GotoCommand g: ExecuteGoto(g, ctx); break;
            case EventCommand e: await ExecuteEvent(e, ctx); break;
        }
    }

    private async Task ExecuteTalk(TalkCommand cmd, EngineContext ctx)
    {
        ctx.SendEvent(new VisualNovelTalkTriggeredEvent
        {
            Talker = cmd.Talker, Content = cmd.TalkContent,
            IsCenter = cmd.IsCenter, AvatarPath = cmd.AvatarPath
        });
        await ctx.AdvanceAsync(cmd.TalkContent.Length * ctx.WordSpeed);
    }

    private async Task ExecuteBackground(BackgroundCommand cmd, EngineContext ctx)
    {
        if (cmd.Delay > 0) await Task.Delay(TimeSpan.FromSeconds(cmd.Delay));
        ctx.SendEvent(new VisualNovelBackgroundTriggeredEvent
        {
            FilePath = cmd.FilePath ?? string.Empty,
            WaitTweenEnd = cmd.WaitTweenEnd, Delay = cmd.Delay
        });
        if (cmd.WaitTweenEnd) await Task.Delay(TimeSpan.FromSeconds(0.5f));
    }

    private void ExecuteTachie(TachieCommand cmd, EngineContext ctx) =>
        ctx.SendEvent(new VisualNovelTachieTriggeredEvent { Tachies = cmd.Tachies });

    private void ExecuteSound(SoundCommand cmd, EngineContext ctx) =>
        ctx.SendEvent(new VisualNovelSoundTriggeredEvent
            { SoundType = cmd.SoundType, FilePath = cmd.FilePath ?? string.Empty });

    private async Task ExecuteBranch(BranchCommand cmd, EngineContext ctx)
    {
        ctx.SendEvent(new VisualNovelBranchTriggeredEvent { Options = cmd.Options });
        var tcs = new TaskCompletionSource<string?>();
        var sub = ctx.RegisterEvent<VisualNovelBranchChosenEvent>(e => tcs.TrySetResult(e.OptionId));
        var chosenId = await tcs.Task;
        sub.UnRegister();
        if (chosenId != null) ctx.TalkBranch.Add(chosenId);
    }

    private void ExecuteGoto(GotoCommand cmd, EngineContext ctx)
    {
        var target = cmd.FilePath;
        if (!string.IsNullOrEmpty(target))
        {
            ctx.SendEvent(new VisualNovelGotoTriggeredEvent { TargetFilePath = target });
            ctx.IsPlaying = false;
            ctx.PendingGoto = target;
        }
    }

    private async Task ExecuteEvent(EventCommand cmd, EngineContext ctx)
    {
        ctx.SendEvent(new VisualNovelCustomEventTriggeredEvent { EventName = cmd.EventName });
        await ctx.AdvanceAsync(0.1f);
    }
}
