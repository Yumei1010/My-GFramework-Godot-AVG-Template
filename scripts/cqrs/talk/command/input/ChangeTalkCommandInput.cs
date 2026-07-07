using GFramework.Core.Abstractions.command;

namespace GFrameworkTemplate.scripts.cqrs.talk.command.input;

/// <summary>
///     对话命令输入参数
/// </summary>
public sealed class ChangeTalkCommandInput : ICommandInput
{
    public string Talker { get; set; } = "";
    public string Content { get; set; } = "";
    public bool IsCenter { get; set; }
    public string AvatarPath { get; set; } = "";
}
