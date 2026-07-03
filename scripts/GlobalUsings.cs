// 全局命名空间导入 - 系统基础功能
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;

// Godot 引擎
global using Godot;

// GFramework 核心扩展（GetSystem/GetUtility/SendEvent/RegisterEvent/UnRegisterWhenNodeExitTree）
global using GFramework.Core.extensions;
global using GFramework.Godot.extensions;

// GFramework 源代码生成器特性（[Log] / [ContextAware]）
global using GFramework.SourceGenerators.Abstractions.logging;
global using GFramework.SourceGenerators.Abstractions.rule;

// GFramework 系统接口
global using GFramework.Core.Abstractions.enums;
global using GFramework.Core.Abstractions.system;