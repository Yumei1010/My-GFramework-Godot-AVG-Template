using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.model.visualnovel;

namespace GFrameworkTemplate.scripts.cqrs.story.command;

/// <summary>
///     更新故事状态模型（内部命令，由 StoryEngineSystem 调用）
/// </summary>
public sealed class UpdateStoryStateCommand : AbstractCommand
{
    public List<StoryCommand>? Commands { get; set; }
    public int? CurrentIndex { get; set; }
    public bool? IsPlaying { get; set; }
    public string? PlayingJson { get; set; }
    public string? PendingGoto { get; set; }
    public List<string>? TalkBranch { get; set; }
    public List<string>? CanNotChoose { get; set; }
    public float? AutoPlayDelay { get; set; }
    public float? WordSpeed { get; set; }

    protected override void OnExecute()
    {
        var model = this.GetModel<StoryStateModel>()!;
        if (Commands != null) model.Commands = Commands;
        if (CurrentIndex.HasValue) model.CurrentIndex = CurrentIndex.Value;
        if (IsPlaying.HasValue) model.IsPlaying = IsPlaying.Value;
        if (PlayingJson != null) model.PlayingJson = PlayingJson;
        if (PendingGoto != null) model.PendingGoto = PendingGoto;
        if (TalkBranch != null) model.TalkBranch = TalkBranch;
        if (CanNotChoose != null) model.CanNotChoose = CanNotChoose;
        if (AutoPlayDelay.HasValue) model.AutoPlayDelay = AutoPlayDelay;
        if (WordSpeed.HasValue) model.WordSpeed = WordSpeed.Value;
    }
}
