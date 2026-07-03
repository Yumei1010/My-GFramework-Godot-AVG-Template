using GFrameworkTemplate.scripts.core.story;

namespace GFrameworkTemplate.scripts.entities.story_command_worker;

/// <summary>
///     即发即忘型 Worker——无异步等待，立即完成
/// </summary>
public abstract class FireAndForgetWorker<TCommand> : IStoryCommandWorker
    where TCommand : StoryCommand
{
    public virtual Task ExecuteAsync(StoryCommand cmd, EngineContext ctx) => Task.CompletedTask;
}
