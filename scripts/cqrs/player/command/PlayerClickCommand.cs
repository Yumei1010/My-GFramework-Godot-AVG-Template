using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.player.command;
public sealed class PlayerClickCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<StoryEngineSystem>().Advance();
    }
}