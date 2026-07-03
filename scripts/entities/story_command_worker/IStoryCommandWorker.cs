using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;

namespace GFrameworkTemplate.scripts.entities.story_command_worker;

/// <summary>
///     命令执行器接口——每种故事命令类型对应一个实现，由 StoryEngineSystem 按 type 分派
/// </summary>
public interface IStoryCommandWorker
{
    /// <summary>
    ///     异步执行命令
    /// </summary>
    /// <param name="cmd">待执行的故事命令</param>
    /// <param name="ctx">引擎上下文（状态、事件发送、等待机制）</param>
    Task ExecuteAsync(StoryCommand cmd, EngineContext ctx);
}
