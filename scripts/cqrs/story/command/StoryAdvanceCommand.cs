using GFrameworkTemplate.global;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

/// <summary>
///     StoryAdvanceCommand —— 推进到下一个故事命令
/// </summary>
public sealed class StoryAdvanceCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<StoryEngine>().Advance();
    }
}
