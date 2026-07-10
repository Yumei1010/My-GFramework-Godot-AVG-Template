using GFramework.Core.Abstractions.utility;
using GFrameworkTemplate.scripts.core.resource;

namespace GFrameworkTemplate.scripts.utility;

/// <summary>
///     音频资源注册表接口
/// </summary>
public interface IGodotAudioRegistry : IUtility
{
    void Registry(SoundConfig config);
    AudioStream? Get(string key);
}
