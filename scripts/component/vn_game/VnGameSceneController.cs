namespace GFrameworkTemplate.scripts.component.vn_game;

/// <summary>
///     VN 游戏场景——Manager 作为场景子节点，通过 static Instance 访问
/// </summary>
[Log]
[ContextAware]
public partial class VnGameSceneController : Node
{
    public override void _Ready()
    {
        _log.Debug("VnGameScene 就绪");
    }
}
