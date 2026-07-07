# AVG 视觉小说引擎架构

基于 `yrdk.ymzc` 项目的 JSON 驱动对话系统，以 GFramework CQRS 风格实现的通用视觉小说引擎。

## 核心理念

```
JSON 脚本 → StoryParser → StoryCommand[] → StoryEngineSystem.PlayLoop
                                                  ↓
                                          Command → System → Event → Controller
                                                  ↓
                                          StoryStateModel（读写状态）
```

故事内容以 **JSON 脚本** 编写，通过 `StoryResourceMapper` 映射逻辑名到文件路径，解析为强类型 `StoryCommand` 对象，`StoryEngineSystem` 循环执行，**通过 CQRS 命令驱动各独立 System，System 发送事件，Controller 订阅事件更新 UI**。

## 架构演进

| 版本 | 特点 |
|------|------|
| 原始（yrdk.ymzc） | GDScript `command_parser.gd` + 7 个 worker，autoload 单例 |
| AVG-CQRS 初版 | C# 移植，`IStoryExecutionSystem` 接口 + `_executors` 字典分发 |
| 重构后（当前） | 纯 `switch` 分发，`Command → System → Event → Controller` 统一模式 |

## 命令类型（7 种）

| 类型 | 用途 | 驱动方式 |
|------|------|---------|
| `talk` | 对话文本 + 说话人 + 头像 | `ChangeTalkCommand` (async) → `TalkSystem.PlayAsync` → 等待点击 |
| `background` | 背景切换（淡入淡出） | `ChangeBackgroundCommand` (async) → `BackgroundSystem.Change` → 等待 Tween |
| `tachie` | 立绘管理（显式槽位） | `ChangeTachieCommand` (sync) → `TachieSystem.Apply` |
| `sound` | 音效/音乐 | `SendEvent(VisualNovelSoundPlayedEvent)` 直接内联 |
| `branch` | 分支选项 + 等待选择 | `ChangeBranchCommand` (async) → `BranchSystem.ShowAsync` |
| `goto` | 跳转到另一脚本 | `ChangeGotoCommand` (async) → `GotoSystem.Navigate` + 写 Model |
| `event` | 自定义事件 + 等待点击 | `SendEvent(VisualNovelCustomEventFiredEvent)` + `WaitClickAsync` 直接内联 |

## PlayLoop 分发

```csharp
switch (cmd.Type)
{
    case "background": await SendCommandAsync(ChangeBackgroundCommand);
    case "tachie":     SendCommand(ChangeTachieCommand);
    case "talk":       await SendCommandAsync(ChangeTalkCommand);
    case "branch":     await SendCommandAsync(ChangeBranchCommand);
    case "goto":       await SendCommandAsync(ChangeGotoCommand);
    case "sound":      SendEvent(VisualNovelSoundPlayedEvent);
    case "event":      SendEvent(VisualNovelCustomEventFiredEvent) + await WaitClickAsync;
}
```

## 分支系统

- `TalkBranch[]`：玩家已选择的分支 ID 列表，存储在 `StoryStateModel` 中
- `CanNotChoose[]`：已被禁用的分支 ID 列表
- 分支流程：`branch` 命令 → `BranchSystem.ShowAsync` → 等待 `VisualNovelBranchChosenEvent` → 写入 Model 的 TalkBranch
- 每条命令可标注 `branch` 字段，`ShouldExecute` 从 Model 读取 TalkBranch 做过滤
- `goto` 命令支持 `branch` 字段实现条件跳转

## 架构统一模式

所有 VN 子系统遵循同一模式：

```
Command（CQRS 层）→ System（业务逻辑层）→ SendEvent → Controller（View 层）
```

### 典型调用链

```
StoryEngineSystem.PlayLoop()
  → case "background":
    → await SendCommandAsync(ChangeBackgroundCommand(input))
      → BackgroundSystem.Change(filePath, waitTweenEnd, delay)
        → delay → SendEvent(VisualNovelBackgroundChangedEvent)
          → StoryPage 事件订阅 → BackgroundController.Change()
```

## 子系统清单

| 系统 | 文件 | 独立？ | 职责 |
|------|------|--------|------|
| `StoryEngineSystem` | `system/visualnovel/` | — | JSON 解释器，PlayLoop 驱动 |
| `BackgroundSystem` | 同上 | ✅ | 背景切换 |
| `TachieSystem` | 同上 | ✅ | 立绘管理（显式槽位） |
| `TalkSystem` | 同上 | ✅ | 对话播放 + 等待点击 |
| `BranchSystem` | 同上 | ✅ | 分支选项 |
| `GotoSystem` | 同上 | ✅ | 脚本跳转 |
| `SoundSystem` | 同上 | ✅ | BGM/SFX |
| `EventSystem` | 同上 | ✅ | 自定义事件 |
| `CameraSystem` | 同上 | ✅ | 相机效果 |
| `SaveSystem` | 同上 | ✅ | 存档管理 |

## 资源映射层

`StoryResourceMapper`（静态类）管理逻辑名到文件路径的映射：

```csharp
// JSON 脚本映射
StoryResourceMapper.RegisterJson("Chapter1", "res://assets/story/Chapter1.json");
StoryResourceMapper.RegisterJson("Option1", "res://assets/story/Option1.json");

// 纹理映射
StoryResourceMapper.RegisterTexture("bg_room", "res://assets/texture/background/room.png");
StoryResourceMapper.RegisterTexture("char_smile", "res://assets/texture/tachie/char_smile.png");
```

## 存档系统

`SaveSystem` 序列化 `StoryStateModel` 的完整状态：
- `PlayingJson`：当前脚本文件路径
- `CurrentIndex`：命令索引
- `TalkBranch` / `CanNotChoose`：分支状态
- `PendingGoto`：待跳转目标
