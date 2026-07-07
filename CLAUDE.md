# CLAUDE.md

本文档为 Claude Code (claude.ai/code) 在基于本框架模板构建的项目中工作时提供指导。

项目编码规范详见 [CONVENTIONS.md](CONVENTIONS.md)，涵盖命名空间、文件结构、CQRS 约定、XML 注释标准、修饰符规范等。以下为关键要点速查与架构概览。

## 构建与测试

```bash
# 构建项目（需要 Godot .NET SDK 4.6）
dotnet build

# 运行全部测试
dotnet test
```

测试使用 xUnit，测试项目位于 `tests/` 目录下。

## 关键约束速查

以下为 CONVENTIONS.md 的核心规则，开发时务必遵守：

- **命名空间** 与目录一对一映射，使用文件范围声明 `namespace X.Y.Z;`（无大括号）；C# 关键字 `event`/`goto` 在命名空间中转义为 `@event`/`@goto`
- **事件** 全部 `public sealed class`；属性全部 `{ get; init; }` + `required`；无数据事件用分号声明 `public sealed class FooEvent;`
- **命令** 全部 `public sealed class`，继承 `AbstractCommand(Async)`；命令输入全部 `sealed class : ICommandInput`（**禁止 struct**）
- **Godot 节点** 全部 `public partial class`（不 sealed）；标注 `[Log]` + `[ContextAware]`（成对，`[Log]` 在前）
- **节点引用** 在 `.Dependencies.cs` 中用 `GetNode<T>("%NodeName")`，使用接口类型（若存在）
- **UI 页面** 不需要提取 `I*` 接口（由 UiRouter 管理，不被其他组件消费）
- **XML 注释** 中文；接口/公开方法/事件/命令必须有 `<summary>`；私有方法按需
- **提交** 格式 `<type>(<scope>): <中文描述>`，每次提交为逻辑独立的原子操作

## 提交规则

- 每次提交必须是逻辑上独立的原子操作。
- **遇到复杂变更时必须分析**：如果一次对话的修改混杂了不同功能、bug 修复或优化，必须主动分析其原子性。
- **按组件或职责拆分**：例如，对 API 格式的调整与对 UI 样式的修改应分开提交。
- **主动建议**：分析后，生成一个包含多个提交的"群组提案"，而不是把所有东西一股脑儿塞进一个提交。

## 架构

**技术栈：** Godot 4.6 + C# (.NET 10) + GFramework (0.0.177) — NuGet 上的 CQRS/ECS 框架。

**DI 引导：** `global/GameEntryPoint`（自动加载单例）创建 `GameArchitecture`，安装 4 个模块：

| 模块 | 职责 |
|---|---|
| `ModelModule` | 注册设置模型及其应用器 + VN 领域模型（StoryState/Camera/Talk/Tachie/Sound） |
| `SystemModule` | 注册 UiRouter、SceneRouter、SettingsSystem + 10 个 VN 子系统 |
| `UtilityModule` | 注册工具：UI/场景/纹理注册表、存储、序列化、工厂 |
| `StateModule` | 注册 `GameStateMachineSystem` + `AppState` |

**状态 → UI 映射：** 每个状态实现 `ContextAwareStateBase`，在 `OnEnter` 中清除之前的 UI/场景，并通过 `UiRouter.Push()` 推入对应的 UI 页面。参见 `AppState` 作为模板示例。

**UI 页面** 继承 `Control`，实现 `IUiPageBehaviorProvider` + `ISimpleUiPage`。采用 partial class 模式：

| Partial 文件 | 用途 |
|---|---|
| `*.cs` | 核心：`_Ready()` 调用 `ReadyAsync()` → `ConnectSignal()` → `RegisterEvent()` |
| `*.Dependencies.cs` | Godot 节点引用（`%NodeName`）、`ReadyAsync()` 初始化逻辑 |
| `*.Properties.cs` | 字段、属性、`UiKeyStr` |
| `*.Events.cs` | 通过 `RegisterEvent()` 订阅 CQRS 事件 |
| `*.Signals.cs` | Godot 信号 → CQRS 事件桥接（`ConnectSignal()`） |

## 核心模式

### CQRS 通信
组件通过 GFramework 事件通信，而非 Godot 信号：
- **发送：** `this.SendEvent(new SomeEvent { ... })`（触发所有已注册的处理程序）
- **订阅：** 在 `RegisterEvent()` 内使用 `this.RegisterEvent<SomeEvent>(e => { ... })`，并以 `.UnRegisterWhenNodeExitTree(this)` 链式调用确保节点退出时自动注销
- 事件位于 `scripts/cqrs/<domain>/event/`，命名空间 `...cqrs.<domain>.@event`
- 命令位于 `scripts/cqrs/<domain>/command/`，命令输入位于 `scripts/cqrs/<domain>/command/input/`
- 命令发送：`this.SendCommand(new SomeCommand(input))`
- 异步命令：`await this.SendCommandAsync(new SomeCommand(input))`

### 日志与上下文
- `[Log]` 特性通过 GFramework 源代码生成器自动生成静态 `Log` 属性
- `[ContextAware]` 特性自动注入 GFramework 架构上下文
- 两者**成对使用**，`[Log]` 在前

## VN 故事引擎

### 命令类型（7 种）

| 类型 | 用途 | 驱动方式 |
|------|------|---------|
| `talk` | 对话文本 + 说话人 | `ChangeTalkCommand` → `TalkSystem.PlayAsync` |
| `background` | 背景切换（淡入淡出） | `ChangeBackgroundCommand` → `BackgroundSystem.Change` |
| `tachie` | 立绘管理（显式槽位） | `ChangeTachieCommand` → `TachieSystem.Apply` |
| `sound` | 音效/音乐 | 直接发送 `VisualNovelSoundPlayedEvent` |
| `branch` | 分支选项 + 等待选择 | `ChangeBranchCommand` → `BranchSystem.ShowAsync` |
| `goto` | 跳转到另一脚本 | `ChangeGotoCommand` → `GotoSystem.Navigate` + 写 Model |
| `event` | 自定义事件 + 等待点击 | 直接发送 `VisualNovelCustomEventFiredEvent` + `WaitClickAsync` |

### PlayLoop 架构

```
PlayLoop (纯 switch 分发)
  ├── "background" → await SendCommandAsync(ChangeBackgroundCommand)
  ├── "tachie"     → SendCommand(ChangeTachieCommand)
  ├── "talk"       → await SendCommandAsync(ChangeTalkCommand)
  ├── "branch"     → await SendCommandAsync(ChangeBranchCommand)
  ├── "goto"       → await SendCommandAsync(ChangeGotoCommand)
  ├── "sound"      → SendEvent(VisualNovelSoundPlayedEvent)
  └── "event"      → SendEvent + await WaitClickAsync

统一模式: Command → System → SendEvent → Controller（View 层）
```

### JSON 故事格式

详见 [STORY_FORMAT.md](STORY_FORMAT.md)。JSON 脚本通过 `StoryParser` 解析为 `StoryCommand` 对象，`StoryEngineSystem.LoadAndPlay(logicName)` 启动播放。纹理/JSON 逻辑名通过 `StoryResourceMapper.Register*` 注册。

## 框架模块清单

| 模块 | 文件 | 注册内容 |
|---|---|---|
| ModelModule | `scripts/module/ModelModule.cs` | `SettingsModel` + VN 领域模型（5 个） |
| SystemModule | `scripts/module/SystemModule.cs` | UiRouter、SceneRouter、SettingsSystem + 10 个 VN 子系统 |
| UtilityModule | `scripts/module/UtilityModule.cs` | UI/场景/纹理注册表、存储、JSON 序列化、设置仓储 |
| StateModule | `scripts/module/StateModule.cs` | `GameStateMachineSystem` + `AppState` |

## VN 子系统清单

| 系统 | 职责 | 命令入口 |
|------|------|---------|
| `StoryEngineSystem` | JSON 脚本解释器，PlayLoop 驱动 | `LoadAndPlay` |
| `BackgroundSystem` | 背景切换（延迟 + 事件） | `ChangeBackgroundCommand` |
| `TachieSystem` | 立绘管理（显式槽位支持） | `ChangeTachieCommand` |
| `TalkSystem` | 对话播放 + 等待点击推进 | `ChangeTalkCommand` |
| `BranchSystem` | 分支选项 + 等待玩家选择 | `ChangeBranchCommand` |
| `GotoSystem` | 跳转导航（发事件 + 写 Model） | `ChangeGotoCommand` |
| `SoundSystem` | BGM/SFX 管理 | — |
| `EventSystem` | 自定义事件系统 | — |
| `CameraSystem` | 相机效果 | — |
| `SaveSystem` | 存档管理 | — |

## 目录结构约定

```
scripts/
├── component/       # 可复用组件（Controller 场景节点）
├── constants/       # 全局常量
├── core/            # 框架核心（架构、路由、状态、故事解析）
├── cqrs/            # CQRS 命令/事件/查询（按域划分）
├── data/            # 数据层（资源映射、设置存储）
├── enums/           # 枚举（UI Key、场景 Key、立绘操作等）
├── model/           # 领域模型（按业务域划分）
├── module/          # DI 模块
├── system/          # 系统（按业务域划分，统一在 visualnovel/ 下）
├── menu/            # UI 页面
└── tool/            # 开发工具脚本
```
