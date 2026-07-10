namespace GFrameworkTemplate.scripts.cqrs.camera.query;

/// <summary>逐帧相机变换复合结果</summary>
public sealed class CameraFrameDataResult
{
    public required Vector2 Offset { get; init; }
    public required float Zoom { get; init; }
    public required float Rotation { get; init; }
    public static CameraFrameDataResult Identity => new() { Offset = Vector2.Zero, Zoom = 1f, Rotation = 0f };
}
