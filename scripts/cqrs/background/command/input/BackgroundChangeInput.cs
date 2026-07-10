using GFramework.Core.Abstractions.command;

namespace GFrameworkTemplate.scripts.cqrs.background.command.input;

public sealed class BackgroundChangeInput : ICommandInput
{
    public string FilePath { get; set; } = "";
    public bool WaitTweenEnd { get; set; }
    public float Delay { get; set; }
}
