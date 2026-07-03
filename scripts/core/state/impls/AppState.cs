using GFramework.Core.Abstractions.state;
using GFramework.Core.state;
using GFramework.Game.Abstractions.scene;
using GFramework.Game.Abstractions.ui;

namespace GFrameworkTemplate.scripts.core.state.impls;

/// <summary>
///     应用默认状态，清除 UI 和场景路由，为推送新页面做准备
/// </summary>
public class AppState : ContextAwareStateBase
{
    public override void OnEnter(IState? from)
    {
        var uiRouter = this.GetSystem<IUiRouter>()!;
        uiRouter.Clear();
        this.GetSystem<ISceneRouter>()!.Unload();
    }

    public override bool CanTransitionTo(IState target) => true;
}
