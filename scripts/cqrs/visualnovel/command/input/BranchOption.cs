namespace GFrameworkTemplate.scripts.cqrs.visualnovel.command.input;

/// <summary>
///     分支选项
/// </summary>
public sealed class BranchOption
{
    public string Text { get; set; } = string.Empty;
    public float? Wait { get; set; }
}
