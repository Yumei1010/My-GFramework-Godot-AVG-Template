using GFramework.Core.Abstractions.enums;
using GFramework.Core.Abstractions.system;
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
public partial class BackgroundManager : CanvasLayer, ISystem
{
    public void OnArchitecturePhase(ArchitecturePhase phase) { }
    public void Init() { }
    public void Destroy() { }
    private TextureRect MainBg => GetNode<TextureRect>("%MainBg");
    private TextureRect HelperBg => GetNode<TextureRect>("%HelperBg");
    private Tween? _tween;

    public override void _Ready()
    {
        HelperBg.Modulate = Colors.Transparent;
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
            HelperBg.Texture = texture;
            HelperBg.Modulate = Colors.Transparent;

            _tween = CreateTween();
            _tween.TweenProperty(HelperBg, "modulate", Colors.White, 0.5f);
            await ToSignal(_tween, Tween.SignalName.Finished);

            MainBg.Texture = texture;
            HelperBg.Modulate = Colors.Transparent;
        }
        else
        {
            MainBg.Texture = texture;
        }

        _log.Debug($"背景切换: {e.FilePath}");
    }
}
