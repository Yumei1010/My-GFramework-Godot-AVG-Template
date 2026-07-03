using GFramework.Core.Abstractions.command;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

/// <summary>
///     推进到故事的下一个命令
/// </summary>
public sealed class AdvanceStoryCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<StoryEngineSystem>().Advance();
    }
}
