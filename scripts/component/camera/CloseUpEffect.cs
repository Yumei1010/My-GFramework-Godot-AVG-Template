
namespace GFrameworkTemplate.scripts.component.camera;

/// <summary>
///     特写聚焦效果——缓动到目标位置 + 缩放，高优先级
/// </summary>
public sealed class CloseUpEffect : CameraEffect
{
    public Vector2 TargetPosition { get; init; }
    public float TargetZoom { get; init; } = 1.5f;

    public CloseUpEffect() { Priority = 70; }

    public override Vector2 GetOffset(float t) => TargetPosition * EaseInOut(t);
    public override float GetZoom(float t) => 1f + (TargetZoom - 1f) * EaseInOut(t);
}
