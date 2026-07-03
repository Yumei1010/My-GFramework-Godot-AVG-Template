using GFrameworkTemplate.scripts.cqrs.camera.query;

namespace GFrameworkTemplate.scripts.component.camera_controller;

/// <summary>
///     相机控制器——CanvasLayer 场景节点，持有 Camera2D，
///     每帧从 CameraSystem (ISystem) 读取帧数据应用到相机
/// </summary>
[Log]
[ContextAware]
public partial class CameraController : CanvasLayer
{
    private Camera2D? _camera;
    private Vector2 _basePosition;
    private float _baseZoom = 1f;
    private float _baseRotation;

    public override void _Ready()
    {
        _camera = GetNode<Camera2D>("Camera2D");
        _camera.MakeCurrent();
        _basePosition = _camera.Position;
        _baseZoom = _camera.Zoom.X;
        _baseRotation = _camera.Rotation;
        _log.Debug("CameraController 就绪");
    }

    public override void _Process(double delta)
    {
        if (_camera == null) return;

        var data = this.SendQuery(new GetCameraFrameDataQuery { Delta = (float)delta });

        _camera.Position = _basePosition + data.Offset;
        _camera.Zoom = new Vector2(data.Zoom, data.Zoom);
        _camera.Rotation = _baseRotation + data.Rotation;
    }
}
