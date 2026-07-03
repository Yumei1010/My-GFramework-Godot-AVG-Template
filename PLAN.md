# AVG Visual Novel 框架模板——待办计划

## Twenty-four 架构审计结论（2026-07-03）

### 确凿规则（逐行代码验证）

| # | 规则 | 证据 |
|---|------|------|
| 1 | ISystem = 纯 C# partial class，**零 Godot 节点** | 13 个 ISystem 无一例外 |
| 2 | Model = `public class X : AbstractModel`，纯数据 | 18 个 Model 一致 |
| 3 | Command = `sealed class : AbstractCommand` | `OnExecute()` 可调用 GetSystem/GetModel/SendEvent/SendQuery |
| 4 | Query = `sealed class : AbstractQuery<TReturn>` | `OnDo()` 只调 GetModel，不调其他 |
| 5 | Event = `sealed class XxxEvent` + `required` + `{ get; init; }` | 36 个事件全部如此 |
| 6 | Entity Signal 层 = Godot 信号 → SendCommand 桥接 | 脚手架代码直接调了 SendEvent，最终应走 Command |
| 7 | Entity(Menu) **绝不可以** GetSystem/GetModel | Poker/CalculateMenu 无一调用 |
| 8 | System **可以** GetSystem/GetModel/SendEvent | SelectorSystem/DeckSystem 等 |

### 分层权限表

```
               SendCmd  SendQuery  SendEvent  RegisterEvent  GetSystem  GetModel
Command/Query    ✅       ✅         ✅         -               ✅         ✅
System           ✅       ✅         ✅         -               ✅         ✅(转)
Control          ✅       ✅         ❌         ✅              ❌         ❌
Worker          (ctx)     -          ✅(ctx)    -               -          -
```

### 当前 AVG-CQRS 分支状态

- [x] ISystem + Controller 分离（7 System + 6 Controller）
- [x] Model 层（7 个 Model）
- [x] Command/Query 层
- [x] 全局 using 优化
- [x] Controller 零 GetSystem/GetModel
- [x] Worker 通过 ctx 发送事件
- [x] 分层权限对齐

## 待办

- [ ] 补充示例素材说明
- [ ] 完善 JSON 故事格式文档
- [ ] 创建示例 Entity 模式参考（Twenty-four 的 Poker 五文件 partial class）
