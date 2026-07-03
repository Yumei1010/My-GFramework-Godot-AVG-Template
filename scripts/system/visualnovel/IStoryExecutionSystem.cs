using GFrameworkTemplate.scripts.core.story;

namespace GFrameworkTemplate.scripts.system.visualnovel;

/// <summary>
///     故事命令执行器接口——每种命令类型对应一个 ISystem 实现
/// </summary>
public interface IStoryExecutionSystem
{
    /// <summary>处理的命令类型字符串</summary>
    string CommandType { get; }

    /// <summary>异步执行命令</summary>
    Task ExecuteAsync(StoryCommand cmd, EngineContext ctx);
}
