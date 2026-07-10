using System.Text.Json;
using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.component.branch_option;

namespace GFrameworkTemplate.scripts.cqrs.visualnovel.command;

/// <summary>
///     分支命令——显示选项并等待玩家选择
/// </summary>
public sealed class BranchCommand : StoryCommand
{
    public Dictionary<string, BranchOption> Options { get; set; } = new();

    public static BranchCommand FromJson(JsonElement element)
    {
        var options = new Dictionary<string, BranchOption>();
        if (element.TryGetProperty("options", out var opts))
        {
            foreach (var entry in opts.EnumerateObject())
            {
                options[entry.Name] = new BranchOption
                {
                    Text = StoryParser.GetString(entry.Value, "text") ?? string.Empty,
                    Wait = StoryParser.GetFloat(entry.Value, "wait")
                };
            }
        }

        var cmd = new BranchCommand { Options = options };
        return cmd;
    }
}
