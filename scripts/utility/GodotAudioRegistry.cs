using GFrameworkTemplate.scripts.core.resource;

namespace GFrameworkTemplate.scripts.utility;

/// <summary>
///     音频资源注册表
/// </summary>
public class GodotAudioRegistry : IGodotAudioRegistry
{
    private readonly Dictionary<string, AudioStream> _map = new();

    public void Registry(SoundConfig config) => _map[config.Key] = config.Value;

    public AudioStream? Get(string key) => _map.GetValueOrDefault(key);
}
