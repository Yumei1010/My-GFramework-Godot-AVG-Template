using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.global;
using GFrameworkTemplate.scripts.system.background;
using GFrameworkTemplate.scripts.system.branch;
using GFrameworkTemplate.scripts.system.camera;
using GFrameworkTemplate.scripts.system.sound;
using GFrameworkTemplate.scripts.system.tachie;
using GFrameworkTemplate.scripts.system.talk;
using Godot;

namespace GFrameworkTemplate.scripts.component.vn_game;

/// <summary>
///     VN 游戏场景控制器——加载时将 Manager 注册到架构 DI
/// </summary>
[Log]
[ContextAware]
public partial class VnGameSceneController : Node
{
    public override void _Ready()
    {
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<TalkSystem>("TalkSystem")); } catch { }
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<TachieSystem>("TachieSystem")); } catch { }
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<BranchSystem>("BranchSystem")); } catch { }
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<BackgroundSystem>("BackgroundSystem")); } catch { }
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<CameraSystem>("CameraSystem")); } catch { }
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<SoundSystem>("SoundSystem")); } catch { }
        _log.Debug("VnGameScene 就绪——Manager 已注册");
    }
}
