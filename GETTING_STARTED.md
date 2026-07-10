# 快速上手

本指南面向**编剧**和**开发者**，教你如何使用本 VN 框架模板构建视觉小说游戏。

---

## 目录

- [编剧篇：写一个 JSON 故事](#编剧篇写一个-json-故事)
- [开发者篇：扩展框架](#开发者篇扩展框架)
- [运行测试故事](#运行测试故事)

---

## 编剧篇：写一个 JSON 故事

你不需要会写代码，只要会写 JSON。

### 1. 新建 JSON 文件

在 `assets/story/` 下创建 `.json` 文件：

```json
{
  "content": [
    {"type": "talk", "talker": "小夜", "talk_content": "你好，欢迎来到这个世界。"},
    {"type": "background", "file_path": "BgTest0", "wait_tween_end": "1"},
    {"type": "talk", "is_center": "1", "talk_content": "（窗外下起了雨。）"}
  ]
}
```

### 2. 七种命令

#### `talk` — 对话 / 旁白

| 字段 | 说明 | 必填 |
|---|---|---|
| `talk_content` | 对话文本 | ✅ |
| `talker` | 说话人姓名（不填=旁白） | 否 |
| `is_center` | `"1"` 居中显示（旁白模式） | 否 |

```json
{"type": "talk", "talker": "小夜", "talk_content": "你好。"}
{"type": "talk", "is_center": "1", "talk_content": "（她微笑了一下。）"}
```

#### `background` — 背景切换

| 字段 | 说明 |
|---|---|
| `file_path` | 背景的资源枚举名 |
| `wait_tween_end` | `"1"` 等待切换动画完成 |
| `delay` | 延迟秒数 |

```json
{"type": "background", "file_path": "BgTest0", "wait_tween_end": "1"}
```

#### `tachie` — 立绘

| 操作 | type | 效果 |
|---|---|---|
| 登场 | `"show"` 或省略 | 自动分配左右站位 |
| 换表情 | `"change"` | 原地淡入淡出切换 |
| 离场 | `"close"` | 移除立绘 |
| 聚光灯 | `"onlyShow"` | 单独居中突出显示 |

```json
{"type": "tachie", "tachies": {"saya": {"file_path": "TachiTest0"}}}
{"type": "tachie", "tachies": {"saya": {"file_path": "TachiTest0", "type": "change"}}}
{"type": "tachie", "tachies": {"saya": {"type": "close"}}}
{"type": "tachie", "tachies": {"saya": {"file_path": "TachiTest0", "type": "onlyShow"}}}

// 多人同框
{"type": "tachie", "tachies": {
  "saya": {"file_path": "TachiTest0", "type": "show", "slot": "Left"},
  "kenta": {"file_path": "TachiTest0", "type": "show", "slot": "Right"}
}}
```

#### `sound` — 音频

```json
{"type": "sound", "sound_type": "bgm", "file_path": "BgmTest0"}
{"type": "sound", "sound_type": "oneSound", "file_path": "SfxTest0"}
```

`sound_type`：`"bgm"` 背景音乐 / `"oneSound"` 音效。

#### `branch` — 分支选项

```json
{"type": "branch", "options": {
  "help": {"text": "帮助她"},
  "leave": {"text": "离开", "wait": "2.0"}
}}
```

玩家选择后，后续带 `branch` 字段的命令只在该分支中执行：

```json
{"type": "talk", "talk_content": "谢谢你！", "branch": "help"}
{"type": "goto", "file_path": "next_chapter", "branch": "help"}
```

#### `goto` — 跳转到另一个 JSON

```json
{"type": "goto", "file_path": "Chapter2"}
```

#### `event` — 自定义事件

```json
{"type": "event", "event_name": "alarm_bell"}
```

（由开发者编写对应的 View 订阅该事件处理特殊逻辑。）

### 3. 公共字段

所有命令都支持：

| 字段 | 说明 |
|---|---|
| `branch` | 所属分支 ID（有了此字段，该命令只在选了对应分支时执行） |
| `wait` | 执行后等待秒数 |
| `hide_labels` | `"1"` 表示执行前隐藏对话栏 |

### 4. 注册 JSON

告诉开发者你的 JSON 文件名，他们在 `StoryPage.Dependencies.cs` 中注册：

```csharp
StoryEngine.RegisterJson("Chapter1", "res://assets/story/Chapter1.json");
```

---

## 开发者篇：扩展框架

### 架构速览

```
Scene(StoryPage) → Command → System(ISystem) → Model + Event → View(CanvasLayer)
```

| 层 | 在哪 | 干什么 |
|---|---|---|
| Scene | `scripts/menu/` | 持有 View 节点，信号→Command |
| System | `scripts/system/` | 业务逻辑 |
| Model | `scripts/model/` | 纯数据 |
| View | `scripts/entities/` | CanvasLayer，订阅 Event 渲染 UI |

### 添加新命令类型

以 `camera_shake` 为例：

**1. 创建 JSON 数据类** — `scripts/cqrs/visualnovel/command/CameraShakeCommand.cs`

```csharp
public sealed class CameraShakeCommand : StoryCommand
{
    public float Intensity { get; set; }
    public static CameraShakeCommand FromJson(JsonElement element)
    {
        return new CameraShakeCommand
        {
            Intensity = StoryParser.GetFloat(element, "intensity") ?? 10f
        };
    }
}
```

**2. 注册到 Parser** — `StoryParser.ParseCommand()` 加一行

```csharp
"camera_shake" => CameraShakeCommand.FromJson(element),
```

**3. 在 PlayLoop 中分发** — `StoryEngine.PlayLoop()` switch 加 case

```csharp
case "camera_shake":
    var shake = (CameraShakeCommand)cmd;
    this.GetSystem<CameraSystem>().Play(
        new EarthquakeEffect { Intensity = shake.Intensity, Duration = 1f });
    break;
```

> 复用已有 System 则不需新建 Command/Event。

### 新建 View

如果要加一个 CG 画廊 View：

```
entities/cg_gallery_view/
  CgGalleryView.cs             — _Ready() { _ = ReadyAsync(); RegisterEvent(); }
  CgGalleryView.Dependencies.cs — GetNode<%> + ReadyAsync()
  CgGalleryView.Events.cs      — RegisterEvent() + OnXxxEvent()
```

在 `StoryPage` 场景中挂一个 `CgGalleryView` 节点。

### 添加新资源

**纹理：** `TextureKey` 枚举加值 → `GameEntryPoint.TextureConfigs` 数组加配置。

**音频：** `SoundKey` 枚举加值 → `GameEntryPoint.SoundConfigs` 数组加配置。

---

## 运行测试故事

### 1. 配置资源

在 Godot 编辑器打开 `global/game_entry_point.tscn`，选中 `GameEntryPoint` 节点，在 Inspector 中：

**TextureConfigs（纹理）：**

| TextureKey | Texture |
|---|---|
| `BgTest0` | `res://assets/texture/background/background_test_0.png` |
| `BgTest1` | `res://assets/texture/background/background_test_1.png` |
| `TachiTest0` | `res://assets/texture/tachie/tachi_test_0.png` |

**SoundConfigs（音频）：**

| SoundKey | Stream |
|---|---|
| `BgmTest0` | `res://assets/sound/bgm/bgm_test_0.mp3` |

### 2. 运行

打开 `scenes/story_page.tscn` → **F6** 运行。

StoryPage 会：
1. 注册所有测试 JSON 脚本
2. 自动播放 `test_prologue`

点击画面推进对话，分支时点击选项按钮。
