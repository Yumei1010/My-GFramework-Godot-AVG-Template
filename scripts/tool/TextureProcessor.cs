using Godot;

namespace GFrameworkTemplate.scripts.tool;

/// <summary>
///     纹理处理工具——F6 运行此场景即可批量缩放 / 裁剪指定目录下的 PNG 图像
///     用法：打开 misc/tool/texture_processor/texture_processor.tscn，
///     在 Inspector 中调整参数，按 F6 运行。
/// </summary>
public partial class TextureProcessor : Node
{
    /// <summary>纹理目录（res:// 路径）</summary>
    [Export] private string _sourceFolder = "res://assets/texture/tachie";

    /// <summary>目标宽度（px）</summary>
    [Export] private int _targetWidth = 540;

    /// <summary>目标高度（px）</summary>
    [Export] private int _targetHeight = 960;

    /// <summary>缩放模式：true = Fit（等比缩放不裁剪），false = Cover（填满居中裁剪）</summary>
    [Export] private bool _fitMode = true;

    /// <summary>是否递归处理子目录</summary>
    [Export] private bool _recursive = true;

    private int _processed;
    private int _skipped;

    public override void _Ready()
    {
        GD.Print($"TextureProcessor: source={_sourceFolder}, target={_targetWidth}×{_targetHeight}, fit={_fitMode}, recursive={_recursive}");

        ProcessDirectory(_sourceFolder);

        GD.Print($"TextureProcessor: {_processed} processed, {_skipped} skipped. Done!");
        GetTree().Quit();
    }

    private void ProcessDirectory(string path)
    {
        using var dir = DirAccess.Open(path);
        if (dir == null)
        {
            GD.PrintErr($"TextureProcessor: cannot open {path}");
            return;
        }

        dir.ListDirBegin();
        var entry = dir.GetNext();
        while (entry != "")
        {
            var fullPath = path.PathJoin(entry);

            if (dir.CurrentIsDir())
            {
                if (_recursive && !entry.StartsWith('.'))
                    ProcessDirectory(fullPath);
            }
            else if (entry.EndsWith(".png") || entry.EndsWith(".PNG"))
            {
                ProcessImage(fullPath);
            }

            entry = dir.GetNext();
        }
        dir.ListDirEnd();
    }

    private void ProcessImage(string filePath)
    {
        var img = Image.LoadFromFile(filePath);
        if (img == null) return;

        var w = img.GetWidth();
        var h = img.GetHeight();

        if (w == _targetWidth && h == _targetHeight)
        {
            GD.Print($"  SKIP {filePath.GetFile()}: already {w}×{h}");
            _skipped++;
            return;
        }

        if (_fitMode)
        {
            ProcessFit(img, filePath, w, h);
        }
        else
        {
            ProcessCover(img, filePath, w, h);
        }

        _processed++;
    }

    private void ProcessFit(Image img, string savePath, int srcW, int srcH)
    {
        var scale = Mathf.Min((float)_targetWidth / srcW, (float)_targetHeight / srcH);
        var newW = Mathf.RoundToInt(srcW * scale);
        var newH = Mathf.RoundToInt(srcH * scale);

        img.Resize(newW, newH, Image.Interpolation.Lanczos);
        img.SavePng(savePath);

        GD.Print($"  FIT  {savePath.GetFile()}: {srcW}×{srcH} → {newW}×{newH}");
    }

    private void ProcessCover(Image img, string savePath, int srcW, int srcH)
    {
        var scale = Mathf.Max((float)_targetWidth / srcW, (float)_targetHeight / srcH);
        var newW = Mathf.RoundToInt(srcW * scale);
        var newH = Mathf.RoundToInt(srcH * scale);

        img.Resize(newW, newH, Image.Interpolation.Lanczos);

        var target = Image.Create(_targetWidth, _targetHeight, false, Image.Format.Rgba8);
        var cropX = (newW - _targetWidth) / 2;
        var cropY = (newH - _targetHeight) / 2;
        target.BlitRect(img, new Rect2I(cropX, cropY, _targetWidth, _targetHeight), Vector2I.Zero);
        target.SavePng(savePath);

        GD.Print($"  COVER {savePath.GetFile()}: {srcW}×{srcH} → {_targetWidth}×{_targetHeight}");
    }
}
