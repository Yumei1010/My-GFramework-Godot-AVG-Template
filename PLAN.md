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

## 后续考虑

- [ ] 给 `branch_option.tscn` 加 C# 脚本，消除 `BranchManager` 中的硬编码字符串路径
- [ ] `StoryResourceMapper` 改为实例化（非静态），避免跨章节残留
- [ ] `CameraManager` 缩放叠加公式简化（`-1f + 1f` 是恒等变换）
- [ ] autoload 路径统一使用 UID 引用
- [ ] TachieManager 支持真正的 3 人同屏（Center 槽位不再被聚光灯独占时可用）
- [ ] 分支选项支持倒计时（`BranchOption.Wait` 字段已定义但未实现）
- [ ] 打字机速度从 `EngineContext.WordSpeed` 读取（当前 `VnTalkPage` 硬编码 0.02f）
- [ ] 自动播放模式（`EngineContext.AutoPlayDelay` 已定义但 `VnTestController` 未暴露开关）
