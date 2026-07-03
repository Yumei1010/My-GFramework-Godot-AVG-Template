using GFramework.Core.command;
using GFramework.Game.Abstractions.setting;
using GFramework.Game.Abstractions.setting.data;
using GFramework.Godot.setting;
using GFrameworkTemplate.scripts.cqrs.graphics.input;

namespace GFrameworkTemplate.scripts.cqrs.graphics.command;

/// <summary>
///     更改分辨率命令类，用于处理分辨率更改操作
/// </summary>
/// <param name="input">分辨率更改命令输入参数</param>
public sealed class ChangeResolutionCommand(ChangeResolutionCommandInput input)
    : AbstractAsyncCommand<ChangeResolutionCommandInput>(input)
{
    /// <summary>
    ///     执行分辨率更改命令
    /// </summary>
    /// <param name="input">分辨率更改命令输入参数，包含新的宽度和高度值</param>
    protected override async Task OnExecuteAsync(ChangeResolutionCommandInput input)
    {
        var model = this.GetModel<ISettingsModel>()!;
        var settings = model.GetData<GraphicsSettings>();
        settings.ResolutionWidth = input.Width;
        settings.ResolutionHeight = input.Height;
        await this.GetSystem<ISettingsSystem>()!.Apply<GodotGraphicsSettings>().ConfigureAwait(false);
    }
}