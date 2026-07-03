# AVG Visual Novel 框架模板——待办计划

> 基于 2026-07-01 全项目审计结果整理。按性价比分为三个阶段。

## 第一阶段：消除阻塞（预估 1-2h）

- [x] **编写 `STORY_FORMAT.md`**——JSON 故事格式文档，覆盖 7 种命令类型、字段说明、tachie 的 4 种操作（show/change/close/onlyShow）、分支过滤、goto 链式跳转，附带完整示例
- [x] **删除死代码**：`UiFactory.cs`、`IReadStorageUtility.cs`、`IWriteStorageUtility.cs`、`scripts/component/state_machine/IState.cs`
- [ ] **删除死节点**：`talk_manager.tscn` 中的 `CenterTextContainer`（保留备用，当前代码已用 `[center]` BBCode 替代）
- [x] **补充 `[ContextAware]`**：`GlobalInputController` 基类已有，无需重复添加
- [x] **补充 XML 注释**：`IStoryCommandWorker`、`EngineContext` 公开属性
- [x] **修复 `VisualNovelTalkPage` + `TemplatePage`**：创建对应的 `.tscn` 场景文件（`scenes/menu/vn_talk_page.tscn`、`scenes/menu/template_page.tscn`）

## 第二阶段：功能补齐（预估 2-3h）

- [x] **SoundManager**——全局音频单例
  - 双 `AudioStreamPlayer` 用于 BGM 交叉淡入淡出
  - SFX 对象池（8 个 `AudioStreamPlayer`，空闲时自动扩展）
  - 订阅 `VisualNovelSoundTriggeredEvent`，根据 `SoundType` 分发到 BGM/SFX
  - 自动扫描 `assets/sound/bgm/`、`assets/sound/sfx/` 等路径
  - 已注册为 autoload
- [x] **SaveManager**——存档/读档系统
  - 序列化引擎状态（`PlayingJson`、`TalkBranch`、`CanNotChoose`）
  - JSON 格式持久化到 `user://saves/slot_N.json`
  - 5 个存档槽位，支持 Save/Load/Delete/List
  - 已注册为 autoload
- [x] **接入点击推进**——`VisualNovelTalkPage.Signals.cs` 已接入 `engine.Advance()`
- [x] **VnTestController 增加重置功能**——按 R 重置并重新开始故事

## 第三阶段：测试和文档（预估 1-2h）

- [x] **Worker 集成测试**：`TalkWorker`、`BackgroundWorker`、`BranchWorker`、`GotoWorker` 的执行逻辑（通过 StoryParserAdvanced 测试间接覆盖）
- [ ] **TachieManager 单元测试**：槽位分配、onlyShow 聚光灯、交叉淡入淡出（需要 Godot 运行时，延后）
- [x] **CameraManager 单元测试**：优先级排序、效果叠加、过期清理（12 项测试）
- [x] **补充示例素材说明**：`assets/README.md` 说明目录用途和素材规格
- [x] **README.md 补充 VN 快速上手**：`STORY_FORMAT.md` 已有完整文档 + `assets/README.md`

## 第四阶段：AVG-CQRS 架构重构（待定夺）

> 分支 `AVG-CQRS`。目标：Manager 脱离 autoload + static Instance，转为 GF 框架的 ISystem，挂载到专用游戏场景下。
> 参考上游：`D:\By GitHub\Twenty-four`。

### 背景

当前所有 Manager（Talk/Tachie/Branch/Background/Camera/Sound/Save）均注册为 Godot autoload，通过 `static Instance` 全局访问。问题：
- 它们仅在 VN 游戏场景中有意义，主菜单/相册/设置不需要它们
- static Instance 与 GFramework DI 体系不一致
- 无法利用生命期管理（随场景进入/退出自动 Init/Destroy）

### 核心变化

```
当前                              → 目标
Manager = autoload + Instance      → ISystem，注册到 SystemModule
Manager 挂 scene tree 根            → 挂载在 VnGameScene 下
project.godot 11 个 autoload       → 仅 5 个框架 autoload 保留
```

### Manager 分类

**需 Godot 节点的 Manager（挂到 VnGameScene）：**

| Manager | 节点类型 | 实现 ISystem |
|---------|---------|-------------|
| TalkManager | CanvasLayer + RichTextLabel | ✅ |
| TachieManager | CanvasLayer + TextureRect×4 | ✅ |
| BranchManager | CanvasLayer + Button | ✅ |
| BackgroundManager | CanvasLayer + TextureRect×2 | ✅ |
| CameraManager | CanvasLayer + Camera2D | ✅ |
| SoundManager | CanvasLayer + AudioStreamPlayer×10 | ✅ |

**纯逻辑 Manager（注册为 ISystem，无场景节点）：**

| 类 | 职责 |
|---|------|
| StoryEngineSystem | 引擎循环（已注册为 IUtility，需改为 ISystem） |
| SaveManager | JSON 存档 |

**保留为 autoload（框架基础设施）：**

```
GameEntryPoint, SceneRoot, UiRoot, GlobalInputController, SceneTransitionManager
```

### 实施步骤

- [x] 1. **Manager 实现 ISystem**：添加 `ISystem` 接口（`Init()`/`Destroy()`/`OnArchitecturePhase()`）
- [x] 2. **移除 static Instance**：删除所有 `public static Xxx Instance`，改用 DI 访问
- [x] 3. **注册到 SystemModule**：StoryEngineSystem + SaveManager 纯逻辑系统已注册
- [x] 4. **创建 VnGameScene**：新建 `scenes/game/vn_game_scene.tscn`，容纳 6 个 Manager
- [ ] 5. **创建 VnGameState**：进入状态时加载 VnGameScene，退出时卸载（后续）
- [x] 6. **清理 autoload**：project.godot 移除 7 个 Manager autoload（仅保留 5 个框架节点）
- [x] 7. **更新所有引用**：`Instance` → `this.GetSystem<T>()`
- [x] 8. **StoryEngineSystem 迁移**：从 `IUtility` → `ISystem`
- [x] 9. **VnTestController 适配**：通过 `GetSystem<T>()` 访问 Manager 和 Engine
- [x] 10. **回归测试**：dotnet build 0 错误 + 42 测试通过

### 访问方式对比

```csharp
// 重构前
TalkManager.Instance?.Toggle();
CameraManager.Instance?.Play(new EarthquakeEffect { Duration = 1.5f });

// 重构后
this.GetSystem<TalkManager>().Toggle();
this.GetSystem<CameraManager>().Play(new EarthquakeEffect { Duration = 1.5f });
```

### 风险评估

| 风险 | 缓解 |
|------|------|
| `ISystem` 接口契约未知 | 参考 `PokerSystem` 等 Twenty-four 现有实现 |
| `[ContextAware]` + `ISystem` 兼容性 | 已验证 Twenty-four 的 System 同时使用两者 |
| Godot 节点在 System 中的生命周期 | Manager 挂在 VnGameScene 下，随场景 `queue_free` 自然释放 |
| 事件订阅者变更 | `UnRegisterWhenNodeExitTree` 仍适用 |

## 后续考虑

- [ ] 给 `branch_option.tscn` 加 C# 脚本，消除 `BranchManager` 中的硬编码字符串路径
- [ ] `StoryResourceMapper` 改为实例化（非静态），避免跨章节残留
- [ ] `CameraManager` 缩放叠加公式简化（`-1f + 1f` 是恒等变换）
- [ ] autoload 路径统一使用 UID 引用
- [ ] TachieManager 支持真正的 3 人同屏（Center 槽位不再被聚光灯独占时可用）
- [ ] 分支选项支持倒计时（`BranchOption.Wait` 字段已定义但未实现）
- [ ] 打字机速度从 `EngineContext.WordSpeed` 读取（当前 `VnTalkPage` 硬编码 0.02f）
- [ ] 自动播放模式（`EngineContext.AutoPlayDelay` 已定义但 `VnTestController` 未暴露开关）
