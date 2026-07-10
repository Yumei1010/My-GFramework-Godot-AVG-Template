# VN 框架架构文档

> 分支 `cleanup/simplify-architecture`，Godot 4.6 + GFramework CQRS

---

## 目录

- [分层架构](#分层架构)
- [启动流程](#启动流程)
- [目录结构](#目录结构)
- [命名规范](#命名规范)
- [core/story 解析流程](#corestory-解析流程)
- [故事播放流程](#故事播放流程)
- [各命令类型详解](#各命令类型详解)
- [View 代码规范](#view-代码规范)
- [事件通信表](#事件通信表)
- [类速查表](#类速查表)
- [添加新命令类型](#添加新命令类型)

---

## 分层架构

```
┌──────────┐   SendCommand    ┌──────────┐   SendEvent    ┌──────────┐
│  Scene   │ ───────────────▶ │  System  │ ────────────▶ │   View   │
│(StoryPage)                  │ (ISystem)│               │(CanvasLr)│
│ 信号→    │ ◀─ RegisterEvent │ 业务逻辑 │               │ 纯渲染   │
│ Command  │                  └────┬─────┘               └──────────┘
└──────────┘                       │
                             读写 Model
                                   │
                              ┌────┴─────┐
                              │  Model   │
                              │ (纯数据) │
                              └──────────┘
```

**四层单向流：** Scene → Command → System → Model + Event → View

| 层 | 示例 | 职责 | 约束 |
|---|---|---|---|
| **Scene** | `StoryPage` | 持有 View 节点，Godot 信号→Command 桥接 | `SendCommand`, `RegisterEvent` |
| **System** | `TalkSystem` 等 × 10 | 业务逻辑，编排流程 | `GetModel`, `SendEvent`, `SendCommand`, `GetSystem` |
| **View** | `TalkView` 等 × 6 | 纯 UI 渲染 | 只读 Event 数据。**禁** `GetModel` / `GetSystem` |
| **Model** | `StoryStateModel` 等 × 6 | 纯数据结构 | 仅通过 Command 写、Query 读 |

---

## 启动流程

```
Godot 启动
  ▼ autoload
GameEntryPoint._Ready()
  ├─ new GameArchitecture → Initialize()
  │   ├─ UtilityModule  ─ 注册 Ui/Scene/Texture/AudioRegistry 等工具
  │   ├─ SystemModule   ─ 注册 11 个 ISystem
  │   ├─ ModelModule    ─ 注册 6 个 Model
  │   └─ StateModule    ─ 注册 AppState
  ├─ 绑定 GameContext
  ├─ 从 Export 数组注册 TextureConfigs + SoundConfigs
  └─ CallDeferred → Timing.Prewarm()

  ▼
StoryPage._Ready()
  ├─ await Architecture.WaitUntilReadyAsync()
  ├─ RegisterResources()        ← StoryEngine.RegisterJson(...)
  └─ AutoPlayTestStory()
      GetSystem<StoryEngine>().LoadAndPlay("test_prologue")
```

---

## 目录结构

```
global/
  GameEntryPoint.cs         autoload 入口
  StoryEngine.cs            JSON ↔ Command 顶层调度器（CanvasLayer + ISystem）

scripts/
  core/story/
    StoryCommand.cs         JSON 数据基类（5 个公共字段）
    StoryParser.cs          解析器（Parse + GetString/GetFloat + FillCommon）
  cqrs/
    visualnovel/command/     7 种 JSON 数据类（TalkCommand, BackgroundCommand, ...）
    talk/command/            TalkPlayCommand, TalkSetVisibleCommand, TalkSetRevealedCommand
    talk/event/              TalkPlayedEvent, TalkTextRevealedEvent
    background/command/      BackgroundChangeCommand
    background/event/        BackgroundChangedEvent
    tachie/command/          TachieApplyCommand, TachieAddCommand, TachieChangeCommand, ...
    tachie/event/            TachieUpdatedEvent
    sound/command/           SoundPlayCommand
    sound/event/             SoundPlayedEvent
    branch/command/          BranchShowCommand
    branch/event/            BranchShownEvent, BranchChosenEvent
    goto/command/            GotoNavigateCommand
    goto/event/              GotoNavigatedEvent
    story/command/           StoryAdvanceCommand, StoryLoadCommand, StoryChooseBranchCommand,
                             StoryFireCustomEventCommand, StorySetXxxCommand × 7
    story/event/             StoryLoadedEvent, StoryFinishedEvent, StoryAdvanceRequestedEvent,
                             StoryCustomEventFiredEvent
    camera/command/          CameraAddEffectCommand, CameraClearEffectsCommand
    audio/command/           AudioChangeBgmVolumeCommand, AudioChangeMasterVolumeCommand,
                             AudioChangeSfxVolumeCommand
    audio/event/             AudioVolumeChangedEvent
    game/command/            GameExitCommand
    setting/command/         SettingChangeLanguageCommand, SettingResetAllCommand, SettingSaveCommand
    graphics/command/        GraphicsChangeResolutionCommand, GraphicsToggleFullscreenCommand
  system/                    10 个 ISystem
  model/                     6 个 Model
  entities/                  6 个 View（CanvasLayer 渲染节点）
  menu/story_page/           StoryPage（UI 页面，partial class 拆分）
  module/                    4 个 DI 模块
  data/story/                StoryResourceMapper（JSON 路径映射）
  enums/                     TextureKey, SoundKey, TachieOperation, UiKey, SceneKey
  utility/                   IGodotAudioRegistry, GodotAudioRegistry, IGodotTextureRegistry
```

---

## 命名规范

### Command: `{System}{操作}Command`（现在时）

```
TalkPlayCommand           TachieApplyCommand        BranchShowCommand
SoundPlayCommand          BackgroundChangeCommand    GotoNavigateCommand
StoryAdvanceCommand       StoryLoadCommand           CameraAddEffectCommand
StorySetIndexCommand      TachieAddCommand           TachieRemoveCommand
```

### Event: `{域}{过去时}Event`

```
TalkPlayedEvent           TalkTextRevealedEvent      BackgroundChangedEvent
TachieUpdatedEvent        SoundPlayedEvent            BranchShownEvent
BranchChosenEvent         GotoNavigatedEvent          StoryLoadedEvent
```

### 目录

- Event 文件放在 `event/` 目录（不是 `@event/`）
- C# 命名空间使用 `@event`（`event` 是关键字需转义）

---

## core/story 解析流程

```
JSON 文本
  │
  ▼
StoryParser.Parse(json)
  ├─ JsonDocument.Parse
  ├─ content 数组遍历
  │   └─ ParseCommand(element)
  │       ├─ type 字段 → switch 分发
  │       │   "talk"       → TalkCommand.FromJson(element)
  │       │   "background" → BackgroundCommand.FromJson(element)
  │       │   ...
  │       └─ FillCommon(cmd, element)  ← 填充公共字段
  └─ 返回 List<StoryCommand>
```

**两个文件：**
- `StoryCommand.cs` — 抽象基类，5 个公共字段（Type, Branch, HideLabels, Wait, FilePath）
- `StoryParser.cs` — 静态类，Parse + ParseCommand + FillCommon + GetString/GetFloat

---

## 故事播放流程

```
StoryEngine.LoadAndPlay("test_prologue")
  │
  ├─ StoryResourceMapper.ResolveJsonPath → 文件路径
  ├─ LoadJsonAsync → 读取 JSON
  ├─ StoryParser.Parse → List<StoryCommand>
  │
  ├─ 7 个 StorySetXxxCommand 初始化 Model
  │
  └─ PlayLoop()
       while (IsPlaying && index < count)
         ├─ 分支过滤（ShouldExecute）
         ├─ switch (cmd.Type)
         │   "talk"       → TalkPlayCommand
         │   "background" → BackgroundChangeCommand
         │   "tachie"     → TachieApplyCommand
         │   "branch"     → BranchShowCommand
         │   "goto"       → GotoNavigateCommand
         │   "sound"      → SoundPlayCommand
         │   "event"      → StoryFireCustomEventCommand
         ├─ cmd.Wait → Task.Delay
         └─ PendingGoto → 递归 LoadAndPlay
```

---

## 各命令类型详解

### talk — 对话

```
PlayLoop → TalkPlayCommand → TalkSystem.PlayAsync()
  ├─ TalkPlayedEvent { Talker, Content, IsCenter, RevealSpeed }
  │    └─ TalkView.OnTalkPlayed() — 打字机逐字显示
  └─ await StoryEngine.WaitForAdvance()
       │  点击 → StoryAdvanceCommand → StoryEngine.Advance()
       ├─ 打字未完成 → TalkSystem.RevealAll() → TalkTextRevealedEvent → 显示全文
       └─ 已完成 → WaitSource.TrySetResult → 推进下一条
```

**打字机：** `RichTextLabel.VisibleCharacters` + Tween，速度由 `TalkPlayedEvent.RevealSpeed` 携带。

### background — 背景

```
PlayLoop → BackgroundChangeCommand → BackgroundSystem.Change()
  ├─ BackgroundChangedEvent { FilePath, WaitTweenEnd }
  │    └─ StoryPage → BackgroundView.Change()
  │         └─ IGodotTextureRegistry.Get → Tween 淡入淡出
  └─ waitTweenEnd → await 0.5s
```

### tachie — 立绘

```
PlayLoop → TachieApplyCommand → TachieSystem.Apply()
  ├─ 更新 Model (Chars, SlotToChar)
  └─ TachieUpdatedEvent
       └─ TachieView.OnTachieUpdated()
            ├─ 更新 _slotChars 映射
            ├─ 隐藏所有槽位
            └─ 活跃槽位 IGodotTextureRegistry.Get → CrossfadeSlot
```

### sound — 音频

```
PlayLoop → SoundPlayCommand → SoundSystem.PlayBgm/PlaySfx
  ├─ BGM: 检查 CurrentBgm 防重复
  └─ SoundPlayedEvent
       └─ SoundView.OnSoundPlayed()
            └─ IGodotAudioRegistry.Get → BgmPlayer/SfxPool 播放
```

### branch — 分支

```
PlayLoop → BranchShowCommand → BranchSystem.ShowAsync()
  ├─ BranchShownEvent → BranchView.OnBranchShown() → 动态创建按钮
  └─ await TCS
       │  点击 → StoryChooseBranchCommand → BranchChosenEvent
       └─ BranchSystem → StorySetBranchCommand → TalkBranch.Add(chosenId)
```

### goto — 跳转

```
PlayLoop → GotoNavigateCommand
  ├─ GotoSystem.Navigate() → GotoNavigatedEvent
  ├─ StorySetPlayingCommand { Playing = false }
  └─ StorySetGotoCommand { GotoTarget = path }
       └─ PlayLoop 检测 PendingGoto → LoadAndPlay(target)
```

### event — 自定义事件

```
PlayLoop → StoryFireCustomEventCommand
  ├─ StoryCustomEventFiredEvent → 章节自定义 View 订阅
  └─ await StoryEngine.WaitForAdvance()
```

---

## View 代码规范

对齐 Twenty-four Poker 的 partial class 模式：

```csharp
// .cs — _Ready() 只做调用链
[Log][ContextAware]
public partial class FooView : CanvasLayer
{
    public override void _Ready()
    {
        _ = ReadyAsync();
        RegisterEvent();
    }
}

// .Dependencies.cs — 节点引用 + 初始化
public partial class FooView
{
    private Button MyBtn => GetNode<Button>("%MyBtn");
    private async Task ReadyAsync()
    {
        await GameEntryPoint.Architecture.WaitUntilReadyAsync();
        Layer = 0;
    }
}

// .Events.cs — RegisterEvent + 处理方法
public partial class FooView
{
    private void RegisterEvent()
    {
        this.RegisterEvent<FooEvent>(e => OnFoo(e))
            .UnRegisterWhenNodeExitTree(this);
    }
    private void OnFoo(FooEvent e) { /* 渲染 */ }
}
```

| 后缀 | 职责 | 必须 |
|---|---|---|
| `.cs` | `_Ready()` 调用链 + 公开方法 | 是 |
| `.Dependencies.cs` | `GetNode<%>` 引用 + `ReadyAsync()` 初始化 | 是 |
| `.Events.cs` | `RegisterEvent()` + `OnXxx()` 处理方法 | 有事件时 |
| `.Signals.cs` | `ConnectSignal()` + Godot 信号处理 | 有信号时 |

---

## 事件通信表

| Event | 发送者 | 订阅者 |
|---|---|---|
| `TalkPlayedEvent` | TalkSystem | TalkView |
| `TalkTextRevealedEvent` | TalkSystem | TalkView |
| `BackgroundChangedEvent` | BackgroundSystem | StoryPage → BackgroundView |
| `TachieUpdatedEvent` | TachieSystem | TachieView |
| `SoundPlayedEvent` | SoundSystem | SoundView |
| `BranchShownEvent` | BranchSystem | BranchView |
| `BranchChosenEvent` | StoryChooseBranchCommand | BranchView, BranchSystem |
| `GotoNavigatedEvent` | GotoSystem | —（预留） |
| `StoryLoadedEvent` | StoryEngine | —（日志） |
| `StoryFinishedEvent` | StoryEngine | —（日志） |
| `StoryCustomEventFiredEvent` | StoryFireCustomEventCmd | 章节自定义 View |
| `StoryAdvanceRequestedEvent` | StoryEngine | —（预留） |

### Godot 信号 → CQRS

```
StoryPage.Signals: InputDetector.Pressed → StoryAdvanceCommand → StoryEngine.Advance()
```

---

## 类速查表

| 类 | 位置 | 基类 |
|---|---|---|
| `GameEntryPoint` | `global/` | Node (autoload) |
| `StoryEngine` | `global/StoryEngine.cs` | CanvasLayer + ISystem |
| `StoryCommand` | `scripts/core/story/` | abstract |
| `StoryParser` | `scripts/core/story/` | static |
| `StoryStateModel` | `scripts/model/visualnovel/` | AbstractModel |
| `BackgroundSystem` | `scripts/system/background_system/` | ISystem |
| `BranchSystem` | `scripts/system/branch_system/` | ISystem |
| `CameraSystem` | `scripts/system/camera_system/` | ISystem |
| `GotoSystem` | `scripts/system/goto_system/` | ISystem |
| `SaveSystem` | `scripts/system/save_system/` | ISystem |
| `SoundSystem` | `scripts/system/sound_system/` | ISystem |
| `TachieSystem` | `scripts/system/tachie_system/` | ISystem |
| `TalkSystem` | `scripts/system/talk_system/` | ISystem |
| `BackgroundView` | `scripts/entities/background_view/` | CanvasLayer |
| `BranchView` | `scripts/entities/branch_view/` | CanvasLayer |
| `CameraView` | `scripts/entities/camera_view/` | CanvasLayer |
| `SoundView` | `scripts/entities/sound_view/` | CanvasLayer |
| `TachieView` | `scripts/entities/tachie_view/` | CanvasLayer |
| `TalkView` | `scripts/entities/talk_view/` | CanvasLayer |
| `StoryPage` | `scripts/menu/story_page/` | Control (UI Page) |
| `StoryResourceMapper` | `scripts/data/story/` | static |

---

## 添加新命令类型

以 `camera_shake` 为例，需改 3 处：

1. **JSON 数据类** — `scripts/cqrs/visualnovel/command/CameraShakeCommand.cs`
```csharp
public sealed class CameraShakeCommand : StoryCommand
{
    public float Intensity { get; set; }
    public static CameraShakeCommand FromJson(JsonElement element)
    {
        var cmd = new CameraShakeCommand { Intensity = StoryParser.GetFloat(element, "intensity") ?? 10f };
        // FillCommon 由 StoryParser.ParseCommand 调用
        return cmd;
    }
}
```

2. **注册 Parser** — `StoryParser.ParseCommand()` switch 加一行
```csharp
"camera_shake" => CameraShakeCommand.FromJson(element),
```

3. **PlayLoop 分发** — `StoryEngine.PlayLoop()` switch 加 case
```csharp
case "camera_shake":
    var shake = (CameraShakeCommand)cmd;
    this.GetSystem<CameraSystem>().Play(
        new EarthquakeEffect { Intensity = shake.Intensity, Duration = 1f });
    break;
```

> 复用已有 System 则不需新建 Command/Event。
