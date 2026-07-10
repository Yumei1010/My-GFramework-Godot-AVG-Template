using GFramework.Core.Abstractions.command;

namespace GFrameworkTemplate.scripts.cqrs.graphics.input;

/// <summary>
///     切换全屏命令输入类
/// </summary>
public sealed class GraphicsToggleFullscreenInput : ICommandInput
{
    /// <summary>
    ///     全屏状态属性
    /// </summary>
    public bool Fullscreen { get; set; }
}