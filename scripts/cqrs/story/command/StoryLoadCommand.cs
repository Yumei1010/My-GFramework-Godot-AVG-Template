using GFrameworkTemplate.global;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

public sealed class StoryLoadCommand : AbstractCommand
{
    public required string StoryName { get; set; }

    protected override void OnExecute()
    {
        _ = this.GetSystem<StoryEngine>().LoadAndPlay(StoryName);
    }
}
