using GFramework.Core.Abstractions.command;

namespace GFrameworkTemplate.scripts.cqrs.talk.command.input;

public sealed class TalkPlayInput : ICommandInput
{
    public string Talker { get; set; } = "";
    public string Content { get; set; } = "";
    public bool IsCenter { get; set; }
    public float RevealSpeed { get; set; } = 0.04f;
}
