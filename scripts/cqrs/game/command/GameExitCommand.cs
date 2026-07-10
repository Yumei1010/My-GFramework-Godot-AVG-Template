using GFramework.Core.command;
using GFrameworkTemplate.scripts.utility;

namespace GFrameworkTemplate.scripts.cqrs.game.command;

/// <summary>
///     退出游戏命令类，用于处理游戏退出逻辑
/// </summary>
public sealed class GameExitCommand : AbstractCommand
{
    /// <summary>
    ///     执行退出游戏命令的具体逻辑
    /// </summary>
    protected override void OnExecute()
    {
        GameUtil.GetTree().Quit();
    }
}