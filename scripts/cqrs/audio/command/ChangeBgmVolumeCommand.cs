using GFramework.Core.command;
using GFramework.Game.Abstractions.setting;
using GFramework.Game.Abstractions.setting.data;
using GFramework.Godot.setting;
using GFrameworkTemplate.scripts.cqrs.audio.command.input;

namespace GFrameworkTemplate.scripts.cqrs.audio.command;

/// <summary>
///     更改背景音乐音量命令类，用于处理BGM音量更改操作
/// </summary>
/// <param name="input">背景音乐音量更改命令输入参数</param>
/// <summary>
///     ChangeBgmVolumeCommand —— 修改 BGM 音量
/// </summary>
public sealed class ChangeBgmVolumeCommand(ChangeBgmVolumeCommandInput input)
    : AbstractAsyncCommand<ChangeBgmVolumeCommandInput>(input)
{
    /// <summary>
    ///     执行背景音乐音量更改命令
    /// </summary>
    /// <param name="input">背景音乐音量更改命令输入参数，包含新的音量值</param>
    protected override async Task OnExecuteAsync(ChangeBgmVolumeCommandInput input)
    {
        var model = this.GetModel<ISettingsModel>()!;
        model.GetData<AudioSettings>().BgmVolume = input.Volume;
        await this.GetSystem<ISettingsSystem>()!.Apply<GodotAudioSettings>().ConfigureAwait(false);
    }
}
