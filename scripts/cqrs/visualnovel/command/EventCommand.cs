using System.Text.Json;

using GFrameworkTemplate.scripts.core.story;
namespace GFrameworkTemplate.scripts.cqrs.visualnovel.command;

/// <summary>
///     自定义事件命令——触发章节特定逻辑
/// </summary>
/// <summary>
///     EventCommand —— JSON 自定义事件指令数据
/// </summary>
public sealed class EventCommand : StoryCommand
{
    public string EventName { get; set; } = string.Empty;

    /// <summary>从 JSON 元素构造 EventCommand</summary>
    public static EventCommand FromJson(JsonElement element)
    {
        var cmd = new EventCommand
        {
            EventName = StoryParser.GetString(element, "event_name") ?? string.Empty
        };
        return cmd;
    }
}
