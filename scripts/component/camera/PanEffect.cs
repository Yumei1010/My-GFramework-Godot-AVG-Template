using Godot;

namespace GFrameworkTemplate.scripts.component.camera;

/// <summary>
///     平移滚动效果——线性水平移动
/// </summary>
public sealed class PanEffect : CameraEffect
{
    public Vector2 Direction { get; init; } = Vector2.Right;
    public float Speed { get; init; } = 100f;

    public PanEffect() { Priority = 20; }

    public override Vector2 GetOffset(float t) => Direction * Speed * Elapsed;
}
