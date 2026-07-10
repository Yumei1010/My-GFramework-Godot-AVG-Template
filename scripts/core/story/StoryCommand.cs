namespace GFrameworkTemplate.scripts.core.story;

/// <summary>
///     JSON 脚本命令数据基类——对应一条 JSON 指令
/// </summary>
/// <summary>
///     故事命令基类 —— JSON 指令的公共字段
/// </summary>
public abstract class StoryCommand
{
    public string Type { get; set; } = string.Empty;
    public string? Branch { get; set; }
    public bool HideLabels { get; set; }
    public float? Wait { get; set; }
    public string? FilePath { get; set; }
}
