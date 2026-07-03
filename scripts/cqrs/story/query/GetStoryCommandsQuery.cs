using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.story.query.result;
using GFrameworkTemplate.scripts.model.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.story.query;

/// <summary>
///     查询当前已加载的故事命令列表
/// </summary>
public sealed class GetStoryCommandsQuery : AbstractQuery<StoryCommandsResult>
{
    protected override StoryCommandsResult OnDo()
    {
        var model = this.GetModel<StoryStateModel>()!;
        return new StoryCommandsResult { Commands = model.Commands };
    }
}
