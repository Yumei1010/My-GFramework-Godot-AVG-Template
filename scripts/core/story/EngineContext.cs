using GFramework.Core.Abstractions.events;
using GFramework.Core.Abstractions.rule;

namespace GFrameworkTemplate.scripts.core.story;

/// <summary>
///     引擎运行时上下文——事件发送 + 等待推进
/// </summary>
public sealed class EngineContext
{
    private readonly IContextAware _owner;

    public TaskCompletionSource<bool>? WaitSource { get; set; }

    public EngineContext(IContextAware owner) => _owner = owner;

    public void SendEvent<T>() where T : class, new() => _owner.SendEvent<T>();
    public void SendEvent<T>(T e) where T : class => _owner.SendEvent(e);
    public IUnRegister RegisterEvent<T>(Action<T> handler) => _owner.RegisterEvent(handler);

    public async Task WaitClickAsync()
    {
        WaitSource = new TaskCompletionSource<bool>();
        await WaitSource.Task;
        WaitSource = null;
    }
}
