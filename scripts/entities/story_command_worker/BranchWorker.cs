using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;

namespace GFrameworkTemplate.scripts.entities.story_command_worker;

public sealed class BranchWorker : IStoryCommandWorker
{
    public async Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        var branch = (BranchCommand)cmd;

        var tcs = new TaskCompletionSource<string?>();
        var sub = ctx.RegisterEvent<VisualNovelBranchChosenEvent>(e => tcs.TrySetResult(e.OptionId));

        var chosenId = await tcs.Task;
        sub.UnRegister();

        if (chosenId != null)
            ctx.TalkBranch.Add(chosenId);
    }
}
