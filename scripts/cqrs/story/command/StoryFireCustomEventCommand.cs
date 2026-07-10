using GFrameworkTemplate.scripts.cqrs.story.@event;
using GFrameworkTemplate.global;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

/// <summary>
///     StoryFireCustomEventCommand —— 触发自定义事件并等待点击
/// </summary>
public sealed class StoryFireCustomEventCommand : AbstractAsyncCommand
{
    public required string EventName { get; set; }

    protected override async Task OnExecuteAsync()
    {
        this.SendEvent(new StoryCustomEventFiredEvent { EventName = EventName });
        await this.GetSystem<StoryEngine>().WaitForAdvance();
    }
}
