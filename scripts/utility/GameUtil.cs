using GFrameworkTemplate.global;

namespace GFrameworkTemplate.scripts.utility;

/// <summary>
///     提供游戏相关的工具方法。
/// </summary>
public static class GameUtil
{
    /// <summary>
    ///     获取游戏的根场景树。
    /// </summary>
    /// <returns>返回当前游戏的根场景树对象。</returns>
    public static SceneTree GetTree()
    {
        return GameEntryPoint.Tree;
    }
}