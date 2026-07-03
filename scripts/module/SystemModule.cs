using GFramework.Core.Abstractions.architecture;
using GFramework.Game.architecture;
using GFramework.Game.setting;
using GFrameworkTemplate.scripts.core.scene;
using GFrameworkTemplate.scripts.core.ui;
using GFrameworkTemplate.scripts.system.visualnovel;
using GFrameworkTemplate.scripts.system.visualnovel;
using GFrameworkTemplate.scripts.system.visualnovel;
using GFrameworkTemplate.scripts.system.visualnovel;
using GFrameworkTemplate.scripts.system.visualnovel;
using GFrameworkTemplate.scripts.system.visualnovel;
using GFrameworkTemplate.scripts.system.visualnovel;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.module;

public class SystemModule : AbstractModule
{
    public override void Install(IArchitecture architecture)
    {
        architecture.RegisterSystem(new UiRouter());
        architecture.RegisterSystem(new SceneRouter());
        architecture.RegisterSystem(new SettingsSystem());
        architecture.RegisterSystem(new BackgroundSystem());
        architecture.RegisterSystem(new BranchSystem());
        architecture.RegisterSystem(new CameraSystem());
        architecture.RegisterSystem(new SaveSystem());
        architecture.RegisterSystem(new SoundSystem());
        architecture.RegisterSystem(new TachieSystem());
        architecture.RegisterSystem(new TalkSystem());
        architecture.RegisterSystem(new StoryEngineSystem());
        architecture.RegisterSystem(new GotoSystem());
        architecture.RegisterSystem(new EventSystem());
    }
}
