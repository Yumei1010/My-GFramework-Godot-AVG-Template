using GFramework.Core.model;

namespace GFrameworkTemplate.scripts.model.tachie;

/// <summary>
///     立绘模型 —— 角色到文件路径的映射
/// </summary>
public class TachieModel : AbstractModel
{
    /// <summary>角色名 → 文件路径</summary>
    public Dictionary<string, string> Chars { get; set; } = new();
    /// <summary>槽位名 → 角色名</summary>
    public Dictionary<string, string> SlotToChar { get; set; } = new();
    /// <summary>聚光灯下的角色名</summary>
    public string SpotlightChar { get; set; } = string.Empty;
    protected override void OnInit()
    {
        
    }
}
