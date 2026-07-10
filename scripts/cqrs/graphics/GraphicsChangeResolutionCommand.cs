using GFramework.Core.command;
using GFramework.Game.Abstractions.setting;
using GFramework.Game.Abstractions.setting.data;
using GFramework.Godot.setting;
using GFrameworkTemplate.scripts.cqrs.graphics.input;

namespace GFrameworkTemplate.scripts.cqrs.graphics.command;

/// <summary>
///     GraphicsChangeResolutionCommand —— 更改分辨率
/// </summary>
public sealed class GraphicsChangeResolutionCommand(GraphicsChangeResolutionInput input)
    : AbstractAsyncCommand<GraphicsChangeResolutionInput>(input)
{
    protected override async Task OnExecuteAsync(GraphicsChangeResolutionInput input)
    {
        var model = this.GetModel<ISettingsModel>()!;
        var settings = model.GetData<GraphicsSettings>();
        settings.ResolutionWidth = input.Width;
        settings.ResolutionHeight = input.Height;
        await this.GetSystem<ISettingsSystem>()!.Apply<GodotGraphicsSettings>().ConfigureAwait(false);
    }
}
