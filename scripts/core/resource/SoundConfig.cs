using GFramework.Core.Abstractions.bases;
using GFrameworkTemplate.scripts.enums.resources;

namespace GFrameworkTemplate.scripts.core.resource;

/// <summary>
///     音频资源配置，在 GameEntryPoint 导出数组中注册
/// </summary>
[GlobalClass]
public partial class SoundConfig : Resource, IKeyValue<string, AudioStream>
{
    [Export]
    public SoundKey SoundKey { get; set; }

    [Export]
    public AudioStream Stream { get; set; } = null!;

    public string Key => SoundKey.ToString();
    public AudioStream Value => Stream;
}
