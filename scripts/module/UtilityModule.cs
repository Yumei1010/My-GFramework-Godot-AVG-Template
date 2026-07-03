using GFramework.Core.Abstractions.architecture;
using GFramework.Game.Abstractions.data;
using GFramework.Game.architecture;
using GFramework.Game.data;
using GFramework.Game.serializer;
using GFramework.Godot.scene;
using GFramework.Godot.storage;
using GFramework.Godot.ui;
using GFrameworkTemplate.scripts.utility;
using Godot;

namespace GFrameworkTemplate.scripts.module;

/// <summary>
///     工具模块类，负责安装和管理框架中的实用工具组件
/// </summary>
public class UtilityModule : AbstractModule
{
    public override void Install(IArchitecture architecture)
    {
        architecture.RegisterUtility(new GodotUiRegistry());
        architecture.RegisterUtility(new GodotSceneRegistry());
        architecture.RegisterUtility(new GodotTextureRegistry());
        architecture.RegisterUtility(new GodotUiFactory());

        var jsonSerializer = new JsonSerializer();
        architecture.RegisterUtility(jsonSerializer);

        var storage = new GodotFileStorage(jsonSerializer);
        architecture.RegisterUtility(storage);

        architecture.RegisterUtility(new UnifiedSettingsDataRepository(storage, jsonSerializer,
            new DataRepositoryOptions
            {
                BasePath = ProjectSettings.GetSetting("application/config/save/setting_path").AsString(),
                AutoBackup = true
            }));
    }
}
