using GFramework.Core.model;
using GFrameworkTemplate.scripts.core.story;

namespace GFrameworkTemplate.scripts.model.visualnovel;

/// <summary>
///     故事状态模型——存储当前播放进度、分支选择和引擎配置
/// </summary>
public class StoryStateModel : AbstractModel
{
    /// <summary>当前已加载的命令列表</summary>
    public List<StoryCommand> Commands { get; set; } = new();

    /// <summary>当前命令索引</summary>
    public int CurrentIndex { get; set; }

    /// <summary>玩家已选择的分支 ID 列表</summary>
    public List<string> TalkBranch { get; set; } = new();

    /// <summary>已被禁用的分支 ID 列表</summary>
    public List<string> CanNotChoose { get; set; } = new();

    /// <summary>当前播放的脚本文件路径</summary>
    public string PlayingJson { get; set; } = string.Empty;

    /// <summary>引擎是否正在播放</summary>
    public bool IsPlaying { get; set; }

    /// <summary>Goto 跳转的目标脚本</summary>
    public string? PendingGoto { get; set; }

    /// <summary>自动播放延迟（null=手动）</summary>
    public float? AutoPlayDelay { get; set; }

    /// <summary>打字机速度（秒/字符）</summary>
    public float WordSpeed { get; set; } = 0.02f;

    protected override void OnInit() { }
}
