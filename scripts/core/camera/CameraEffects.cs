using Godot;

namespace GFrameworkTemplate.scripts.core.camera;

/// <summary>走路摇晃——周期性正弦振动，低优先级</summary>
public sealed class WalkBobEffect : CameraEffect
{
    public float Amplitude { get; init; } = 4f;
    public float Frequency { get; init; } = 2f;

    public WalkBobEffect() { Priority = 10; Duration = -1; } // 无限，手动停止

    public override Vector2 GetOffset(float t)
    {
        var phase = Elapsed * Frequency * MathF.PI * 2f;
        return new Vector2(MathF.Sin(phase * 0.5f) * Amplitude * 0.3f, MathF.Abs(MathF.Sin(phase)) * Amplitude);
    }
}

/// <summary>地震震动——随机衰减振荡，高优先级覆盖一切</summary>
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

/// <summary>特写聚焦——缓动到目标位置 + 缩放，高优先级</summary>
public sealed class CloseUpEffect : CameraEffect
{
    public Vector2 TargetPosition { get; init; }
    public float TargetZoom { get; init; } = 1.5f;

    public CloseUpEffect() { Priority = 70; }

    public override Vector2 GetOffset(float t) => TargetPosition * EaseInOut(t);
    public override float GetZoom(float t) => 1f + (TargetZoom - 1f) * EaseInOut(t);
}

/// <summary>平移滚动——线性水平移动</summary>
public sealed class PanEffect : CameraEffect
{
    public Vector2 Direction { get; init; } = Vector2.Right;
    public float Speed { get; init; } = 100f;

    public PanEffect() { Priority = 20; }

    public override Vector2 GetOffset(float t) => Direction * Speed * Elapsed;
}

/// <summary>呼吸感——极慢的缩放浮动</summary>
public sealed class BreatheEffect : CameraEffect
{
    public float Magnitude { get; init; } = 0.02f;

    public BreatheEffect() { Priority = 5; Duration = -1; }

    public override float GetZoom(float t) => 1f + MathF.Sin(Elapsed * 0.5f) * Magnitude;
}
