using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.global;
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
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<TalkManager>("TalkManager")); } catch { }
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<TachieManager>("TachieManager")); } catch { }
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<BranchManager>("BranchManager")); } catch { }
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<BackgroundManager>("BackgroundManager")); } catch { }
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<CameraManager>("CameraManager")); } catch { }
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<SoundManager>("SoundManager")); } catch { }
        _log.Debug("VnGameScene 就绪——Manager 已注册");
    }
}
