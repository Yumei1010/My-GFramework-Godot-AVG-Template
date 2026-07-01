using GFrameworkTemplate.scripts.core.story;

namespace GFrameworkTemplate.scripts.system.visualnovel;

public interface IStoryCommandWorker
{
    Task ExecuteAsync(StoryCommand cmd, EngineContext ctx);
}
