using GFramework.Core.command;
using GFramework.Game.Abstractions.setting;
using GFramework.Game.Abstractions.setting.data;
using GFramework.Godot.setting;
using GFrameworkTemplate.scripts.cqrs.setting.command.input;

namespace GFrameworkTemplate.scripts.cqrs.setting.command;

/// <summary>
///     SettingChangeLanguageCommand —— 更改语言设置
/// </summary>
public sealed class SettingChangeLanguageCommand(SettingChangeLanguageInput input)
    : AbstractAsyncCommand<SettingChangeLanguageInput>(input)
{
    protected override async Task OnExecuteAsync(SettingChangeLanguageInput input)
    {
        var model = this.GetModel<ISettingsModel>()!;
        var settings = model.GetData<LocalizationSettings>();
        settings.Language = input.Language;
        await this.GetSystem<ISettingsSystem>()!.Apply<GodotLocalizationSettings>().ConfigureAwait(false);
    }
}
