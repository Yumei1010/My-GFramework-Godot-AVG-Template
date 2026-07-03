using GFrameworkTemplate.scripts.component.branch_option;
using GFrameworkTemplate.scripts.cqrs.story.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;

namespace GFrameworkTemplate.scripts.component.branch_controller;

/// <summary>
///     分支栏控制器——CanvasLayer 场景节点，动态实例化分支按钮
/// </summary>
[Log]
[ContextAware]
public partial class BranchController : CanvasLayer
{
    private VBoxContainer _buttonList = null!;
    private PackedScene _optionScene = null!;
    private readonly List<Node> _activeOptions = new();

    public override void _Ready()
    {
        _optionScene = GD.Load<PackedScene>("res://scenes/component/branch_option/branch_option.tscn");
        _buttonList = GetNode<VBoxContainer>("%ButtonList");
        Hide();
        this.RegisterEvent<VisualNovelBranchShownEvent>(OnBranch).UnRegisterWhenNodeExitTree(this);
    }


    private void OnBranch(VisualNovelBranchShownEvent e)
    {
        ClearOptions();

        foreach (var (optionId, option) in e.Options)
        {
            var node = _optionScene.Instantiate();
            node.GetNode<RichTextLabel>("%BranchContentLabel").Text = $"[center]{option.Text}[/center]";
            var capturedId = optionId;
            node.GetNode<Button>("%BranchOptionButton").Pressed += () =>
                this.SendCommand(new ChooseBranchCommand { OptionId = capturedId });
            _buttonList.AddChild(node);
            _activeOptions.Add(node);
        }

        Show();
    }

    private void ClearOptions()
    {
        foreach (var node in _activeOptions) node.QueueFree();
        _activeOptions.Clear();
    }
}
