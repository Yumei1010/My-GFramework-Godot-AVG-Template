# My GFramework Godot Template

基于 [GFramework](https://github.com/GeWuYou/GFramework) (v0.0.177) 的 Godot 4.6 项目起手模板，**经 [Twenty-four](https://github.com/Yumei1010/Twenty-four) 项目重度实战魔改后反向提炼**，贴合个人使用习惯。

## 项目渊源

```
GFramework（上游 CQRS/ECS 框架）
    ↓
Twenty-four（24 点游戏，重度魔改 GFramework 用法）
    ↓
My-GFramework-Godot-Template（从 Twenty-four 剥离业务逻辑，保留骨架）
```

本模板并非 GFramework 官方模板，与上游 [GFramework-Godot-Template](https://github.com/GeWuYou/GFramework-Godot-Template) 出发点相似但走向不同。Twenty-four 项目在大量实战中沉淀下来的**个人偏好**——DI 模块划分、partial class 拆分粒度、CQRS 事件/命令的 `sealed` + `init` 约束、XML 注释规范、Godot 节点注入模式等——都保留在了这套骨架里。

## 技术栈

- **引擎：** Godot 4.6 (.NET)
- **运行时：** .NET 10
- **框架：** GFramework 0.0.177（NuGet: `GeWuYou.GFramework`）
- **语言：** C# (LangVersion preview)

## 命名规范

1. 变量名：首字母小写，驼峰命名法
2. 常量名：全大写，下划线分隔
3. 文件夹和文件名：全小写，下划线分隔（snake_case）
4. 命名空间：与目录层次一一对应，`namespace X.Y.Z;`（文件范围声明，无花括号）
5. 提交：`<type>(<scope>): <中文描述>`

详见 [CONVENTIONS.md](CONVENTIONS.md)。

## 文件夹结构

| 目录 | 用途 |
|---|---|
| `assets/` | 游戏资源：字体、Shader、音频/数据占位目录 |
| `global/` | Godot 自动加载单例（GameEntryPoint、UiRoot、SceneRoot 等） |
| `resource/` | Godot 资源文件（音频总线布局、主题） |
| `scenes/` | 主场景 `main.tscn` 及组件场景 |
| `script_templates/` | Godot 自定义脚本模板（Controller / Page / Model） |
| `scripts/core/` | 框架核心：架构引导、状态机、UI/场景路由、配置资源 |
| `scripts/module/` | DI 模块安装（Model / System / Utility / State） |
| `scripts/cqrs/` | CQRS 命令/事件/查询，按域分目录 |
| `scripts/component/` | 可复用组件（VolumeContainer、IState 等） |
| `scripts/enums/` | 枚举定义（UiKey、SceneKey、TextureKey、InputPhase） |
| `scripts/constants/` | 全局常量（GameConstants、UiLayers） |
| `scripts/utility/` | 工具类（纹理注册表、存储接口） |
| `scripts/data/` | 数据层（设置位置提供者） |

## 骨架包含

| 层级 | 内容 |
|---|---|
| DI 引导 | `GameArchitecture` + 4 模块（Model / System / Utility / State） |
| 路由 | `UiRouter`、`SceneRouter`、`UiFactory` |
| 状态机 | `GameStateMachineSystem` + `AppState` 示例 |
| 全局节点 | `GameEntryPoint`、`UiRoot`、`SceneRoot`、`SceneTransitionManager` |
| CQRS 示例 | 音量控制、分辨率/全屏切换、设置存取、退出游戏 |
| 通用组件 | `VolumeContainer`、`IState` |
| 编码模板 | `script_templates/` 下 3 个 Godot 脚本模板 |
| 编码规范 | `CONVENTIONS.md` — 命名空间、CQRS、partial class、XML 注释等全套约束 |
| CI 审查 | TruffleHog 密钥扫描 + CodeQL 静态分析 + .NET 构建 + 自动版本标签 |

## 快速开始

点击仓库首页 **"Use this template"** 按钮即可从本模板创建新仓库，或：

```bash
git clone https://github.com/Yumei1010/My-GFramework-Godot-Template.git MyNewProject
cd MyNewProject
dotnet build
# 用 Godot 打开 project.godot 即可开始开发
```

## 与新项目对接

1. 全局替换命名空间 `GFrameworkTemplate` → 你的项目名
2. 重命名 `.csproj`、`.sln` 文件
3. 在 `scripts/enums/ui/UiKey.cs` 中添加你的 UI 页面键
4. 在 `scripts/enums/scene/SceneKey.cs` 中添加你的场景键
5. 在 `scripts/core/state/impls/` 下创建你的状态
6. 在 `scripts/module/StateModule.cs` 中注册新状态
7. 开始在 `scripts/cqrs/`、`scripts/entities/`、`scripts/menu/` 下添加业务代码

## CI/CD 工作流

| 工作流 | 触发条件 | 用途 |
|---|---|---|
| `ci.yml` | push / PR 到 main | TruffleHog 密钥扫描 + .NET 10 构建 |
| `codeql.yml` | push / PR / 每日 2:00 | CodeQL C# 静态安全分析 |
| `auto-tag.yml` | push 到 main | 自动递增修订号并打 tag（需配置 `PAT_TOKEN`） |
| `release.yml` | 手动触发 | Godot 导出 Win/Linux/macOS + 创建 GitHub Release |

启用自动标签：在仓库 Settings → Secrets and variables → Actions 中添加 `PAT_TOKEN`（`contents: write` 权限的 Personal Access Token）。

## 框架文档

- [GFramework 官方文档](https://gewuyou.github.io/GFramework)
- [CLAUDE.md](CLAUDE.md) — Claude Code 使用指导

## 许可证

### 源代码
本项目源代码按 Apache License 2.0 许可。详见 [LICENSE](LICENSE)。

### 资源
所有资源（包括但不限于美术、音频、字体）不受 Apache 2.0 约束。除非另有说明，所有资源均为 © 作者所有，未经明确许可不得使用。
