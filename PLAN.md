# AVG Visual Novel 框架模板——待办计划

## Twenty-four 架构审计结论（2026-07-03）

### 确凿规则（逐行代码验证）

| # | 规则 | 证据 |
|---|------|------|
| 1 | ISystem = 纯 C# partial class，**零 Godot 节点** | 13 个 ISystem 无一例外 |
| 2 | Model = `public class X : AbstractModel`，纯数据 | 18 个 Model 一致 |
| 3 | Command = `sealed class : AbstractCommand` | `OnExecute()` 可调用 GetSystem/GetModel/SendEvent/SendQuery |
| 4 | Query = `sealed class : AbstractQuery<TReturn>` | `OnDo()` 只调 GetModel（偶有 GetSystem），不调其他 |
| 5 | Event = `sealed class XxxEvent` + `required` + `{ get; init; }` | 36 个事件全部如此 |
| 6 | Entity(Menu) **可以** SendEvent！ | `CalculateMenu.Signals.cs:15` 直接调 SendEvent |
| 7 | Entity(Menu) **绝不可以** GetSystem/GetModel | Poker/CalculateMenu 无一调用 |
| 8 | System **可以** GetSystem/GetModel/SendEvent | SelectorSystem/DeckSystem 等 |

### 修正后的分层权限

```
               SendCmd  SendQuery  SendEvent  RegisterEvent  GetSystem  GetModel
Command/Query    ✅       ✅         ✅         -              ✅         ✅
System           ✅       ✅         ✅         -              ✅         ✅(转)
Control/View     ✅       ✅         ✅         ✅             ❌         ❌
```

### 当前 AVG-CQRS 分支需要修正的地方

1. Worker 层应该可以通过 `ctx.SendEvent()` 发事件（它是 System 的扩展，等同于 System 内部逻辑）
2. Control 层可以 `SendEvent`（用于 Godot 信号→CQRS 桥接）
3. Control 层**绝不能** `GetSystem/GetModel`（必须通过 SendCommand/SendQuery）

## 第四阶段：AVG-CQRS 架构重构（已完成 ✅）

- [x] ISystem + Controller 分离（7+6）
- [x] Model 层（7 个）
- [x] Command/Query 层
- [x] 全局 using 优化
- [x] Controller 移除 GetSystem/GetModel

## 待办

- [ ] 恢复 Worker 通过 ctx 发送事件
- [ ] 示例 Entity 模式（参考 Twenty-four 的 Poker 五文件 partial class）
- [ ] 补全文档（README 中 VN 快速上手）
- [ ] 添加示例素材说明
