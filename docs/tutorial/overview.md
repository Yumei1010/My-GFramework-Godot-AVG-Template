# 框架教程计划

## 目标

通过 7 章可播放的 JSON 脚本教编剧和开发者使用本 VN 框架。每章**边看边学**，理论讲解与实际演示交替进行。

---

## 目录结构

```
assets/story/tutorial/
  prologue.json         序章：什么是 VN 框架
  ch1_talk.json         第一章：对话与旁白
  ch2_background.json   第二章：背景切换
  ch3_tachie.json       第三章：立绘系统
  ch4_sound.json        第四章：音频播放
  ch5_branch.json       第五章：分支与跳转
  ch6_event.json        第六章：自定义事件
  epilogue.json         终章：回顾与下一步

docs/tutorial/
  overview.md           本文档
```

---

## 每章结构模板

```
1. 向导登场 + 欢迎语（talk）
2. 概念讲解 — 居中旁白 + BBCode 代码块
3. 实际演示 — 真刀真枪跑一遍效果
4. 小节总结
5. 分支选择 — "继续下一章" / "再看一遍本节" / "跳过演示"
```

**代码展示：** 用 `[code]` BBCode 包裹，`[font_size=20]` 标题，`[color=#888]` 提示文字。

---

## 分章详情

### 序章 `prologue.json` — 欢迎

| 节目 | 内容 |
|---|---|
| 招呼 | 向导自我介绍，说明即将开始的旅程 |
| 概念 | "这个游戏本身就是一个教程"——展示 JSON 驱动游戏的核心理念 |
| 结构 | 展示 JSON 文件结构：`{ "content": [ ... ] }` |
| 预览 | 七种命令类型一览表（talk/background/tachie/sound/branch/goto/event） |
| 示范 | 一句最简单的 dialogue（"你好，世界"） |
| 跳转 | 分支 → 选第几章，或顺序进入 ch1 |

**分支设计：**
```
"一、对话系统 talk"
"二、背景切换 background"  
"三、立绘系统 tachie"
"四、音频播放 sound"
"五、分支与跳转 branch+goto"
"六、自定义事件 event"
"七、终章与回顾"
```

---

### 第一章 `ch1_talk.json` — 对话与旁白

**理论讲解：**

| 知识点 | 展示方式 |
|---|---|
| 基础对话 `talk` | 代码块 + 即时演示 |
| 说话人 `talker` | 指名 vs 不指名效果对比 |
| 居中旁白 `is_center` | 两种模式对比 |
| 等待 `wait` | 演示延迟效果 |
| 隐藏文本 `hide_labels` | 演示背景切换前隐藏对话栏 |
| BBCode 格式化 | `[b]粗体[/b]` `[color=red]红色[/color]` 等效果演示 |

**演示桥段：** 一段包含所有 talk 用法的短对话场景。

---

### 第二章 `ch2_background.json` — 背景切换

| 知识点 | 展示方式 |
|---|---|
| 切换背景 `file_path` | 代码块 + 实际切换 |
| 等待动画 `wait_tween_end` | 有等待 vs 无等待对比 |
| 延迟播放 `delay` | 代码块 + 等待演示 |
| 资源注册 `TextureKey` | 居中展示枚举定义（代码块） |

**演示：** 两张背景来回切换，展示淡入淡出效果。

---

### 第三章 `ch3_tachie.json` — 立绘系统

| 知识点 | 展示方式 |
|---|---|
| 登场 `show` / 省略 type | 代码 + 实际出现 |
| 指定槽位 `slot: Left/Center/Right` | 逐次展示 |
| 换表情 `change` | 代码 + 实际切换（同一张图演示概念） |
| 离场 `close` | 代码 + 实际消失 |
| 聚光灯 `onlyShow` | 代码 + 居中凸显 |
| 多人同框 | 两个角色同时显示（同一张图占两个槽位演示） |
| 资源注册 `TextureKey` | 居中代码展示 |

---

### 第四章 `ch4_sound.json` — 音频

| 知识点 | 展示方式 |
|---|---|
| BGM 播放 `sound_type: bgm` | 代码 + 实际播放 |
| 音效 `sound_type: oneSound` | 如果有音效素材 |
| 防重复播放 | 解释 + 再次触发同 BGM |
| 资源注册 `SoundKey` | 居中代码展示 |
| GameEntryPoint 配 SoundConfigs | 居中代码展示（导出数组） |

---

### 第五章 `ch5_branch.json` — 分支与跳转

| 知识点 | 展示方式 |
|---|---|
| 选项定义 `options` | 代码块 + 实际弹出选项 |
| 分支命令 `branch` 字段 | 选后走不同对话 |
| 跳转 `goto` | 跳转到其他章节 JSON |
| 选项带 `wait` | 演示离开选项的延迟 |
| 禁用分支 `can_not_choose` | 概念讲解（居中旁白） |

**演示：** 三选项场景，每个选项对应不同对话和跳转。分支过滤展示。

---

### 第六章 `ch6_event.json` — 自定义事件

| 知识点 | 展示方式 |
|---|---|
| 自定义事件 `event` | 代码 + 实际触发 |
| 事件名约定 | 居中旁白讲解 |
| 开发者如何写 View 订阅 | 代码展示（居中旁白 + BBCode 代码块） |
| 事件的 `wait` 特性 | 触发后等待点击推进 |

**演示：** 触发 `tutorial_firework` 事件（由教程专用 View 订阅实现烟花效果，或简化为弹窗提示）。

**代码展示：** View 的 RegisterEvent 示例 C# 代码。

---

### 终章 `epilogue.json` — 回顾与下一步

| 节目 | 内容 |
|---|---|
| 回顾 | 快速重放所有系统效果（5 秒内展示 talk→bg→tachie→sound→branch） |
| 新增素材 | 居中展示如何在 `TextureKey` 枚举加值、`GameEntryPoint` 配资源 |
| 添加命令 | 居中展示 `StoryParser` switch + `PlayLoop` case 写法 |
| 再见 | 向导告别，感谢阅读 |

---

## BBCode 排版约定

```
[font_size=22]JSON 写法[/font_size]         ← 标题

[code]{                                 ← 代码块
  "type": "talk",
  "talker": "角色",
  "talk_content": "你好"
}[/code]

[color=#666]← 点击继续[/color]            ← 提示文字

[color=#cc4444]重点[/color]               ← 强调
```

## 注册方式

教程脚本通过 `TutorialStoryPage.cs`（或复用 StoryPage）注册并自动播放序章。

```csharp
StoryEngine.RegisterJson("tutorial_prologue", "res://assets/story/tutorial/prologue.json");
StoryEngine.RegisterJson("tutorial_ch1", "res://assets/story/tutorial/ch1_talk.json");
// ...
```

---

## 后续

- [ ] 用户确认每章内容
- [ ] 用户提供 OC 角色设定（名字、性格、口头禅）
- [ ] 编写全部 JSON
- [ ] 素材替换（用户绘制后）
