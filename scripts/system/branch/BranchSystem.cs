using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.system.branch;

/// <summary>
///     分支栏——动态实例化 branch_option.tscn，通过 ISystem 注册到 GF 框架
/// </summary>
[Log]
[ContextAware]
public partial class BranchSystem : CanvasLayer
{
    public static BranchSystem? Instance { get; private set; }
    private VBoxContainer _buttonList = null!;
    private StoryEngineSystem _engine = null!;
    private PackedScene _optionScene = null!;
    private readonly List<Node> _activeOptions = new();


    public override void _Ready()
    {
        _engine = this.GetSystem<StoryEngineSystem>()!;
        _optionScene = GD.Load<PackedScene>("res://scenes/component/branch_option/branch_option.tscn");

        _buttonList = GetNode<VBoxContainer>("%ButtonList");
        Hide();
        this.RegisterEvent<VisualNovelBranchTriggeredEvent>(OnBranch).UnRegisterWhenNodeExitTree(this);
    }

    private void OnBranch(VisualNovelBranchTriggeredEvent e)
    {
        ClearOptions();

        foreach (var (optionId, option) in e.Options)
        {
            var node = _optionScene.Instantiate();
            var label = node.GetNode<RichTextLabel>("%BranchContentLabel");
            var button = node.GetNode<Button>("%BranchOptionButton");

            label.Text = $"[center]{option.Text}[/center]";

            var capturedId = optionId;
            button.Pressed += () => Choose(capturedId);

            _buttonList.AddChild(node);
            _activeOptions.Add(node);
        }

        Show();
    }

    private void Choose(string optionId)
    {
        ClearOptions();
        Hide();
        _engine.ChooseBranch(optionId);
    }

    private void ClearOptions()
    {
        foreach (var node in _activeOptions)
            node.QueueFree();
        _activeOptions.Clear();
    }
}
