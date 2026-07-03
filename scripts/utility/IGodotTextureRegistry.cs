using GFramework.Game.Abstractions.ui;

namespace GFrameworkTemplate.scripts.utility;

/// <summary>
///     Godot 纹理注册表接口，用于管理纹理资源的注册和查找
/// </summary>
public interface IGodotTextureRegistry : IAssetRegistry<Texture>;