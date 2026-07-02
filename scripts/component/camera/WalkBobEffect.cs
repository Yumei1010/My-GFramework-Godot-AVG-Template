using Godot;

namespace GFrameworkTemplate.scripts.component.camera;

/// <summary>
///     走路摇晃效果——周期性正弦振动，低优先级
/// </summary>
public sealed class WalkBobEffect : CameraEffect
{
    public float Amplitude { get; init; } = 4f;
    public float Frequency { get; init; } = 2f;

    public WalkBobEffect() { Priority = 10; Duration = -1; }

    public override Vector2 GetOffset(float t)
    {
        var phase = Elapsed * Frequency * MathF.PI * 2f;
        return new Vector2(MathF.Sin(phase * 0.5f) * Amplitude * 0.3f, MathF.Abs(MathF.Sin(phase)) * Amplitude);
    }
}
