using GFrameworkTemplate.scripts.system.story_engine_system;

namespace GFrameworkTemplate.scripts.cqrs.player.command;
public sealed class PlayerClickCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<StoryEngineSystem>().Advance();
    }
}