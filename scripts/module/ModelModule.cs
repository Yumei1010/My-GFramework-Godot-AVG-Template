using GFramework.Core.Abstractions.architecture;
using GFramework.Core.functional.pipe;
using GFramework.Game.Abstractions.data;
using GFramework.Game.architecture;
using GFramework.Game.setting;
using GFramework.Godot.setting;
using GFramework.Godot.setting.data;
using GFrameworkTemplate.scripts.data.setting;
using GFrameworkTemplate.scripts.model.background;
using GFrameworkTemplate.scripts.model.camera;
using GFrameworkTemplate.scripts.model.sound;
using GFrameworkTemplate.scripts.model.tachie;
using GFrameworkTemplate.scripts.model.talk;
using GFrameworkTemplate.scripts.model.visualnovel;

namespace GFrameworkTemplate.scripts.module;

/// <summary>
/// ModelModule 类继承自 AbstractModule，用于在架构中安装和注册模型。
/// </summary>
public class ModelModule : AbstractModule
{
    public override void Install(IArchitecture architecture)
    {
        var settingsDataRepository = architecture.Context.GetUtility<ISettingsDataRepository>()!;

        // 注册设置模型，并配置其应用器
        architecture.RegisterModel(
            new SettingsModel<ISettingsDataRepository>(new SettingDataLocationProvider(), settingsDataRepository)
                .Also(it =>
                {
                    it.RegisterApplicator(new GodotAudioSettings(it, new AudioBusMap()));
                    it.RegisterApplicator(new GodotGraphicsSettings(it));
                    it.RegisterApplicator(new GodotLocalizationSettings(it, new LocalizationMap()));
                })
        );

        architecture.RegisterModel(new StoryStateModel());
        architecture.RegisterModel(new CameraModel());
        architecture.RegisterModel(new TalkModel());
        architecture.RegisterModel(new TachieModel());
        architecture.RegisterModel(new BackgroundModel());
        architecture.RegisterModel(new SoundModel());
    }
}
