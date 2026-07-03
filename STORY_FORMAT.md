# VN 故事 JSON 格式文档

## 快速上手

创建一个 `.json` 文件放入 `resource/story/` 目录，然后在控制器中注册：

```csharp
StoryEngineSystem.RegisterJson("MyStory", "res://resource/story/MyStory.json");
_ = engine.LoadAndPlay("MyStory");
```

## JSON 结构

```json
{
  "content": [
    // 命令列表，按顺序执行
  ]
}
```

## 七种命令类型

### 1. `talk` —— 对话 / 旁白

| 字段 | 类型 | 必填 | 说明 |
|------|------|------|------|
| `type` | string | ✅ | `"talk"` |
| `talk_content` | string | ✅ | 对话/旁白文本 |
| `talker` | string | 否 | 说话人姓名（不填=旁白） |
| `is_center` | `"1"` | 否 | 设为 `"1"` 表示居中旁白（隐藏姓名头像） |
| `avatar_path` | string | 否 | 头像逻辑名（需注册纹理映射） |

**示例：**
```json
// 角色对话
{"type": "talk", "talker": "小夜", "talk_content": "你好。", "avatar_path": "saya"}

// 居中旁白（无说话人）
{"type": "talk", "is_center": "1", "talk_content": "（窗外传来鸟鸣声。）"}
```

### 2. `background` —— 背景切换

| 字段 | 类型 | 说明 |
|------|------|------|
| `type` | string | `"background"` |
| `file_path` | string | 背景逻辑名 |
| `wait_tween_end` | `"1"` | 是否等待交叉淡入淡出完成 |
| `delay` | string | 延迟秒数 |

**示例：**
```json
{"type": "background", "file_path": "bg_classroom", "wait_tween_end": "1", "delay": "0.5"}
```

### 3. `tachie` —— 立绘管理

| 字段 | 类型 | 说明 |
|------|------|------|
| `type` | string | `"tachie"` |
| `tachies` | object | `{角色名: {file_path, type}}` |

**操作类型（type）：**

| type 值 | 效果 | 是否可省略 |
|---------|------|-----------|
| `show` | 入场到左右槽位 | ✅ 默认（省略等于 show） |
| `change` | 原位交叉淡入淡出换表情 | 否 |
| `close` | 离场 | 否 |
| `onlyShow` | 居中聚光灯凸显（独占 Center 槽位） | 否 |

**示例：**
```json
// 登场（type 可省略）
{"type": "tachie", "tachies": {"saya": {"file_path": "saya_normal"}}}

// 切换表情
{"type": "tachie", "tachies": {"saya": {"file_path": "saya_smile", "type": "change"}}}

// 离场
{"type": "tachie", "tachies": {"saya": {"type": "close"}}}

// 居中聚光灯
{"type": "tachie", "tachies": {"saya": {"file_path": "saya_cry", "type": "onlyShow"}}}

// 批量操作
{"type": "tachie", "tachies": {
  "saya": {"file_path": "saya_normal", "type": "show"},
  "kenta": {"file_path": "kenta_excited", "type": "show"}
}}
```

### 4. `sound` —— 音频

| 字段 | 类型 | 说明 |
|------|------|------|
| `type` | string | `"sound"` |
| `sound_type` | string | `"bgm"` 背景音乐 / `"oneSound"` 音效 |
| `file_path` | string | 音频逻辑名 |

**示例：**
```json
{"type": "sound", "sound_type": "bgm", "file_path": "bgm_morning"}
{"type": "sound", "sound_type": "oneSound", "file_path": "sfx_door_open"}
```

### 5. `branch` —— 分支选项

| 字段 | 类型 | 说明 |
|------|------|------|
| `type` | string | `"branch"` |
| `options` | object | `{选项ID: {text, wait?}}` |

**示例：**
```json
{"type": "branch", "options": {
  "1A": {"text": "友好地回应"},
  "1B": {"text": "冷淡地点头"},
  "1C": {"text": "装作没听见", "wait": "2.0"}
}}
```

### 6. `goto` —— 场景跳转

| 字段 | 类型 | 说明 |
|------|------|------|
| `type` | string | `"goto"` |
| `file_path` | string | 目标脚本逻辑名（需已注册） |

**示例：**
```json
{"type": "goto", "file_path": "Chapter2_Opening"}
```

### 7. `event` —— 自定义事件

| 字段 | 类型 | 说明 |
|------|------|------|
| `type` | string | `"event"` |
| `event_name` | string | 事件名（章脚本订阅处理） |

**示例：**
```json
{"type": "event", "event_name": "bell_ring"}
```

## 公共字段

所有命令类型都支持以下可选字段：

| 字段 | 类型 | 说明 |
|------|------|------|
| `branch` | string | 所属分支 ID。有此字段的命令仅在玩家选择了对应分支时才执行 |
| `hide_labels` | `"1"` | 执行前隐藏对话文本 |
| `wait` | string | 执行后等待秒数 |

## 分支过滤

```json
[
  {"type": "talk", "talk_content": "公共对话"},
  {"type": "talk", "talk_content": "分支1A专属", "branch": "1A"},
  {"type": "talk", "talk_content": "分支1B专属", "branch": "1B"},
  {"type": "goto", "file_path": "NextChapter"}
]
```

- 无 `branch` 字段的命令 → 所有人可见
- 有 `branch` 字段的命令 → 仅在匹配时执行，否则跳过

## 注册纹理/音频映射

```csharp
// 在章脚本 _Ready() 中注册
StoryResourceMapper.RegisterTexture("saya_normal", "res://assets/texture/tachie/saya_normal.png");
StoryResourceMapper.RegisterTexture("bg_classroom", "res://assets/texture/background/classroom.png");
```

## 完整示例

参见 `resource/story/chapter1/Chapter1_Prologue.json`
