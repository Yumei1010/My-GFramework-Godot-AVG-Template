using GFramework.Core.Abstractions.controller;
using GFramework.Game.Abstractions.enums;
using GFramework.Game.Abstractions.ui;
using GFramework.Godot.ui;
using GFrameworkTemplate.scripts.core.ui;

namespace GFrameworkTemplate.scripts.menu.story_page;

[Log]
[ContextAware]
public partial class StoryPage : Control, IController, IUiPageBehaviorProvider, ISimpleUiPage
{
    private IUiPageBehavior? _page;

    public override void _Ready()
    {
        _ = ReadyAsync();
        ConnectPageSignals();
        RegisterEvents();
    }

    public IUiPageBehavior GetPage()
    {
        _page ??= UiPageBehaviorFactory.Create<StoryPage>(this, UiKeyStr, UiLayer.Page);
        return _page;
    }
}
