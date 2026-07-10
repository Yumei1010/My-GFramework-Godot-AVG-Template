using System.Text.Json;

using GFrameworkTemplate.scripts.core.story;
namespace GFrameworkTemplate.scripts.cqrs.visualnovel.command;

/// <summary>
///     背景命令——切换场景背景
/// </summary>
/// <summary>
///     BackgroundCommand —— JSON 背景指令数据
/// </summary>
public sealed class BackgroundCommand : StoryCommand
{
    public bool WaitTweenEnd { get; set; }
    public float Delay { get; set; }

    /// <summary>从 JSON 元素构造 BackgroundCommand</summary>
    public static BackgroundCommand FromJson(JsonElement element)
    {
        var cmd = new BackgroundCommand
        {
            WaitTweenEnd = StoryParser.GetString(element, "wait_tween_end") == "1",
            Delay = StoryParser.GetFloat(element, "delay") ?? 0f
        };
        return cmd;
    }
}
