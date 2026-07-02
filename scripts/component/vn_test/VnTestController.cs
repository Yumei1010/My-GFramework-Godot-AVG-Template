using GFramework.Core.extensions;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.data.story;
using GFrameworkTemplate.scripts.system.visualnovel;
using Godot;

namespace GFrameworkTemplate.scripts.component.vn_test;

/// <summary>
///     VN 引擎测试控制器——TalkManager/TachieManager/BranchManager 全局单例自动响应事件
/// </summary>
[Log]
[ContextAware]
public partial class VnTestController : Node
{
    private Label StatusLabel => GetNode<Label>("%StatusLabel");

    private StoryEngineSystem _engine = null!;
    private TextureRect _bgRect = null!;

    public override void _Ready()
    {
        _engine = this.GetUtility<StoryEngineSystem>()!;

        StoryEngineSystem.RegisterJson("FirstDay", "res://resource/story/chapter1/Chapter1_Prologue.json");
        StoryEngineSystem.RegisterJson("Chapter2.json", "res://resource/story/chapter2/Chapter2.json");
        StoryEngineSystem.RegisterJson("Chapter3.json", "res://resource/story/chapter3/Chapter3.json");

        StoryResourceMapper.RegisterTexture("bg_classroom_sunset", "res://assets/texture/background/chapter_1.png");
        StoryResourceMapper.RegisterTexture("bg_school_night", "res://assets/texture/background/chapter_2.png");
        StoryResourceMapper.RegisterTexture("bg_library_dark", "res://assets/texture/background/chapter_2.png");
        StoryResourceMapper.RegisterTexture("bg_stairway_underground", "res://assets/texture/background/chapter_2.png");
        StoryResourceMapper.RegisterTexture("bg_underground_city", "res://assets/texture/background/chapter_3.png");
        StoryResourceMapper.RegisterTexture("bg_white_space", "res://assets/texture/background/chapter_3.png");

        // 全屏背景
        _bgRect = new TextureRect
        {
            ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
            StretchMode = TextureRect.StretchModeEnum.Scale,
            MouseFilter = Control.MouseFilterEnum.Ignore
        };
        AddChild(_bgRect);
        MoveChild(_bgRect, 0); // 放到最底层

        this.RegisterEvent<VisualNovelStoryFinishedEvent>(_ =>
            StatusLabel.Text = "故事播放完毕。"
        ).UnRegisterWhenNodeExitTree(this);

        this.RegisterEvent<VisualNovelBackgroundTriggeredEvent>(e =>
        {
            var path = StoryResourceMapper.ResolveTexturePath(e.FilePath);
            if (!string.IsNullOrEmpty(path))
                _bgRect.Texture = GD.Load<Texture2D>(path);
            _log.Debug($"背景切换: {e.FilePath}");
        }).UnRegisterWhenNodeExitTree(this);

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
