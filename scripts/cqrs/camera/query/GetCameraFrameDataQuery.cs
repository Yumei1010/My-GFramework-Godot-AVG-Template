using GFrameworkTemplate.scripts.component.camera;
using GFrameworkTemplate.scripts.model.camera;

namespace GFrameworkTemplate.scripts.cqrs.camera.query;

/// <summary>
///     查询当前帧的相机偏移数据
/// </summary>
public sealed class GetCameraFrameDataQuery : AbstractQuery<CameraFrameDataResult>
{
    public required float Delta { get; set; }

    protected override CameraFrameDataResult OnDo()
    {
        var effects = this.GetModel<CameraModel>()!.Effects;
        for (var i = effects.Count - 1; i >= 0; i--)
        {
            effects[i].Elapsed += Delta;
            if (effects[i].IsExpired) effects.RemoveAt(i);
        }

        if (effects.Count == 0) return CameraFrameDataResult.Identity;

        effects.Sort((a, b) => b.Priority.CompareTo(a.Priority));

        var offset = Vector2.Zero; var zoom = 1f; var rotation = 0f;
        foreach (var fx in effects)
        {
            var t = fx.Progress;
            offset += fx.GetOffset(t);
            zoom *= fx.GetZoom(t);
            rotation += fx.GetRotation(t);
        }

        return new CameraFrameDataResult { Offset = offset, Zoom = zoom, Rotation = rotation };
    }
}

public sealed class CameraFrameDataResult
{
    public Vector2 Offset { get; init; }
    public float Zoom { get; init; }
    public float Rotation { get; init; }
    public static CameraFrameDataResult Identity => new() { Zoom = 1f };
}
