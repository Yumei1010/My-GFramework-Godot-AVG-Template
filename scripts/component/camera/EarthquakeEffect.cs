using Godot;

namespace GFrameworkTemplate.scripts.component.camera;

/// <summary>
///     地震震动效果——随机衰减振荡，高优先级覆盖一切
/// </summary>
public sealed class EarthquakeEffect : CameraEffect
{
    public float Intensity { get; init; } = 20f;
    public float Decay { get; init; } = 0.9f;

    public EarthquakeEffect() { Priority = 90; }

    public override Vector2 GetOffset(float t)
    {
        var strength = Intensity * (1f - EaseOut(t)) * (float)Math.Pow(Decay, Elapsed * 10);
        return new Vector2(
            (Random.Shared.NextSingle() - 0.5f) * strength * 2f,
            (Random.Shared.NextSingle() - 0.5f) * strength * 2f);
    }
}
