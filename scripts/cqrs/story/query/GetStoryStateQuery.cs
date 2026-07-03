using GFrameworkTemplate.scripts.cqrs.story.query.result;
using GFrameworkTemplate.scripts.model.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.story.query;

/// <summary>
///     查询故事状态
/// </summary>
public sealed class GetStoryStateQuery : AbstractQuery<StoryStateResult>
{
    protected override StoryStateResult OnDo()
    {
        var model = this.GetModel<StoryStateModel>()!;
        return new StoryStateResult
        {
            IsPlaying = model.IsPlaying,
            PlayingJson = model.PlayingJson,
            CurrentIndex = model.CurrentIndex,
            CommandCount = model.Commands.Count,
            TalkBranch = model.TalkBranch,
            CanNotChoose = model.CanNotChoose,
            PendingGoto = model.PendingGoto,
            AutoPlayDelay = model.AutoPlayDelay,
            WordSpeed = model.WordSpeed
        };
    }
}
