using System.Text.Json;
using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command.input;

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
                    Text = GetString(entry.Value, "text") ?? string.Empty,
                    Wait = GetFloat(entry.Value, "wait")
                };
            }
        }

        var cmd = new BranchCommand { Options = options };
        cmd.FillCommon(element);
        return cmd;
    }
}
