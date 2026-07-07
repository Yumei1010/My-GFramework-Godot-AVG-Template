using GFramework.Core.Abstractions.events;
using GFramework.Core.Abstractions.rule;

namespace GFrameworkTemplate.scripts.core.story;

/// <summary>
///     引擎运行时上下文——事件发送 + 等待推进机制
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
    /// <summary>引擎是否正在播放</summary>
    public bool IsPlaying { get; set; }
    /// <summary>Goto 跳转的目标脚本（非null时PlayLoop结束后自动加载）</summary>
    public string? PendingGoto { get; set; }
    /// <summary>等待玩家推进的信号源</summary>
    public TaskCompletionSource<bool>? WaitSource { get; set; }

    public EngineContext(IContextAware owner) => _owner = owner;

    public void SendEvent<T>() where T : class, new() => _owner.SendEvent<T>();
    public void SendEvent<T>(T e) where T : class => _owner.SendEvent(e);
    public IUnRegister RegisterEvent<T>(Action<T> handler) => _owner.RegisterEvent(handler);

    /// <summary>等待玩家点击推进</summary>
    public async Task WaitClickAsync()
    {
        WaitSource = new TaskCompletionSource<bool>();
        await WaitSource.Task;
        WaitSource = null;
    }
}
