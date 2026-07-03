namespace GFrameworkTemplate.scripts.cqrs.tachie.query.result;

public sealed class TachieStateResult
{
    public required Dictionary<string, string> Chars { get; init; }
    public required Dictionary<string, string> SlotToChar { get; init; }
    public string? SpotlightChar { get; init; }
}
