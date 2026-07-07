# AVG Visual Novel 框架模板——待办计划

## 重构历史

### AVG-CQRS 初版（已合并 ✅）

> 分支 `AVG-CQRS` → `main`。完全的 CQRS 架构对齐 Twenty-four 模式。

### refactor 分支（当前 ✅）

统一架构模式 `Command → System → Event → Controller`。按模块剥离 `IStoryExecutionSystem`：

| 提交 | 模块 | 变更 |
|------|------|------|
| `d080120` | scenes | manager → controller 迁移 |
| `c471335` | menu | 两个页面独立子目录 |
| `582a184` | menu | vn_talk_page → story_page |
| `68a4d53` | story | 输入链路重构 |
| `e47910c` | background | 链路收尾 |
| `1d3fbb7` | background | 剥离为独立 ISystem + 命令驱动 |
| `b706750` | tool | TextureProcessor 纹理处理工具 |
| `af3f1c2` | tachie | 剥离为独立 ISystem + 显式槽位 |
| `4f58835` | talk | 剥离为独立 ISystem + 命令驱动 |
| `3d30f8c` | branch+goto+talk | 剥离 Branch/Goto，简化驱动 |
| `cfdd442` | sound+event | 剥离最后 executor，移除 IStoryExecutionSystem |

### 架构成果

- [x] 7 个 VN 子系统全部独立 ISystem
- [x] `IStoryExecutionSystem` 体系彻底移除
- [x] `PlayLoop` 改为纯 `switch` 分发
- [x] `EngineContext` 瘦身（移除 TalkBranch/CanNotChoose/AutoPlayDelay 等冗余字段）
- [x] Model 双写 bug 修复（TalkBranch/IsPlaying/PendingGoto 统一走 Model）
- [x] `TachieSlot` 支持显式 `Slot` 字段（Left/Center/Right）
- [x] `BranchController` 选中后自动隐藏
- [x] `TextureProcessor` Godot 原生纹理处理工具

## 分层权限表（Twenty-four 审计结论）

```
               SendCmd  SendQuery  SendEvent  RegisterEvent  GetSystem  GetModel
Command/Query    ✅       ✅         ✅         -               ✅         ✅
System           ✅       ✅         ✅         -               ✅         ✅(转)
Control(Page)    ✅       ✅         ❌         ✅              ❌         ❌
```

## 待办

- [ ] 音频系统完善（BGM 交叉淡入淡出、SFX 对象池）
- [ ] 语音系统（与对话文字联动）
- [ ] 存档系统完善（序列化/反序列化 StoryStateModel）
- [ ] 设置界面（音量、语言切换 UI）
- [ ] 自动播放/快速播放
- [ ] 打字机效果（逐字显示）
- [ ] 回看历史对话
