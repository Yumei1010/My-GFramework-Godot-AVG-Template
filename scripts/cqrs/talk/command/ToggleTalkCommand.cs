using GFramework.Core.Abstractions.command;
using GFrameworkTemplate.scripts.system.talk;

namespace GFrameworkTemplate.scripts.cqrs.talk.command;

/// <summary>
///     切换对话框显隐
/// </summary>
public sealed class ToggleTalkCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<TalkSystem>().Toggle();
    }
}
