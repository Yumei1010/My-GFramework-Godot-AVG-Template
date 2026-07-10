namespace GFrameworkTemplate.scripts.cqrs.@goto.@event;

/// <summary>
///     GotoNavigatedEvent —— 跳转导航事件
/// </summary>
public sealed class GotoNavigatedEvent
{
    public required string TargetFilePath { get; init; }
}
