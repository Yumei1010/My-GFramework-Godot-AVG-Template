using GFramework.Core.Abstractions.events;
using GFramework.Core.Abstractions.rule;
using GFramework.Core.extensions;

namespace GFrameworkTemplate.scripts.system.visualnovel;

public sealed class EngineContext
{
    private readonly IContextAware _owner;

    public List<string> TalkBranch { get; } = new();
    public List<string> CanNotChoose { get; } = new();
    public string PlayingJson { get; set; } = string.Empty;
    public float? AutoPlayDelay { get; set; }
    public float WordSpeed { get; set; } = 0.02f;
    public bool IsPlaying { get; set; }
    public string? PendingGoto { get; set; }
    public TaskCompletionSource<bool>? WaitSource { get; set; }

    public EngineContext(IContextAware owner) => _owner = owner;

    public void SendEvent<T>() where T : class, new() => _owner.SendEvent<T>();
    public void SendEvent<T>(T e) where T : class => _owner.SendEvent(e);
    public IUnRegister RegisterEvent<T>(Action<T> handler) => _owner.RegisterEvent(handler);
}
