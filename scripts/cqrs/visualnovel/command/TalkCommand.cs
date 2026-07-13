using System.Text.Json;

using GFrameworkTemplate.scripts.core.story;
namespace GFrameworkTemplate.scripts.cqrs.visualnovel.command;

/// <summary>
///     对话命令——显示角色对话或旁白
/// </summary>
/// <summary>
///     TalkCommand —— JSON 对话指令数据
/// </summary>
public sealed class TalkCommand : StoryCommand
{
    public string? Talker { get; set; }
    public bool IsCenter { get; set; }
    public bool Center { get; set; }
    public bool Code { get; set; }
    public string TalkContent { get; set; } = string.Empty;
    public string? AvatarPath { get; set; }

    /// <summary>从 JSON 元素构造 TalkCommand</summary>
    public static TalkCommand FromJson(JsonElement element)
    {
        var cmd = new TalkCommand
        {
            Talker = StoryParser.GetString(element, "talker"),
            IsCenter = StoryParser.GetString(element, "is_center") == "1",
            Center = StoryParser.GetString(element, "center") == "1",
            Code = StoryParser.GetString(element, "code") == "1",
            TalkContent = StoryParser.GetString(element, "talk_content") ?? string.Empty,
            AvatarPath = StoryParser.GetString(element, "avatar_path")
        };
        return cmd;
    }
}
