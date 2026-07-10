namespace GFrameworkTemplate.scripts.entities.branch_view;

[Log]
[ContextAware]
public partial class BranchView : CanvasLayer
{
    public override void _Ready()
    {
        _ = ReadyAsync();
        RegisterEvent();
    }
}
