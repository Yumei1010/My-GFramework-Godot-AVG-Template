using GFramework.Core.extensions;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.component.branch_option;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.system.visualnovel;
using Godot;

namespace GFrameworkTemplate.scripts.component.branch_bar;

/// <summary>
///     分支栏控制器——响应 VN 分支事件，显示选项按钮并提交选择
/// </summary>
[Log]
[ContextAware]
public partial class BranchBarController : CanvasLayer
{
    private Control BranchOption1 => GetNode<Control>("BranchBarContainer/VBoxContainer/BranchOption");
    private Control BranchOption2 => GetNode<Control>("BranchBarContainer/VBoxContainer/BranchOption2");
    private Control BranchOption3 => GetNode<Control>("BranchBarContainer/VBoxContainer/BranchOption3");

    private RichTextLabel ContentLabel1 => GetNode<RichTextLabel>("%BranchContentLabel");
    private RichTextLabel ContentLabel2 => GetNode<RichTextLabel>("BranchBarContainer/VBoxContainer/BranchOption2/BranchContentLabel");
    private RichTextLabel ContentLabel3 => GetNode<RichTextLabel>("BranchBarContainer/VBoxContainer/BranchOption3/BranchContentLabel");

    private Button Button1 => GetNode<Button>("%BranchOptionButton");
    private Button Button2 => GetNode<Button>("BranchBarContainer/VBoxContainer/BranchOption2/BranchOptionButton");
    private Button Button3 => GetNode<Button>("BranchBarContainer/VBoxContainer/BranchOption3/BranchOptionButton");

    private StoryEngineSystem _engine = null!;
    private string? _id1, _id2, _id3;
    private bool _bound;

    public override void _Ready()
    {
        _engine = this.GetUtility<StoryEngineSystem>()!;
        Visible = false;

        this.RegisterEvent<VisualNovelBranchTriggeredEvent>(OnBranch).UnRegisterWhenNodeExitTree(this);
    }

    private void OnBranch(VisualNovelBranchTriggeredEvent e)
    {
        Unbind();

        var ids = e.Options.Keys.ToArray();
        var slots = new[] { (BranchOption1, ContentLabel1, Button1, ids.Length > 0 ? ids[0] : null),
                            (BranchOption2, ContentLabel2, Button2, ids.Length > 1 ? ids[1] : null),
                            (BranchOption3, ContentLabel3, Button3, ids.Length > 2 ? ids[2] : null) };

        foreach (var (ctrl, label, btn, id) in slots)
        {
            if (id == null)
            {
                ctrl.Visible = false;
                continue;
            }

            ctrl.Visible = true;
            label.Text = $"[center]{e.Options[id].Text}[/center]";
            btn.Pressed += OnOptionPressed;
            _bound = true;
        }

        (_id1, _id2, _id3) = (slots[0].Item4, slots[1].Item4, slots[2].Item4);
        Visible = true;
    }

    private void OnOptionPressed()
    {
        // 找到被按下的按钮对应的选项 ID
        if (Button1.ButtonPressed) { Choose(_id1); return; }
        if (Button2.ButtonPressed) { Choose(_id2); return; }
        if (Button3.ButtonPressed) { Choose(_id3); return; }
    }

    private void Choose(string? id)
    {
        if (id == null) return;
        Unbind();
        Visible = false;
        _engine.ChooseBranch(id);
    }

    private void Unbind()
    {
        if (!_bound) return;
        Button1.Pressed -= OnOptionPressed;
        Button2.Pressed -= OnOptionPressed;
        Button3.Pressed -= OnOptionPressed;
        _bound = false;
    }
}
