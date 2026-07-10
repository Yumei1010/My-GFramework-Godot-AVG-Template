using GFrameworkTemplate.global;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

public sealed class StoryAdvanceCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<StoryEngine>().Advance();
    }
}
