using GFramework.Core.extensions;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkTemplate.scripts.cqrs.visualnovel.@event;
using GFrameworkTemplate.scripts.data.story;
using Godot;

namespace GFrameworkTemplate.global;

/// <summary>
///     背景管理器全局单例——双 TextureRect 交叉淡入淡出切换背景
/// </summary>
[Log]
[ContextAware]
public partial class BackgroundManager : CanvasLayer
{
    private TextureRect _mainBg = null!;
    private TextureRect _helperBg = null!;
    private Tween? _tween;

    public override void _Ready()
    {
        _mainBg = CreateBackgroundRect("MainBg");
        _helperBg = CreateBackgroundRect("HelperBg");
        AddChild(_mainBg);
        AddChild(_helperBg);
        _helperBg.Modulate = Colors.Transparent;

        this.RegisterEvent<VisualNovelBackgroundTriggeredEvent>(OnBackground).UnRegisterWhenNodeExitTree(this);
        _log.Debug("BackgroundManager 就绪");
    }

    private async void OnBackground(VisualNovelBackgroundTriggeredEvent e)
    {
        var path = StoryResourceMapper.ResolveTexturePath(e.FilePath);
        if (string.IsNullOrEmpty(path))
        {
            _log.Warn($"背景纹理未注册: {e.FilePath}");
            return;
        }

        var texture = GD.Load<Texture2D>(path);
        if (texture == null) return;

        _tween?.Kill();

        if (e.Delay > 0)
            await Task.Delay(TimeSpan.FromSeconds(e.Delay));

        if (e.WaitTweenEnd)
        {
            // 交叉淡入淡出：新背景在 helper 上加载 → helper 淡入覆盖 main
            _helperBg.Texture = texture;
            _helperBg.Modulate = Colors.Transparent;

            _tween = CreateTween();
            _tween.TweenProperty(_helperBg, "modulate", Colors.White, 0.5f);
            await ToSignal(_tween, Tween.SignalName.Finished);

            // 交换
            _mainBg.Texture = texture;
            _helperBg.Modulate = Colors.Transparent;
        }
        else
        {
            _mainBg.Texture = texture;
        }

        _log.Debug($"背景切换: {e.FilePath}");
    }

    private static TextureRect CreateBackgroundRect(string name)
    {
        var rect = new TextureRect
        {
            Name = name,
            ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
            StretchMode = TextureRect.StretchModeEnum.Scale,
            MouseFilter = Control.MouseFilterEnum.Ignore,
            AnchorRight = 1f,
            AnchorBottom = 1f,
            OffsetRight = 0f,
            OffsetBottom = 0f
        };
        rect.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        return rect;
    }
}
