using System.Text.Json;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;

namespace GFrameworkTemplate.scripts.core.story;

/// <summary>
///     故事解析器——JSON 文本 → List&lt;StoryCommand&gt;
/// </summary>
public static class StoryParser
{
    /// <summary>从 JSON 字符串解析</summary>
    public static List<StoryCommand> Parse(string json)
    {
        var commands = new List<StoryCommand>();
        using var doc = JsonDocument.Parse(json);
        if (!doc.RootElement.TryGetProperty("content", out var contentArray))
            return commands;

        foreach (var element in contentArray.EnumerateArray())
        {
            var cmd = ParseCommand(element);
            if (cmd != null) commands.Add(cmd);
        }
        return commands;
    }

    /// <summary>type 分发到各子类的 FromJson 工厂</summary>
    public static StoryCommand? ParseCommand(JsonElement element)
    {
        var type = element.TryGetProperty("type", out var t) ? t.GetString() : null;

        StoryCommand? cmd = type switch
        {
            "talk" => TalkCommand.FromJson(element),
            "background" => BackgroundCommand.FromJson(element),
            "tachie" => TachieCommand.FromJson(element),
            "sound" => SoundCommand.FromJson(element),
            "branch" => BranchCommand.FromJson(element),
            "goto" => GotoCommand.FromJson(element),
            "event" => EventCommand.FromJson(element),
            _ => null
        };

        if (cmd != null) FillCommon(cmd, element);
        return cmd;
    }

    private static void FillCommon(StoryCommand cmd, JsonElement element)
    {
        cmd.Type = GetString(element, "type") ?? string.Empty;
        cmd.Branch = GetString(element, "branch");
        cmd.HideLabels = GetString(element, "hide_labels") == "1";
        cmd.Wait = GetFloat(element, "wait");
        cmd.FilePath = GetString(element, "file_path");
    }

    public static string? GetString(JsonElement element, string name) =>
        element.TryGetProperty(name, out var p) && p.ValueKind != JsonValueKind.Null ? p.GetString() : null;

    public static float? GetFloat(JsonElement element, string name)
    {
        if (!element.TryGetProperty(name, out var p) || p.ValueKind == JsonValueKind.Null) return null;
        return p.ValueKind == JsonValueKind.String
            ? float.TryParse(p.GetString(), out var f) ? f : null
            : p.GetSingle();
    }
}
