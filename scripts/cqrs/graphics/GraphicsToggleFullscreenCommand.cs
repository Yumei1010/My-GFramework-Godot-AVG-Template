using GFramework.Core.command;
using GFramework.Game.Abstractions.setting;
using GFramework.Game.Abstractions.setting.data;
using GFramework.Godot.setting;
using GFrameworkTemplate.scripts.cqrs.graphics.input;

namespace GFrameworkTemplate.scripts.cqrs.graphics.command;

/// <summary>
///     GraphicsToggleFullscreenCommand —— 切换全屏
/// </summary>
public sealed class GraphicsToggleFullscreenCommand(GraphicsToggleFullscreenInput input)
    : AbstractAsyncCommand<GraphicsToggleFullscreenInput>(input)
{
    protected override async Task OnExecuteAsync(GraphicsToggleFullscreenInput input)
    {
        var model = this.GetModel<ISettingsModel>()!;
        model.GetData<GraphicsSettings>().Fullscreen = input.Fullscreen;
        await this.GetSystem<ISettingsSystem>()!.Apply<GodotGraphicsSettings>().ConfigureAwait(false);
    }
}
