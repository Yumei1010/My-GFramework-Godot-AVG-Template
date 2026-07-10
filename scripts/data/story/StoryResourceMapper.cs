namespace GFrameworkTemplate.scripts.data.story;

/// <summary>
///     故事资源映射器——管理 JSON 脚本逻辑名到文件路径的映射
/// </summary>
/// <summary>
///     故事资源映射器 —— JSON 路径映射与加载
/// </summary>
public static class StoryResourceMapper
{
    private static readonly Dictionary<string, string> JsonPathMap = new(StringComparer.Ordinal);

    public static void RegisterJson(string logicalName, string jsonPath) =>
        JsonPathMap[logicalName] = jsonPath;

    public static string? ResolveJsonPath(string logicalName) =>
        JsonPathMap.GetValueOrDefault(logicalName);

    /// <summary>异步加载 JSON 文件内容</summary>
    public static async Task<string?> LoadJsonAsync(string jsonPath)
    {
        try
        {
            using var file = FileAccess.Open(jsonPath, FileAccess.ModeFlags.Read);
            if (file == null) return null;
            return await Task.Run(() => file.GetAsText()).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            GD.PrintErr($"StoryResourceMapper: failed to load JSON {jsonPath}: {ex.Message}");
            return null;
        }
    }

    public static void Clear() => JsonPathMap.Clear();
}
