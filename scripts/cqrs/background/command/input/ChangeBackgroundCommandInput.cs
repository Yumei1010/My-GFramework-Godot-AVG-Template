using GFramework.Core.Abstractions.command;

namespace GFrameworkTemplate.scripts.cqrs.background.command.input;

/// <summary>
///     背景切换命令输入参数
/// </summary>
public sealed class ChangeBackgroundCommandInput : ICommandInput
{
    /// <summary>
    ///     背景图片的逻辑名
    /// </summary>
    public string FilePath { get; set; } = "";

    /// <summary>
    ///     是否等待淡入淡出动画结束
    /// </summary>
    public bool WaitTweenEnd { get; set; }

    /// <summary>
    ///     延迟秒数
    /// </summary>
    public float Delay { get; set; }
}
