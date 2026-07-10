using GFrameworkTemplate.scripts.system.camera_system;

namespace GFrameworkTemplate.scripts.entities.camera_view;

[Log]
[ContextAware]
/// <summary>
///     相机 View —— 每帧读取并应用 CameraSystem 数据
/// </summary>
public partial class CameraView : CanvasLayer
{
    public override void _Ready()
    {
        _camera = GetNode<Camera2D>("Camera2D");
        _camera.MakeCurrent();
        _basePosition = _camera.Position;
        _baseZoom = _camera.Zoom.X;
        _baseRotation = _camera.Rotation;
        _log.Debug("CameraView 就绪");
    }

    public override void _Process(double delta)
    {
        if (_camera == null) return;
        var data = this.GetSystem<CameraSystem>()!.Update((float)delta);
        _camera.Position = _basePosition + data.Offset;
        _camera.Zoom = new Vector2(data.Zoom, data.Zoom);
        _camera.Rotation = _baseRotation + data.Rotation;
    }
}
