using GFrameworkTemplate.scripts.cqrs.story.@event;
using GFrameworkTemplate.global;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

public sealed class StoryFireCustomEventCommand : AbstractAsyncCommand
{
    public required string EventName { get; set; }

    protected override async Task OnExecuteAsync()
    {
        this.SendEvent(new StoryCustomEventFiredEvent { EventName = EventName });
        await this.GetSystem<StoryEngine>().WaitForAdvance();
    }
}
