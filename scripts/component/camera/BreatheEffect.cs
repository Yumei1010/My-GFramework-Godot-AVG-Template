using Godot;

namespace GFrameworkTemplate.scripts.component.camera;

/// <summary>
///     呼吸感效果——极慢的缩放浮动
/// </summary>
public sealed class BreatheEffect : CameraEffect
{
    public float Magnitude { get; init; } = 0.02f;

    public BreatheEffect() { Priority = 5; Duration = -1; }

    public override float GetZoom(float t) => 1f + MathF.Sin(Elapsed * 0.5f) * Magnitude;
}
