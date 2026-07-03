using GFramework.Core.Abstractions.events;
using GFramework.Core.Abstractions.rule;
using GFramework.Core.extensions;

namespace GFrameworkTemplate.scripts.core.story;

/// <summary>
///     引擎运行时上下文——封装 Worker 所需的全部状态、事件发送和等待机制
/// </summary>
public sealed class EngineContext
{
    private readonly IContextAware _owner;

    /// <summary>玩家已选择的分支 ID 列表</summary>
    public List<string> TalkBranch { get; } = new();
    /// <summary>已被禁用的分支 ID 列表</summary>
    public List<string> CanNotChoose { get; } = new();
    /// <summary>当前播放的脚本文件路径</summary>
    public string PlayingJson { get; set; } = string.Empty;
    /// <summary>自动播放延迟（null=手动，非null=自动间隔秒数）</summary>
    public float? AutoPlayDelay { get; set; }
    /// <summary>打字机速度（秒/字符）</summary>
    public float WordSpeed { get; set; } = 0.02f;
    /// <summary>引擎是否正在播放</summary>
    public bool IsPlaying { get; set; }
    /// <summary>Goto 跳转的目标脚本（非null时PlayLoop结束后自动加载）</summary>
    public string? PendingGoto { get; set; }
    /// <summary>等待玩家推进的信号源</summary>
    public TaskCompletionSource<bool>? WaitSource { get; set; }

    /// <summary>创建引擎上下文</summary>
    public EngineContext(IContextAware owner) => _owner = owner;

    public void SendEvent<T>() where T : class, new() => _owner.SendEvent<T>();
    public void SendEvent<T>(T e) where T : class => _owner.SendEvent(e);
    public IUnRegister RegisterEvent<T>(Action<T> handler) => _owner.RegisterEvent(handler);

    /// <summary>等待玩家点击推进或自动播放计时器</summary>
    public async Task AdvanceAsync(float minDuration)
    {
        if (AutoPlayDelay.HasValue)
        {
            await Task.Delay(TimeSpan.FromSeconds(Math.Max(minDuration, AutoPlayDelay.Value)));
        }
        else
        {
            WaitSource = new TaskCompletionSource<bool>();
            await WaitSource.Task;
            WaitSource = null;
        }
    }
}
