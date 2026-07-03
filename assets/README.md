# 素材目录说明

## 纹理 (`texture/`)

| 目录 | 用途 | 建议规格 |
|------|------|---------|
| `background/` | 场景背景图 | 1920×1080 PNG |
| `tachie/` | 角色立绘（全身/半身） | 透明 PNG，高度建议 800-1000px |
| `avatar/` | 对话头像（小图） | 透明 PNG，建议 128×128 |
| `cg/` | 特殊插画/事件图 | 1920×1080 PNG |

## 音频 (`sound/`)

| 目录 | 用途 | 格式 |
|------|------|------|
| `bgm/` | 背景音乐（循环） | .ogg / .mp3 |
| `sfx/` | 音效（一次性） | .ogg |
| `ambience/` | 环境音（循环） | .ogg |
| `voice/` | 角色语音 | .ogg |

## 使用

在章脚本中注册纹理映射：

```csharp
StoryResourceMapper.RegisterTexture("bg_classroom", "res://assets/texture/background/classroom.png");
StoryResourceMapper.RegisterTexture("saya_normal", "res://assets/texture/tachie/saya_normal.png");
```

音频文件按目录自动扫描，无需注册。在 JSON 中直接使用文件名（不含路径和扩展名）：

```json
{"type": "sound", "sound_type": "bgm", "file_path": "bgm_morning"}
{"type": "sound", "sound_type": "oneSound", "file_path": "sfx_door_open"}
```
