using GFramework.Core.Abstractions.command;

namespace GFrameworkTemplate.scripts.cqrs.setting.command.input;

/// <summary>
///     更改语言命令输入类
///     用于封装更改语言的命令输入参数
/// </summary>
public sealed class SettingChangeLanguageInput : ICommandInput
{
    /// <summary>
    ///     获取或设置要更改的新语言
    /// </summary>
    public string Language { get; set; } = null!;
}