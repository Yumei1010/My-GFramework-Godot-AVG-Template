using GFramework.Core.Abstractions.architecture;
using GFramework.Core.Abstractions.logging;
using GFramework.Core.Abstractions.properties;
using GFramework.Core.Abstractions.state;
using GFramework.Core.architecture;
using GFramework.Core.extensions;
using GFramework.Game.Abstractions.setting;
using GFramework.Game.setting.events;
using GFramework.Godot.coroutine;
using GFramework.Godot.logging;
using GFramework.Godot.scene;
using GFramework.Godot.ui;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.core;
using GFrameworkTemplate.scripts.core.environment;
using GFrameworkTemplate.scripts.core.resource;
using GFrameworkTemplate.scripts.core.state.impls;
using GFrameworkTemplate.scripts.enums.scene;
using GFrameworkTemplate.scripts.utility;
using GFrameworkTemplate.scripts.cqrs.setting.command;
using Godot;
using Godot.Collections;

namespace GFrameworkTemplate.global;

/// <summary>
///     游戏入口点节点类，负责初始化游戏架构和管理全局游戏状态
/// </summary>
[Log]
[ContextAware]
public partial class GameEntryPoint : Node
{
    [Export] public bool IsDev { get; set; } = true;

    [Export] public Array<UiPageConfig> UiPageConfigs { get; set; } = null!;

    [Export] public Array<SceneConfig> GameSceneConfigs { get; set; } = null!;

    [Export] public Array<TextureConfig> TextureConfigs { get; set; } = null!;

    private IGodotSceneRegistry _sceneRegistry = null!;
    private ISettingsModel _settingsModel = null!;
    private ISettingsSystem _settingsSystem = null!;
    private IGodotTextureRegistry _textureRegistry = null!;
    private IGodotUiRegistry _uiRegistry = null!;
    public static IArchitecture Architecture { get; private set; } = null!;
    public static SceneTree Tree { get; private set; } = null!;

    public override void _Ready()
    {
        Tree = GetTree();

        var arch = new GameArchitecture(new ArchitectureConfiguration
        {
            LoggerProperties = new LoggerProperties
            {
                LoggerFactoryProvider = new GodotLoggerFactoryProvider
                {
                    MinLevel = LogLevel.Debug
                }
            }
        }, IsDev ? new GameDevEnvironment() : new GameMainEnvironment());
        Architecture = arch;
        arch.Initialize();
        try { GameContext.Bind(typeof(GameArchitecture), arch.Context); }
        catch (InvalidOperationException) { /* 上下文已绑定 */ }
        _settingsModel = this.GetModel<ISettingsModel>()!;
        _ = _settingsModel.InitializeAsync();

        this.RegisterEvent<SettingsInitializedEvent>(e =>
        {
            _settingsSystem = this.GetSystem<ISettingsSystem>()!;
            _ = _settingsSystem.ApplyAll();
            _log.Info("设置已加载");
        });

        _sceneRegistry = this.GetUtility<IGodotSceneRegistry>()!;
        _uiRegistry = this.GetUtility<IGodotUiRegistry>()!;
        _textureRegistry = this.GetUtility<IGodotTextureRegistry>()!;

        foreach (var gameSceneConfig in GameSceneConfigs) _sceneRegistry.Registry(gameSceneConfig);
        foreach (var uiPageConfig in UiPageConfigs) _uiRegistry.Registry(uiPageConfig);
        foreach (var textureConfig in TextureConfigs) _textureRegistry.Registry(textureConfig);

        if (ShouldEnterAppState())
            this.RegisterEvent<UiRoot.UiRootReadyEvent>(_ =>
            {
                this.GetSystem<IStateMachineSystem>()!.ChangeTo<AppState>();
            });

        _log.Debug("框架入口点就绪.");
        CallDeferred(nameof(CallDeferredInit));
    }

    private static void CallDeferredInit()
    {
        Timing.Prewarm();
    }

    private bool ShouldEnterAppState()
    {
        var tree = GetTree();
        var currentScene = tree.CurrentScene;
        if (currentScene == null)
            return false;
        var scenePath = currentScene.SceneFilePath;
        return string.Equals(scenePath, _sceneRegistry.Get(nameof(SceneKey.Main)).GetPath(),
            StringComparison.Ordinal);
    }

    public override void _ExitTree()
    {
        _ = this.SendCommandAsync(new SaveSettingsCommand());
    }
}
