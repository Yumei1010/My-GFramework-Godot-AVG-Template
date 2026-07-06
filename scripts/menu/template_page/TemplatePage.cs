using GFramework.Core.Abstractions.controller;
using GFramework.Game.Abstractions.enums;
using GFramework.Game.Abstractions.ui;
using GFramework.Godot.ui;
using GFrameworkTemplate.scripts.core.ui;

namespace GFrameworkTemplate.scripts.menu.template_page;

/// <summary>
///     模板页面——演示 UI 页面的 partial class 五文件模式
///     使用者可参考此类创建新的 UI 页面
/// </summary>
[Log]
[ContextAware]
public partial class TemplatePage : Control, IController, IUiPageBehaviorProvider, ISimpleUiPage
{
    private IUiPageBehavior? _page;

    /// <summary>
    ///     Godot 节点就绪回调，按顺序执行：异步初始化 → 信号绑定 → 事件注册
    /// </summary>
    public override void _Ready()
    {
        _ = ReadyAsync();
        ConnectPageSignals();
        RegisterEvents();
    }

    /// <summary>
    ///     获取当前页面的 UI 行为实例
    /// </summary>
    public IUiPageBehavior GetPage()
    {
        _page ??= UiPageBehaviorFactory.Create<TemplatePage>(this, UiKeyStr, UiLayer.Page);
        return _page;
    }
}
