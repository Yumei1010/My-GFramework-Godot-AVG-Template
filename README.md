# My-GFramework-Godot-AVG-Template

基于 [GFramework](https://github.com/GeWuYou/GFramework) (v0.0.177) 的 Godot 4.6 视觉小说游戏框架模板。

## 项目渊源

```
GFramework（上游 CQRS/ECS 框架）
    ↓
Twenty-four（24 点游戏，重度魔改 GFramework 用法）
    ↓
My-GFramework-Godot-Template（从 Twenty-four 剥离业务逻辑，保留骨架）
    ↓
My-GFramework-Godot-AVG-Template（基于骨架特化的视觉小说框架）
```

## 技术栈

- **引擎：** Godot 4.6 (.NET)
- **运行时：** .NET 10
- **框架：** GFramework 0.0.177
- **语言：** C#

## 快速上手

### 编剧（不需要写代码）

1. 在 `assets/story/` 下创建 `.json` 文件
2. 用 7 种命令类型编写脚本（`talk` / `background` / `tachie` / `sound` / `branch` / `goto` / `event`）
3. 在 `StoryPage.Dependencies.cs` 注册脚本
4. 运行即看效果

详见 [GETTING_STARTED.md](GETTING_STARTED.md)。

### 开发者

```bash
git clone https://github.com/Yumei1010/My-GFramework-Godot-AVG-Template.git MyVNProject
cd MyVNProject
dotnet build
# 用 Godot 打开 project.godot
```

## 架构

四层 MVC：

```
Scene (StoryPage) → Command → System (ISystem) → Model + Event → View (CanvasLayer)
```

| 层 | 目录 | 职责 |
|---|---|---|
| Scene | `scripts/menu/` | 持有 View，信号→Command 桥接 |
| System | `scripts/system/` | 业务逻辑，ISystem 实现 |
| View | `scripts/entities/` | CanvasLayer，订阅 Event 渲染 UI |
| Model | `scripts/model/` | 纯数据结构 |

详见 [ARCHITECTURE.md](ARCHITECTURE.md)。

## 文件夹结构

```
assets/story/           JSON 故事脚本
assets/texture/         纹理素材（背景、立绘）
assets/sound/           音频素材（bgm、sfx）
global/                 Godot 单例（GameEntryPoint、StoryEngine、UiRoot 等）
scripts/core/           框架核心（GameArchitecture、UiRouter、StoryParser）
scripts/cqrs/           CQRS 命令/事件/查询（按 talk、tachie、sound 等域分目录）
scripts/system/         10 个 ISystem（TalkSystem、BackgroundSystem 等）
scripts/model/          6 个 Model（StoryState、Talk、Tachie、Camera、Sound）
scripts/entities/       6 个 View（TalkView、TachieView 等 CanvasLayer 渲染节点）
scripts/module/         DI 模块安装
scripts/menu/           UI 页面（StoryPage）
scripts/enums/          枚举（TextureKey、SoundKey、TachieOperation 等）
scripts/utility/        工具接口与实现（IGodotTextureRegistry、IGodotAudioRegistry）
scripts/data/           数据映射（StoryResourceMapper）
scripts/component/      可复用组件（BranchOption、TachieSlot、CameraEffect）
docs/                   文档
```

## 核心系统

| 系统 | 命令 | 说明 |
|---|---|---|
| TalkSystem | `TalkPlayCommand` | 对话播放 + 打字机效果 + 点击推进 |
| BackgroundSystem | `BackgroundChangeCommand` | 背景交叉淡入淡出 |
| TachieSystem | `TachieApplyCommand` | 立绘 show/change/close/onlyShow |
| SoundSystem | `SoundPlayCommand` | BGM 防重复 + SFX 对象池 |
| BranchSystem | `BranchShowCommand` | 分支选项 + 等待选择 |
| GotoSystem | `GotoNavigateCommand` | JSON 脚本跳转 |
| CameraSystem | `CameraAddEffectCommand` | 相机效果叠加 |
| SaveSystem | — | 5 槽位 JSON 存档 |

## 文档

| 文档 | 内容 |
|---|---|
| [GETTING_STARTED.md](GETTING_STARTED.md) | 快速上手（编剧 + 开发者） |
| [ARCHITECTURE.md](ARCHITECTURE.md) | 框架运行流程、类速查表、事件通信表 |
| [CONVENTIONS.md](CONVENTIONS.md) | 编码规范（命名、分层约束、partial 规则） |
| [docs/PLAN.md](docs/PLAN.md) | 下一阶段计划 |
| [docs/tutorial/overview.md](docs/tutorial/overview.md) | 7 章可播放教程大纲 |

## 编码规范

- 命名空间与目录一一对应，`namespace X.Y.Z;` 文件范围声明
- `Command: {System}{操作}Command`（现在时）、`Event: {域}{过去时}Event`
- View 的 partial 拆分：`.cs` / `.Dependencies.cs` / `.Events.cs` / `.Signals.cs`
- 提交格式：`<type>(<scope>): <中文描述>`

详见 [CONVENTIONS.md](CONVENTIONS.md)。

## 框架文档

- [GFramework 官方文档](https://gewuyou.github.io/GFramework)
- [CLAUDE.md](CLAUDE.md) — Claude Code 使用指导

## 许可证

### 源代码
本项目源代码按 Apache License 2.0 许可。详见 [LICENSE](LICENSE)。

### 资源
所有资源（包括但不限于美术、音频、字体）不受 Apache 2.0 约束。除非另有说明，所有资源均为 © 作者所有，未经明确许可不得使用。
