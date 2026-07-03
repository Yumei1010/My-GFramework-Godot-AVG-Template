using GFrameworkTemplate.global;
using GFrameworkTemplate.scripts.component.camera;
using GFrameworkTemplate.scripts.cqrs.camera.command;
using GFrameworkTemplate.scripts.cqrs.story.command;
using GFrameworkTemplate.scripts.cqrs.talk.command;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.system.background;
using GFrameworkTemplate.scripts.system.branch;
using GFrameworkTemplate.scripts.system.camera;
using GFrameworkTemplate.scripts.system.sound;
using GFrameworkTemplate.scripts.system.tachie;
using GFrameworkTemplate.scripts.system.talk;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.scripts.component.vn_test;

/// <summary>
///     VN 引擎测试控制器——通过 SendCommand 走 CQRS 管线
///     数字键 1-5: 相机效果 | H: 切换对话框 | R: 重置
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
                    this.SendCommand(new PlayCameraEffectCommand
                        { Effect = new CloseUpEffect { TargetPosition = Vector2.Zero, TargetZoom = 1.3f, Duration = 1.5f } });
                    break;
                case "finale":
                    this.SendCommand(new PlayCameraEffectCommand { Effect = new BreatheEffect { Magnitude = 0.03f } });
                    this.SendCommand(new PlayCameraEffectCommand
                        { Effect = new CloseUpEffect { TargetPosition = new Vector2(0, -20), TargetZoom = 1.2f, Duration = 3f } });
                    break;
            }
        }).UnRegisterWhenNodeExitTree(this);

        // 将子 Manager 注册到架构 DI（替代 autoload 的自动注册）
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<TalkSystem>("TalkManager")); } catch { }
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<TachieSystem>("TachieManager")); } catch { }
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<BranchSystem>("BranchManager")); } catch { }
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<BackgroundSystem>("BackgroundManager")); } catch { }
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<CameraSystem>("CameraManager")); } catch { }
        try { GameEntryPoint.Architecture.RegisterSystem(GetNode<SoundSystem>("SoundManager")); } catch { }

        StatusLabel.Text = "点击开始 | 1-5: 相机效果 | H: 对话框 | R: 重置";
        _log.Debug("VnTestController 就绪");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
        {
            if (!_engine.IsPlaying)
            {
                this.SendCommand(new LoadStoryCommand { StoryName = "FirstDay" });
                StatusLabel.Text = "播放中... | 1-5: 相机 | H: 对话框 | R: 重置";
            }
            else
            {
                this.SendCommand(new AdvanceStoryCommand());
            }
        }

        if (@event is InputEventKey { Pressed: true } key)
        {
            switch (key.Keycode)
            {
                case Key.Key1:
                    this.SendCommand(new PlayCameraEffectCommand { Effect = new WalkBobEffect() });
                    StatusLabel.Text = "走路摇晃 (按 1 再次叠加)";
                    break;
                case Key.Key2:
                    this.SendCommand(new PlayCameraEffectCommand
                        { Effect = new EarthquakeEffect { Duration = 1.5f, Intensity = 25f } });
                    StatusLabel.Text = "地震震动 1.5s";
                    break;
                case Key.Key3:
                    this.SendCommand(new PlayCameraEffectCommand
                        { Effect = new CloseUpEffect { TargetPosition = new Vector2(50, -30), TargetZoom = 2f, Duration = 1f } });
                    StatusLabel.Text = "特写聚焦 1s";
                    break;
                case Key.Key4:
                    this.SendCommand(new PlayCameraEffectCommand
                        { Effect = new PanEffect { Direction = Vector2.Left, Speed = 80f, Duration = 2f } });
                    StatusLabel.Text = "左平移 2s";
                    break;
                case Key.Key5:
                    this.SendCommand(new StopCameraEffectCommand());
                    StatusLabel.Text = "相机重置";
                    break;
                case Key.H:
                    this.SendCommand(new ToggleTalkCommand());
                    StatusLabel.Text = "对话框: 已切换";
                    break;
                case Key.R:
                    _engine.Stop();
                    this.SendCommand(new LoadStoryCommand { StoryName = "FirstDay" });
                    StatusLabel.Text = "已重置，重新播放中...";
                    break;
            }
        }
    }
}
