using GFramework.Core.model;

namespace GFrameworkTemplate.scripts.model.talk;

/// <summary>
///     对话模型——管理对话栏可见性与打字机状态
/// </summary>
public class TalkModel : AbstractModel
{
    /// <summary>对话栏是否可见</summary>
    public bool Visible { get; set; }

    /// <summary>打字机是否已完成文本显示</summary>
    public bool Revealed { get; set; } = true;

    /// <summary>打字速度（秒/字符）</summary>
    public float Speed { get; set; } = 0.04f;

    protected override void OnInit() { }
}
