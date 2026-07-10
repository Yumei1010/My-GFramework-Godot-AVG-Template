# 下一阶段计划

本文档记录已完成重构后的待办事项，供日后讨论和执行。

---

## 1. PlayLoop 直调 System

**现状：** `StoryEngine.PlayLoop()` switch 里每个 case 手动构造一个 Command 再 `SendCommand(Async)`。

```csharp
case "talk":
    await this.SendCommandAsync(new TalkPlayCommand(new TalkPlayInput { ... }));
    break;
```

**方案：** PlayLoop 直接调用 System 方法，跳过中间 Command。

```csharp
case "talk":
    var t = (TalkCommand)cmd;
    await this.GetSystem<TalkSystem>().PlayAsync(t.Talker, t.TalkContent, t.IsCenter, 0.04f);
    break;
```

**影响：**
- `TalkPlayCommand`、`BackgroundChangeCommand`、`SoundPlayCommand` 等保留为**外部 API**（UI 按钮调用）
- PlayLoop 内部更简洁（~30 行减少）
- 需确认是否违反分层原则（StoryEngine → System 直接调是否 OK）

---

## 2. `cqrs/visualnovel/command/` 目录改名

**现状：** JSON 数据类（`TalkCommand`、`BackgroundCommand` 等 `StoryCommand` 子类）放在 `visualnovel/command/`，容易和 CQRS 的 `talk/command/TalkPlayCommand` 混淆。

**候选名：**
- `story_data/` — 直观，表示"故事数据"
- `json_dto/` — 明确是 JSON DTO
- `script_cmd/` — 脚本命令

**影响：** 7 个文件改名 + 命名空间更新 + 所有引用方 using 更新。

---

## 3. TachieView 对象池 / 动态槽位

**现状：** 硬编码 3 个槽位（Left/Center/Right），超过 3 个角色无法同时显示。

**旧 GDScript 参考：** 有 `acquire()` / `release()` 对象池，动态创建 `Sprite2D` 节点。

**改进方向：**
- 槽位从 3 个扩展到可配置数量
- 立绘节点动态创建而非场景预置
- 或者保持 3 个固定槽位（VN 场景通常不超过 3 人同框）但文档说清楚上限

**影响：** TachieView + TachieSystem 改动。

---

## 4. SaveSystem 断点续档

**现状：** 存档保存 `PlayingJson` + `TalkBranch`，读档时重放脚本。不保存 `CurrentIndex`。

**方案：** `SaveData` 加 `CurrentIndex` 字段，读档时从 index 开始播放而非重头。

**权衡：**
- 重放模式：状态一致（背景/立绘正确恢复），但长脚本慢
- 断点模式：快，但需额外保存背景/立绘状态

**建议：** 混合模式——保存 `CurrentIndex`，恢复时同时应用最近的状态快照。

---

## 5. 全量注释规范化

**范围：** ~57 个文件缺少 XML `<summary>` 注释。

| 类别 | 文件数 |
|---|---|
| Command | ~22 |
| Event | ~10 |
| System 公开方法 | ~4 |
| JSON 数据类 | ~3 |
| Model 字段 | ~5 |
| Enum | ~5 |
| View `.cs` 文件 | ~6 |
| core/ | 2 |

**规范：** 中文，类级或公开方法级。不改变逻辑，纯注释添加。

---

## 6. 框架教程

详见 `docs/tutorial/overview.md`。7 章可播放 JSON 教程。

**阻塞项：** 等用户提供 OC 角色设定和素材。

---

## 优先级建议

| 优先级 | 事项 | 理由 |
|---|---|---|
| P0 | 教程 JSON 编写 | 最重要的交付物，展示框架能力 |
| P1 | 注释规范化 | 不费脑，量可控 |
| P2 | PlayLoop 直调 | 改善架构但需评估是否过度耦合 |
| P3 | 目录改名 | 小改动但波及面广 |
| P3 | SaveSystem 增强 | 功能补全 |
| P4 | TachieView 对象池 | 对大多数 VN 非必须 |
