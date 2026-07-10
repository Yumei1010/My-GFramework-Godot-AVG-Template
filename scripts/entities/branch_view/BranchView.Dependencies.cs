using GFrameworkTemplate.global;

namespace GFrameworkTemplate.scripts.entities.branch_view;

public partial class BranchView
{
    private VBoxContainer ButtonList => GetNode<VBoxContainer>("%ButtonList");
    private PackedScene _optionScene = null!;
    private readonly List<Node> _activeOptions = new();

    private async Task ReadyAsync()
    {
        await GameEntryPoint.Architecture.WaitUntilReadyAsync().ConfigureAwait(false);

        Layer = 2;
        _optionScene = GD.Load<PackedScene>("res://scenes/component/branch_option/branch_option.tscn");
        Hide();
    }
}
