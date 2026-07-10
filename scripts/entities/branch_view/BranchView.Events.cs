using GFrameworkTemplate.scripts.component.branch_option;
using GFrameworkTemplate.scripts.cqrs.branch.@event;
using GFrameworkTemplate.scripts.cqrs.story.command;

namespace GFrameworkTemplate.scripts.entities.branch_view;

public partial class BranchView
{
    private void RegisterEvent()
    {
        this.RegisterEvent<BranchShownEvent>(e =>
        {
            OnBranchShownEvent(e.Options);
        }).UnRegisterWhenNodeExitTree(this);

        this.RegisterEvent<BranchChosenEvent>(_ =>
        {
            Hide();
        }).UnRegisterWhenNodeExitTree(this);
    }

    private void OnBranchShownEvent(Dictionary<string, BranchOption> options)
    {
        ClearOptions();
        foreach (var (optionId, option) in options)
        {
            var node = _optionScene.Instantiate();
            node.GetNode<RichTextLabel>("%BranchContentLabel").Text = $"[center]{option.Text}[/center]";
            var capturedId = optionId;
            node.GetNode<Button>("%BranchOptionButton").Pressed += () =>
            {
                this.SendCommand(new StoryChooseBranchCommand { OptionId = capturedId });
                ClearOptions();
                Hide();
            };
            ButtonList.AddChild(node);
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
