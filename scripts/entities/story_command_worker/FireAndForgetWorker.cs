using GFrameworkTemplate.scripts.core.story;

namespace GFrameworkTemplate.scripts.entities.story_command_worker;

/// <summary>
///     即发即忘型 Worker——通过 ctx 发送事件后立即完成
/// </summary>
public abstract class FireAndForgetWorker<TCommand, TEvent> : IStoryCommandWorker
    where TCommand : StoryCommand
    where TEvent : class
{
    public Task ExecuteAsync(StoryCommand cmd, EngineContext ctx)
    {
        ctx.SendEvent(CreateEvent((TCommand)cmd));
        return Task.CompletedTask;
    }

    protected abstract TEvent CreateEvent(TCommand cmd);
}
