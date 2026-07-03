using GFramework.Core.Abstractions.architecture;
using GFramework.Game.architecture;
using GFramework.Game.setting;
using GFrameworkTemplate.scripts.core.scene;
using GFrameworkTemplate.scripts.core.ui;
using GFrameworkTemplate.scripts.system.save;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.module;

/// <summary>
///     系统模块类——注册纯逻辑 ISystem（无 Godot 节点依赖）
///     UI Manager（Talk/Tachie/Branch/Background/Camera/Sound）作为场景子节点，不在此注册
/// </summary>
public class SystemModule : AbstractModule
{
    public override void Install(IArchitecture architecture)
    {
        architecture.RegisterSystem(new UiRouter());
        architecture.RegisterSystem(new SceneRouter());
        architecture.RegisterSystem(new SettingsSystem());
        architecture.RegisterSystem(new SaveSystem());
        architecture.RegisterSystem(new StoryEngineSystem());
    }
}
