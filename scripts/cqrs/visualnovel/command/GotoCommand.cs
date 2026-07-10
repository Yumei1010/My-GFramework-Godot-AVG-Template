using System.Text.Json;

using GFrameworkTemplate.scripts.core.story;
namespace GFrameworkTemplate.scripts.cqrs.visualnovel.command;

/// <summary>
///     跳转命令——跳转到另一个 JSON 脚本
/// </summary>
/// <summary>
///     GotoCommand —— JSON 跳转指令数据
/// </summary>
public sealed class GotoCommand : StoryCommand
{
    /// <summary>从 JSON 元素构造 GotoCommand</summary>
    public static GotoCommand FromJson(JsonElement element)
    {
        var cmd = new GotoCommand();
        return cmd;
    }
}
