using GFramework.Core.extensions;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.component.camera;
using GFrameworkTemplate.scripts.system.camera_manager;
using GFrameworkTemplate.scripts.system.talk_manager;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.system.visualnovel;
using Godot;

namespace GFrameworkTemplate.scripts.component.vn_test;

/// <summary>
///     VN 引擎测试控制器
///     数字键 1-5: 相机效果 | R: 重置故事
/// </summary>
[Log]
[ContextAware]
public partial class VnTestController : Node
{
    private Label StatusLabel => GetNode<Label>("%StatusLabel");
    private StoryEngineSystem _engine = null!;

    public override void _Ready()
    {
        _engine = this.GetSystem<StoryEngineSystem>()!;

        StoryEngineSystem.RegisterJson("FirstDay", "res://resource/story/chapter1/Chapter1_Prologue.json");
        StoryEngineSystem.RegisterJson("Chapter2.json", "res://resource/story/chapter2/Chapter2.json");
        StoryEngineSystem.RegisterJson("Chapter3.json", "res://resource/story/chapter3/Chapter3.json");

        this.RegisterEvent<VisualNovelStoryFinishedEvent>(_ =>
            StatusLabel.Text = "故事播放完毕。按 1-5 测试相机效果，R 重置。"
        ).UnRegisterWhenNodeExitTree(this);

        this.RegisterEvent<VisualNovelCustomEventTriggeredEvent>(e =>
        {
            switch (e.EventName)
            {
                case "chapter_end":
                    this.GetSystem<CameraManager>().Play(new CloseUpEffect { TargetPosition = Vector2.Zero, TargetZoom = 1.3f, Duration = 1.5f });
                    break;
                case "finale":
                    this.GetSystem<CameraManager>().Play(new BreatheEffect { Magnitude = 0.03f });
                    this.GetSystem<CameraManager>().Play(new CloseUpEffect { TargetPosition = new Vector2(0, -20), TargetZoom = 1.2f, Duration = 3f });
                    break;
            }
        }).UnRegisterWhenNodeExitTree(this);

        StatusLabel.Text = "点击开始 | 1-5: 相机效果 | R: 重置";
        _log.Debug("VnTestController 就绪");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
        {
            if (!_engine.IsPlaying)
            {
                _ = _engine.LoadAndPlay("FirstDay");
                StatusLabel.Text = "播放中... | 1-5: 相机 | R: 重置";
            }
            else
            {
                _engine.Advance();
            }
        }

        if (@event is InputEventKey { Pressed: true } key)
        {
            switch (key.Keycode)
            {
                case Key.Key1:
                    this.GetSystem<CameraManager>().Play(new WalkBobEffect());
                    StatusLabel.Text = "走路摇晃 (按 1 再次叠加)";
                    break;
                case Key.Key2:
                    this.GetSystem<CameraManager>().Play(new EarthquakeEffect { Duration = 1.5f, Intensity = 25f });
                    StatusLabel.Text = "地震震动 1.5s";
                    break;
                case Key.Key3:
                    this.GetSystem<CameraManager>().Play(new CloseUpEffect { TargetPosition = new Vector2(50, -30), TargetZoom = 2f, Duration = 1f });
                    StatusLabel.Text = "特写聚焦 1s";
                    break;
                case Key.Key4:
                    this.GetSystem<CameraManager>().Play(new PanEffect { Direction = Vector2.Left, Speed = 80f, Duration = 2f });
                    StatusLabel.Text = "左平移 2s";
                    break;
                case Key.Key5:
                    this.GetSystem<CameraManager>().Stop<WalkBobEffect>();
                    this.GetSystem<CameraManager>().Clear();
                    StatusLabel.Text = "相机重置";
                    break;
                case Key.H:
                    this.GetSystem<TalkManager>().Toggle();
                    StatusLabel.Text = this.GetSystem<TalkManager>().IsVisible ? "对话框: 显示" : "对话框: 隐藏";
                    break;
                case Key.R:
                    _engine.Stop();
                    _ = _engine.LoadAndPlay("FirstDay");
                    StatusLabel.Text = "已重置，重新播放中...";
                    break;
            }
        }
    }
}
