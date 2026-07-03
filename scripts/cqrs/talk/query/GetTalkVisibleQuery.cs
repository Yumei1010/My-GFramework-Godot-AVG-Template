using GFrameworkTemplate.scripts.cqrs.talk.query.result;
using GFrameworkTemplate.scripts.model.talk;

namespace GFrameworkTemplate.scripts.cqrs.talk.query;

public sealed class GetTalkVisibleQuery : AbstractQuery<GetTalkVisibleResult>
{
    protected override GetTalkVisibleResult OnDo()
    {
        return new GetTalkVisibleResult { Visible = this.GetModel<TalkModel>()!.Visible };
    }
}
