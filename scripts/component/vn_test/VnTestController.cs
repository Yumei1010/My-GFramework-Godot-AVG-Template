using GFramework.Core.extensions;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.system.visualnovel;
using Godot;

namespace GFrameworkTemplate.scripts.component.vn_test;

/// <summary>
///     VN 引擎测试控制器——组装 TalkBar + BranchBar + TachieBar，驱动故事播放
/// </summary>
[Log]
[ContextAware]
public partial class VnTestController : Node
{
    private Label StatusLabel => GetNode<Label>("%StatusLabel");

    private StoryEngineSystem _engine = null!;

    public override void _Ready()
    {
        _engine = this.GetUtility<StoryEngineSystem>()!;

        StoryEngineSystem.RegisterJson("FirstDay", "res://resource/story/example/First.json");
        StoryEngineSystem.RegisterJson("Second.json", "res://resource/story/example/Second.json");

        this.RegisterEvent<VisualNovelStoryFinishedEvent>(_ =>
            StatusLabel.Text = "故事播放完毕。"
        ).UnRegisterWhenNodeExitTree(this);

        StatusLabel.Text = "点击屏幕开始测试...";
        _log.Debug("VnTestController 就绪");
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
