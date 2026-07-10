using GFrameworkTemplate.scripts.model.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

/// <summary>
///     StorySetPlayingCommand —— 写入 IsPlaying 播放状态
/// </summary>
public sealed class StorySetPlayingCommand : AbstractCommand
{
    public required bool Playing { get; set; }
    protected override void OnExecute() => this.GetModel<StoryStateModel>()!.IsPlaying = Playing;
}
