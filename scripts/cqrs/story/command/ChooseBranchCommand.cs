using GFramework.Core.Abstractions.command;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

/// <summary>
///     选择分支选项
/// </summary>
public sealed class ChooseBranchCommand : AbstractCommand
{
    public required string OptionId { get; set; }

    protected override void OnExecute()
    {
        this.GetSystem<StoryEngineSystem>().ChooseBranch(OptionId);
    }
}
