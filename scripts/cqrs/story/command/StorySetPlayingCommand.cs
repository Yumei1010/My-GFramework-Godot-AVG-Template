using GFrameworkTemplate.scripts.model.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

public sealed class StorySetPlayingCommand : AbstractCommand
{
    public required bool Playing { get; set; }
    protected override void OnExecute() => this.GetModel<StoryStateModel>()!.IsPlaying = Playing;
}
