using GFramework.Core.Abstractions.command;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

/// <summary>
///     加载并播放故事脚本
/// </summary>
public sealed class LoadStoryCommand : AbstractCommand
{
    public required string StoryName { get; set; }

    protected override void OnExecute()
    {
        _ = this.GetSystem<StoryEngineSystem>().LoadAndPlay(StoryName);
    }
}
