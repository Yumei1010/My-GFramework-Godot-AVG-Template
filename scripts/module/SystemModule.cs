using GFramework.Core.Abstractions.architecture;
using GFramework.Game.architecture;
using GFramework.Game.setting;
using GFrameworkTemplate.scripts.core.scene;
using GFrameworkTemplate.scripts.core.ui;
using GFrameworkTemplate.scripts.system.save;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.module;

/// <summary>
///     系统模块类，负责安装和注册框架所需的各种系统组件
/// </summary>
public class SystemModule : AbstractModule
{
    public override void Install(IArchitecture architecture)
    {
        architecture.RegisterSystem(new UiRouter());
        architecture.RegisterSystem(new SceneRouter());
        architecture.RegisterSystem(new SettingsSystem());
        architecture.RegisterSystem(new StoryEngineSystem());
        architecture.RegisterSystem(new SaveManager());
    }
}
