using System.Text.Json;
using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.component.tachie_slot;
using GFrameworkTemplate.scripts.enums.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.visualnovel.command;

/// <summary>
///     立绘命令——管理角色立绘的显示/切换/隐藏
/// </summary>
public sealed class TachieCommand : StoryCommand
{
    public Dictionary<string, TachieSlot> Tachies { get; set; } = new();

    public static TachieCommand FromJson(JsonElement element)
    {
        var tachies = new Dictionary<string, TachieSlot>();
        if (element.TryGetProperty("tachies", out var t))
        {
            foreach (var entry in t.EnumerateObject())
            {
                tachies[entry.Name] = new TachieSlot
                {
                    FilePath = StoryParser.GetString(entry.Value, "file_path") ?? string.Empty,
                    Type = StoryParser.GetString(entry.Value, "type") switch
                    {
                        "change" => TachieOperation.Change,
                        "close" => TachieOperation.Close,
                        "onlyShow" => TachieOperation.OnlyShow,
                        _ => TachieOperation.Show
                    },
                    Slot = StoryParser.GetString(entry.Value, "slot")
                };
            }
        }

        var cmd = new TachieCommand { Tachies = tachies };
        return cmd;
    }
}
