using GFramework.Core.extensions;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.component.branch_option;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.system.visualnovel;
using Godot;

namespace GFrameworkTemplate.scripts.component.vn_test;

/// <summary>
///     VN 引擎测试控制器——最小可运行场景，验证 JSON→解析→事件→UI 完整流水线
///     点击左键开始/推进，分支选项通过按钮选择
/// </summary>
[Log]
[ContextAware]
public partial class VnTestController : Node
{
    private Label TalkerName => GetNode<Label>("%TalkerName");
    private Label TalkContent => GetNode<Label>("%TalkContent");
    private Label CenterContent => GetNode<Label>("%CenterContent");
    private Label StatusLabel => GetNode<Label>("%StatusLabel");
    private VBoxContainer BranchContainer => GetNode<VBoxContainer>("%BranchContainer");
    private Button BranchBtn1 => GetNode<Button>("%BranchBtn1");
    private Button BranchBtn2 => GetNode<Button>("%BranchBtn2");
    private Button BranchBtn3 => GetNode<Button>("%BranchBtn3");
    private Button BranchBtn4 => GetNode<Button>("%BranchBtn4");

    private StoryEngineSystem _engine = null!;
    private Button[] _branchBtns = null!;
    private bool _choosingBranch;

    public override void _Ready()
    {
        _engine = this.GetUtility<StoryEngineSystem>()!;
        _branchBtns = new[] { BranchBtn1, BranchBtn2, BranchBtn3, BranchBtn4 };
        BranchContainer.Visible = false;

        RegisterEvents();

        StoryEngineSystem.RegisterJson("FirstDay", "res://resource/story/example/First.json");
        StoryEngineSystem.RegisterJson("Chapter1_Lesson", "res://resource/story/example/second.json");
        StatusLabel.Text = "点击屏幕开始测试...";
        _log.Debug("VnTestController 就绪");
    }

    private void RegisterEvents()
    {
        this.RegisterEvent<VisualNovelTalkTriggeredEvent>(e =>
        {
            if (e.IsCenter)
            {
                CenterContent.Text = e.Content;
                CenterContent.Visible = true;
                TalkContent.Visible = false;
                TalkerName.Visible = false;
            }
            else
            {
                TalkerName.Text = e.Talker ?? "";
                TalkContent.Text = e.Content;
                TalkContent.Visible = true;
                TalkerName.Visible = true;
                CenterContent.Visible = false;
            }
            StatusLabel.Text = $"播放中... {_engine.CurrentJsonPath}";
        }).UnRegisterWhenNodeExitTree(this);

        this.RegisterEvent<VisualNovelStoryFinishedEvent>(_ =>
        {
            StatusLabel.Text = "故事播放完毕。";
            _log.Debug("测试脚本播放完成");
        }).UnRegisterWhenNodeExitTree(this);

        this.RegisterEvent<VisualNovelBranchTriggeredEvent>(e =>
        {
            ShowBranchButtons(e.Options);
        }).UnRegisterWhenNodeExitTree(this);

        this.RegisterEvent<VisualNovelBackgroundTriggeredEvent>(e =>
        {
            StatusLabel.Text = $"背景切换: {e.FilePath}";
        }).UnRegisterWhenNodeExitTree(this);
    }

    private string? _currentBranchId1, _currentBranchId2, _currentBranchId3, _currentBranchId4;
    private bool _branchButtonsBound;

    private void ShowBranchButtons(Dictionary<string, BranchOption> options)
    {
        _choosingBranch = true;

        if (_branchButtonsBound)
            UnbindBranchButtons();
        BranchContainer.Visible = true;

        var ids = options.Keys.ToArray();
        for (var i = 0; i < _branchBtns.Length; i++)
        {
            if (i < ids.Length)
            {
                _branchBtns[i].Text = options[ids[i]].Text;
                _branchBtns[i].Visible = true;
            }
            else
            {
                _branchBtns[i].Visible = false;
            }
        }

        // 存储当前选项 ID
        _currentBranchId1 = ids.Length > 0 ? ids[0] : null;
        _currentBranchId2 = ids.Length > 1 ? ids[1] : null;
        _currentBranchId3 = ids.Length > 2 ? ids[2] : null;
        _currentBranchId4 = ids.Length > 3 ? ids[3] : null;

        // 绑定按钮事件
        BranchBtn1.Pressed += OnBranchBtn1Pressed;
        BranchBtn2.Pressed += OnBranchBtn2Pressed;
        BranchBtn3.Pressed += OnBranchBtn3Pressed;
        BranchBtn4.Pressed += OnBranchBtn4Pressed;
        _branchButtonsBound = true;

        StatusLabel.Text = $"请选择分支 ({ids.Length} 个选项)";
    }

    private void UnbindBranchButtons()
    {
        BranchBtn1.Pressed -= OnBranchBtn1Pressed;
        BranchBtn2.Pressed -= OnBranchBtn2Pressed;
        BranchBtn3.Pressed -= OnBranchBtn3Pressed;
        BranchBtn4.Pressed -= OnBranchBtn4Pressed;
        _branchButtonsBound = false;
    }

    private void ChooseAndHide(string? optionId)
    {
        if (optionId == null) return;
        UnbindBranchButtons();
        BranchContainer.Visible = false;
        _choosingBranch = false;
        _engine.ChooseBranch(optionId);
    }

    private void OnBranchBtn1Pressed() => ChooseAndHide(_currentBranchId1);
    private void OnBranchBtn2Pressed() => ChooseAndHide(_currentBranchId2);
    private void OnBranchBtn3Pressed() => ChooseAndHide(_currentBranchId3);
    private void OnBranchBtn4Pressed() => ChooseAndHide(_currentBranchId4);

    public override void _Input(InputEvent @event)
    {
        if (_choosingBranch)
            return;

        if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
        {
            if (!_engine.IsPlaying)
            {
                _ = _engine.LoadAndPlay("FirstDay");
                StatusLabel.Text = "播放中...";
            }
            else
            {
                _engine.Advance();
            }
        }
    }
}
