# AVG 视觉小说引擎架构

基于 `yrdk.ymzc` 项目的 JSON 驱动对话系统，抽离为 GFramework CQRS 风格的通用视觉小说引擎。

## 核心理念

故事内容以 **JSON 脚本** 编写 → 通过 `.tres` 资源文件映射逻辑名到文件路径 → 解析为强类型 `StoryCommand` 对象 → `StoryEngineSystem` 递归执行 → 通过 CQRS 事件驱动 UI 更新。

```
JSON 脚本 → StoryParser → StoryCommand[] → StoryEngineSystem → 事件 → UI 更新
                                                      ↓
                                               SaveManager ← 当前状态
```

## 命令类型（7 种）

| 类型 | 用途 | 异步？ |
|------|------|--------|
| `talk` | 对话文本 + 说话人 + 头像 | ✅ 等待点击 |
| `background` | 背景切换（淡入淡出） | ✅ 等待 Tween |
| `tachie` | 立绘管理（显示/切换/隐藏） | ❌ 即发 |
| `sound` | 音效/音乐 | ❌ 即发 |
| `branch` | 分支选项 | ✅ 等待选择 |
| `goto` | 跳转到另一脚本 | ✅ 终止当前，开始新脚本 |
| `event` | 自定义事件（章特定） | ✅ 等待完成回调 |

### JSON 脚本示例

```json
{
  "content": [
    {"type": "background", "file_path": "bg_room01", "wait_tween_end": "1"},
    {"type": "tachie", "tachies": {"nunu": {"file_path": "nunu_normal", "type": "show"}}},
    {"type": "talk", "talker": "努努", "talk_content": "欢迎回来。", "avatar_path": "nunu"},
    {"type": "talk", "is_center": "1", "talk_content": "（窗外传来鸟鸣声）"},
    {"type": "branch", "options": {
      "1A": {"text": "回应她的问候"},
      "1B": {"text": "沉默不语"}
    }},
    {"type": "goto", "file_path": "Chapter1_Option1"}
  ]
}
```

## 解释器循环

StoryEngineSystem 维护一个递归异步链：

```
play_next_command()
  ├─ 获取 cmd = commands[current_index]
  ├─ 分支过滤（跳过不属于当前路径的命令）
  ├─ Dispatch(cmd) → 发送 CqrsEvent → Worker 执行
  ├─ 如果 cmd 需要等待 → await 信号
  ├─ current_index++
  └─ play_next_command()  ← 递归
```

Goto 命令通过 `finish_story() + load_story(newPath)` 打破当前链并启动新链。

## 分支系统

- `talk_branch[]`：玩家已选择的分支 ID 列表
- `can_not_choose[]`：已被禁用的分支 ID 列表
- 每条命令可标注 `branch` 字段——解释器执行时跳过不匹配的分支
- 分支选项支持倒计时（超时自动选择）

## 服务接收者（Service Recipient）

`event` 命令通过回调桥接章节特定逻辑：

```
command_parser (通用引擎)
  └─ service_recipient.call_me(event_name)
       └─ 章节场景: match event_name → 章节特定代码
```

在 GFramework 中映射为 CQRS 事件订阅：
```csharp
this.RegisterEvent<VisualNovelCustomEventTriggeredEvent>(e => { /* 章节特定处理 */ });
```

## 存档系统

存档序列化解释器的完整状态：
- `playing_json`：当前脚本文件路径
- `current_index`：命令索引
- `talk_branch[]` / `can_not_choose[]`：分支状态
- 视觉快照：当前背景、立绘、对话文本
- 子存档：分支决策点前的临时存档

在 GFramework 中通过 JSON 序列化实现。

## 资源映射层

两层映射解耦逻辑名与文件路径：

1. **纹理映射** `TextureMapEntry`：逻辑名 → 实际纹理资源
2. **脚本映射** `CommandMapEntry`：逻辑名 → JSON 文件路径

批处理工具 `batch_create_resource.gd` 从文件夹自动生成 `.tres` 映射文件。

## 立绘对象池

- 对象池管理立绘 Sprite 节点（默认 2 个，自动扩展）
- 槽位映射（`_slot_map`）关联角色名到 Sprite
- 交叉淡入淡出切换（helper sprite 技术）
- 自动水平分布定位

## 与 GFramework 模板集成

| yrdk.ymzc 组件 | GFramework C# 映射 |
|----------------|-------------------|
| `command_parser.gd` (CanvasLayer) | `StoryEngineSystem` (注册到 SystemModule) + `VisualNovelTalkPage` (UI 页面) |
| `StoryCommand` Resource 子类 | `sealed class` 数据模型 + JSON 反序列化 |
| 7 个 parser_woker | StoryEngineSystem 内部的 dispatch 方法 |
| 6 个 autoload | GFramework DI（System/Utility 注册） |
| 章场景 `call_me()` | CQRS 事件订阅 |
| `inPerformance` Resource | `VisualNovelSaveModel` + JSON 序列化 |
