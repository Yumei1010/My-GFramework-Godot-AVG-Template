using GFramework.Core.Abstractions.registries;

namespace GFrameworkTemplate.scripts.utility;

/// <summary>
/// GodotTextureRegistry 类继承自 KeyValueRegistryBase 并实现 IGodotTextureRegistry 接口。
/// 该类用于管理纹理资源的注册表，提供基于字符串键的纹理查找功能。
/// </summary>
/// <remarks>
/// 此类使用 StringComparer.Ordinal 作为键的比较器，确保键的比较行为一致且高效。
/// </remarks>
public class GodotTextureRegistry() : KeyValueRegistryBase<string, Texture>(StringComparer.Ordinal), IGodotTextureRegistry;