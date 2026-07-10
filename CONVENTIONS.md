# 项目约束规范

本文档定义基于本 VN 框架模板的项目的编码规范、架构约束和命名约定。

---

## 1. 命名空间规范

命名空间与目录层次一一对应，文件范围声明（`namespace X.Y.Z;`）。

```
根命名空间: GFrameworkTemplate

global/                              → GFrameworkTemplate.global;
scripts/component/<name>/            → GFrameworkTemplate.scripts.component.<name>;
scripts/core/<dir>/                  → GFrameworkTemplate.scripts.core.<dir>;
scripts/cqrs/<domain>/command/       → GFrameworkTemplate.scripts.cqrs.<domain>.command;
scripts/cqrs/<domain>/command/input/ → GFrameworkTemplate.scripts.cqrs.<domain>.command.input;
scripts/cqrs/<domain>/event/        → GFrameworkTemplate.scripts.cqrs.<domain>.@event;
scripts/cqrs/visualnovel/command/    → GFrameworkTemplate.scripts.cqrs.visualnovel.command;
scripts/data/<domain>/              → GFrameworkTemplate.scripts.data.<domain>;
scripts/entities/<view_name>/       → GFrameworkTemplate.scripts.entities.<view_name>;
scripts/enums/<domain>/             → GFrameworkTemplate.scripts.enums.<domain>;
scripts/menu/<page_name>/           → GFrameworkTemplate.scripts.menu.<page_name>;
scripts/model/<domain>/             → GFrameworkTemplate.scripts.model.<domain>;
scripts/module/                     → GFrameworkTemplate.scripts.module;
scripts/system/<name>/              → GFrameworkTemplate.scripts.system.<name>;
scripts/utility/                    → GFrameworkTemplate.scripts.utility;
```

> C# 关键字 `event` / `goto` 在命名空间中转义为 `@event` / `@goto`

---

## 2. 文件与目录结构

### 四层架构下的目录职责

| 目录 | 层 | 存放内容 | 示例 |
|---|---|---|---|
| `global/` | — | Godot 单例（autoload + ISystem） | `GameEntryPoint`, `StoryEngine` |
| `scripts/menu/` | Scene | UI 页面，持有 View 节点 | `StoryPage` |
| `scripts/system/` | System | ISystem 实现，业务逻辑 | `TalkSystem`, `BackgroundSystem` |
| `scripts/entities/` | View | CanvasLayer 渲染节点（*_view） | `TalkView`, `TachieView` |
| `scripts/model/` | Model | AbstractModel 纯数据 | `StoryStateModel`, `TalkModel` |
| `scripts/cqrs/` | — | Command / Event / Query / 数据类 | 按域分子目录 |
| `scripts/core/` | — | 架构核心 | `GameArchitecture`, `UiRouter`, `StoryParser` |
| `scripts/module/` | — | DI 模块安装 | `SystemModule`, `ModelModule` |
| `scripts/enums/` | — | 枚举 | `TextureKey`, `SoundKey`, `UiKey` |
| `scripts/data/` | — | 数据映射与持久化 | `StoryResourceMapper` |
| `scripts/utility/` | — | 工具接口与实现 | `IGodotTextureRegistry` |
| `scripts/component/` | — | 可复用组件与数据类 | `BranchOption`, `TachieSlot`, `CameraEffect` |

### 目录命名

- **小写 + 下划线**（snake_case）
- 例：`background_system`、`story_page`、`tachie_view`

### .uid 文件

每个 `.cs` 文件有对应的 `.cs.uid`（Godot 自动生成），必须纳入版本控制。

---

## 3. 四层分层约束

| 层 | 示例类 | 允许操作 | 禁止操作 |
|---|---|---|---|
| **Scene** | `StoryPage` | `SendCommand`, `RegisterEvent`, `GetNode<%>` | `GetModel`, `GetSystem` |
| **System** | `TalkSystem` | `GetModel` 读写, `SendEvent`, `SendCommand`, `GetSystem` | 直接操作 Godot 节点 |
| **View** | `TalkView` | `RegisterEvent`, `GetNode<%>`, `GetUtility` | `GetModel`, `GetSystem` |
| **Model** | `TalkModel` | 纯字段 | 业务逻辑, `SendEvent` |

**单向流：** Scene → Command → System → Model 写入 + Event 发送 → View 订阅 Event 渲染

- System 和 View 之间通过 **Event 通信**，不直接调用
- View 渲染所需数据由 **Event 携带**，不读 Model

---

## 4. Partial Class 模式（View 节点）

View（`entities/` 下的 CanvasLayer）严格遵循此拆分：

| 后缀 | 职责 | 必须 |
|---|---|---|
| `.cs` | `_Ready()` 调用链：`_ = ReadyAsync(); RegisterEvent();` + 公开方法 | 是 |
| `.Dependencies.cs` | `GetNode<T>("%Name")` 节点引用 + `ReadyAsync()` 全部初始化 | 是 |
| `.Events.cs` | `RegisterEvent()` + `OnXxxEvent()` 私有处理方法 | 有事件时 |
| `.Signals.cs` | `ConnectSignal()` + Godot 信号处理方法 | 有信号时 |

### 示例

```
TalkView.cs
TalkView.Dependencies.cs
TalkView.Events.cs
```

### `.cs` 规则

```csharp
[Log]
[ContextAware]
public partial class TalkView : CanvasLayer
{
    public override void _Ready()
    {
        _ = ReadyAsync();
        RegisterEvent();
    }
}
```

- `[Log]` 和 `[ContextAware]` 仅标注在 `.cs` 文件，partial 文件不加
- `_Ready()` 只做函数调用链，不写业务逻辑

### `.Dependencies.cs` 规则

```csharp
public partial class TalkView
{
    private RichTextLabel ContentLabel => GetNode<RichTextLabel>("%ContentLabel");
    private Tween? _typewriter;

    private async Task ReadyAsync()
    {
        await GameEntryPoint.Architecture.WaitUntilReadyAsync();
        // 初始化逻辑（Layer 赋值、GD.Load、初始显隐等）
    }
}
```

### `.Events.cs` 规则

```csharp
public partial class TalkView
{
    private void RegisterEvent()
    {
        this.RegisterEvent<TalkPlayedEvent>(e => OnTalkPlayed(e))
            .UnRegisterWhenNodeExitTree(this);
    }

    private void OnTalkPlayed(TalkPlayedEvent e) { /* 渲染 */ }
}
```

- `RegisterEvent()` 内 lambdas 调用 `OnXxx()` 方法
- 处理逻辑在 `OnXxx` 方法中

---

## 5. 命名规范

### Command: `{System}{操作}Command`（现在时）

```
TalkPlayCommand           TachieApplyCommand        BranchShowCommand
SoundPlayCommand          BackgroundChangeCommand    GotoNavigateCommand
StoryAdvanceCommand       CameraAddEffectCommand     StorySetIndexCommand
```

Commando 原子化：一个 Command 只做一件事。

### Event: `{域}{过去时}Event`

```
TalkPlayedEvent           BackgroundChangedEvent     TachieUpdatedEvent
SoundPlayedEvent          BranchShownEvent           StoryFinishedEvent
```

### View: `{域}View`

```
TalkView      BackgroundView      TachieView
SoundView     BranchView          CameraView
```

### System: `{域}System`

```
TalkSystem    BackgroundSystem    TachieSystem
SoundSystem   BranchSystem        CameraSystem
```

### 目录名 vs 命名空间

- 文件系统目录：`event/`（不加 `@`）
- C# 命名空间：`@event`（`event` 是 C# 关键字需转义）

---

## 6. CQRS 事件与命令规范

### 事件

- 所有事件 `public sealed class`
- 属性 `{ get; init; }` + `required`
- 标记事件用分号：`public sealed class SomeEvent;`
- 禁止 `{ get; set; }`

### 命令

- 所有命令 `public sealed class`
- 属性 `{ get; set; }` + `required`
- 异步继承 `AbstractAsyncCommand<T>`，同步继承 `AbstractCommand`
- 禁止 `{ get; init; }`

### 命令输入

- `public sealed class`，实现 `ICommandInput`
- 属性 `{ get; set; }`
- 禁止 `struct`

---

## 7. JSON 数据类规范

`scripts/cqrs/visualnovel/command/` 下的是 **JSON 数据类**（不是 CQRS Command）：

- 继承 `StoryCommand`（core/story 中的抽象基类）
- 提供 `static FromJson(JsonElement element)` 工厂方法
- 公共字段从 JSON 解析而来
- 调用 `StoryParser.GetString/GetFloat` 读取 JSON 字段

```csharp
public sealed class TalkCommand : StoryCommand
{
    public string? Talker { get; set; }
    public string TalkContent { get; set; } = string.Empty;
    public bool IsCenter { get; set; }

    public static TalkCommand FromJson(JsonElement element)
    {
        var cmd = new TalkCommand
        {
            Talker = StoryParser.GetString(element, "talker"),
            TalkContent = StoryParser.GetString(element, "talk_content") ?? "",
            IsCenter = StoryParser.GetString(element, "is_center") == "1"
        };
        return cmd;
    }
}
```

> `FillCommon` 由 `StoryParser.ParseCommand` 统一调用，子类不需要调。

---

## 8. 资源注册规范

### 纹理（TextureConfig）

1. `TextureKey` 枚举添加值
2. 在 `game_entry_point.tscn` 的 `GameEntryPoint.TextureConfigs` 数组中添加 `TextureConfig` 资源
3. View 中通过 `this.GetUtility<IGodotTextureRegistry>().Get("KeyName")` 获取

### 音频（SoundConfig）

1. `SoundKey` 枚举添加值
2. 在 `game_entry_point.tscn` 的 `GameEntryPoint.SoundConfigs` 数组中添加 `SoundConfig` 资源
3. SoundView 中通过 `this.GetUtility<IGodotAudioRegistry>().Get("KeyName")` 获取

### JSON 脚本

1. 在 `StoryPage.Dependencies.RegisterResources()` 中调用 `StoryEngine.RegisterJson(name, path)`
2. JSON 中的 `file_path` 使用枚举名而非文件系统路径

---

## 9. 全局引用

`scripts/GlobalUsings.cs` 仅包含：

```csharp
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
global using LanguageExt;
```

- 不添加 Godot / GFramework 命名空间
- 每文件显式导入

---

## 10. 提交规范

- 格式：`<type>(<scope>): <中文描述>`
- 每次提交是逻辑独立的原子操作
- 不同职责的修改拆分提交

| Type | 含义 |
|---|---|
| `feat` | 新功能 |
| `refactor` | 重构 |
| `fix` | Bug 修复 |
| `test` | 测试 |
| `docs` | 文档 |
| `chore` | 构建/工具/杂项 |
| `style` | 代码样式 |

---

## 附录：禁止事项

- ❌ 传统花括号命名空间
- ❌ 事件 `{ get; set; }` / 命令 `{ get; init; }`
- ❌ 命令输入用 `struct`
- ❌ 事件/命令不加 `sealed`
- ❌ 驼峰目录名
- ❌ GlobalUsings 加 Godot/GFramework 引用
- ❌ `_Ready()` 中直写业务逻辑
- ❌ View 层读 Model（`GetModel` / `SendQuery`）
- ❌ JsonCommand 子类中调 `cmd.FillCommon(element)`
- ❌ `--no-verify` 跳钩子
