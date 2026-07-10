namespace GFrameworkTemplate.scripts.entities.branch_view;

[Log]
[ContextAware]
/// <summary>
///     分支 View —— 动态创建分支选项按钮
/// </summary>
public partial class BranchView : CanvasLayer
{
    public override void _Ready()
    {
        _ = ReadyAsync();
        RegisterEvent();
    }
}
