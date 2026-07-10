using GFramework.Game.Abstractions.setting.data;

namespace GFrameworkTemplate.scripts.cqrs.setting.query.result;

/// <summary>
///     表示游戏设置视图的数据模型，包含音频和显示设置信息
/// </summary>
public sealed class SettingsView
{
    /// <summary>
    ///     音频设置配置对象，包含所有音频相关的设置选项
    /// </summary>
    public AudioSettings Audio { get; set; } = new();

    /// <summary>
    ///     图形设置配置对象，包含所有图形渲染相关的设置选项
    /// </summary>
    public GraphicsSettings Graphics { get; set; } = new();

    /// <summary>
    ///     本地化设置配置对象，包含所有语言和区域相关的设置选项
    /// </summary>
    public LocalizationSettings Localization { get; set; } = new();
}
