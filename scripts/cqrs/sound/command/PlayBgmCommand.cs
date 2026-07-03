using GFrameworkTemplate.scripts.model.sound;

namespace GFrameworkTemplate.scripts.cqrs.sound.command;

/// <summary>
///     播放背景音乐
/// </summary>
public sealed class PlayBgmCommand : AbstractCommand
{
    public required string LogicalName { get; set; }

    protected override void OnExecute()
    {
        var model = this.GetModel<SoundModel>()!;
        if (model.CurrentBgm == LogicalName) return;
        model.CurrentBgm = LogicalName;
    }
}
