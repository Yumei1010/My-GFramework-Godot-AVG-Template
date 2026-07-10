using GFrameworkTemplate.global;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

/// <summary>
///     StoryLoadCommand —— 加载并播放指定故事脚本
/// </summary>
public sealed class StoryLoadCommand : AbstractCommand
{
    public required string StoryName { get; set; }

    protected override void OnExecute()
    {
        _ = this.GetSystem<StoryEngine>().LoadAndPlay(StoryName);
    }
}
