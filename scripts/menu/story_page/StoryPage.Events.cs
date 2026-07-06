using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.menu.story_page;

public partial class StoryPage
{
    private void RegisterEvent()
    {
        this.RegisterEvent<VisualNovelBackgroundChangedEvent>(e =>
        {
            OnVisualNovelBackgroundChangedEvent(e.FilePath, e.WaitTweenEnd, e.Delay);
        }).UnRegisterWhenNodeExitTree(this);
    }

    private void OnVisualNovelBackgroundChangedEvent(string filePath, bool waitTweenEnd, float delay)
    {
        Background.Change(filePath, waitTweenEnd, delay);
    }
}
