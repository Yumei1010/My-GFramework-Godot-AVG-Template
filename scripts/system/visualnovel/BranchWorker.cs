using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.system.visualnovel;

public sealed class BranchWorker : IStoryCommandWorker
{
    public async Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var branch = (BranchCommand)cmd;
        ctx.SendEvent(new VisualNovelBranchTriggeredEvent { Options = branch.Options });

        var tcs = new TaskCompletionSource<string?>();
        var sub = ctx.RegisterEvent<VisualNovelBranchChosenEvent>(e => tcs.TrySetResult(e.OptionId));

        var chosenId = await tcs.Task;
        sub.UnRegister();

        if (chosenId != null)
            ctx.TalkBranch.Add(chosenId);
    }
}
