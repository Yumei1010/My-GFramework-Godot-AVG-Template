using Godot;

namespace GFrameworkTemplate.scripts.core.camera;

/// <summary>
///     相机效果策略基类——每个子类定义一种镜头效果
/// </summary>
public abstract class CameraEffect
{
    /// <summary>优先级（数值越大越靠前，同类型高优先级覆盖低优先级）</summary>
    public int Priority { get; init; } = 50;

    /// <summary>持续时间（秒），0 表示瞬时，负值表示无限</summary>
    public float Duration { get; init; }

    /// <summary>已流逝时间</summary>
    public float Elapsed { get; set; }

    /// <summary>是否已结束</summary>
    public bool IsExpired => Duration >= 0 && Elapsed >= Duration;

    /// <summary>时间进度 0~1（应用 Easing 之前）</summary>
    public float Progress => Duration > 0 ? Math.Clamp(Elapsed / Duration, 0f, 1f) : 1f;

    /// <summary>获取当前位置偏移（默认无偏移）</summary>
    public virtual Vector2 GetOffset(float t) => Vector2.Zero;

    /// <summary>获取缩放因子（1.0 表示不变）</summary>
    public virtual float GetZoom(float t) => 1f;

    /// <summary>获取旋转角度（弧度）</summary>
    public virtual float GetRotation(float t) => 0f;

    /// <summary>Easing 函数（默认线性）</summary>
    protected static float EaseInOut(float t) => t < 0.5f ? 2f * t * t : 1f - MathF.Pow(-2f * t + 2f, 2f) / 2f;
    protected static float EaseOut(float t) => 1f - MathF.Pow(1f - t, 3f);
    protected static float EaseIn(float t) => t * t * t;
}
