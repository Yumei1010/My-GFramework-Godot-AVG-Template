using GFramework.Core.Abstractions.controller;
using GFramework.Game.Abstractions.enums;
using GFramework.Game.Abstractions.ui;
using GFramework.Godot.ui;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.core.ui;
using Godot;

namespace GFrameworkTemplate.scripts.menu;

[Log]
[ContextAware]
public partial class VisualNovelTalkPage : Control, IController, IUiPageBehaviorProvider, ISimpleUiPage
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
        _page ??= UiPageBehaviorFactory.Create<VisualNovelTalkPage>(this, UiKeyStr, UiLayer.Page);
        return _page;
    }
}
