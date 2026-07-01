using GFramework.Core.extensions;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.system.visualnovel;
using Godot;

namespace GFrameworkTemplate.scripts.component.vn_test;

/// <summary>
///     VN 引擎测试控制器——最小可运行场景，验证 JSON→解析→事件→UI 完整流水线
/// </summary>
[Log]
[ContextAware]
public partial class VnTestController : Node
{
    private Label TalkerName => GetNode<Label>("%TalkerName");
    private Label TalkContent => GetNode<Label>("%TalkContent");
    private Label CenterContent => GetNode<Label>("%CenterContent");
    private Label StatusLabel => GetNode<Label>("%StatusLabel");

    private StoryEngineSystem _engine = null!;

    public override void _Ready()
    {
        _engine = this.GetUtility<StoryEngineSystem>()!;

        RegisterEvents();

        // 注册示例脚本路径
        StoryEngineSystem.RegisterJson("FirstDay", "res://resource/story/example/FirstDay.json");

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
            StatusLabel.Text = $"命令进度: {_engine.CurrentJsonPath}";
        }).UnRegisterWhenNodeExitTree(this);

        this.RegisterEvent<VisualNovelStoryFinishedEvent>(_ =>
        {
            StatusLabel.Text = "故事播放完毕。";
            _log.Debug("测试脚本播放完成");
        }).UnRegisterWhenNodeExitTree(this);

        this.RegisterEvent<VisualNovelBranchTriggeredEvent>(e =>
        {
            StatusLabel.Text = $"分支选择: {e.Options.Count} 个选项";
            // 自动选第一个——演示分支选择
            var firstOption = e.Options.Keys.First();
            _engine.ChooseBranch(firstOption);
        }).UnRegisterWhenNodeExitTree(this);

        this.RegisterEvent<VisualNovelBackgroundTriggeredEvent>(e =>
        {
            StatusLabel.Text = $"背景切换: {e.FilePath}";
        }).UnRegisterWhenNodeExitTree(this);
    }

    public override void _Input(InputEvent @event)
    {
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
