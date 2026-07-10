using System.Text.Json;

using GFrameworkTemplate.scripts.core.story;
namespace GFrameworkTemplate.scripts.cqrs.visualnovel.command;

/// <summary>
///     音频命令——播放音效或音乐
/// </summary>
public sealed class SoundCommand : StoryCommand
{
    public string SoundType { get; set; } = "oneSound";

    /// <summary>从 JSON 元素构造 SoundCommand</summary>
    public static SoundCommand FromJson(JsonElement element)
    {
        var cmd = new SoundCommand
        {
            SoundType = StoryParser.GetString(element, "sound_type") ?? "oneSound"
        };
        return cmd;
    }
}
