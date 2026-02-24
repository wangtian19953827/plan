# 产品销量优化与库存管理系统

**Windows 桌面端应用，用于产品销量优化与库存管理**

支持 Excel 导入导出，基于约束优化算法计算最优销量/产量。

---

## 🎯 两个版本

本项目提供两个版本，以满足不同需求：

### 版本 1：.NET 8.0 版本（推荐）
- **适用场景**: 个人使用、现代系统、希望独立的 EXE
- **目标机器**: Windows 10/11
- **运行时**: 独立 EXE，无需安装 .NET
- **编译工具**: .NET 8.0 SDK
- **打包方式**: `scripts/install.bat` → `scripts/create_zip.ps1`

### 版本 2：.NET Framework 4.8 版本
- **适用场景**: 企业内部分发、兼容旧系统
- **目标机器**: Windows 7+（需要 .NET Framework 4.8）
- **运行时**: Windows 10/11 已预装 .NET Framework 4.8
- **编译工具**: Visual Studio 2017/2019/2022
- **打包方式**: `scripts/build_net48.bat`

---

## 🚀 快速开始

### 如果你需要 .NET 8.0 版本（推荐）

**编译**：
```bash
cd scripts
install.bat
```

**生成分发包**：
```bash
create_zip.ps1
```

详细说明：`快速启动.md` 或 `scripts/使用说明.md`

---

### 如果你需要 .NET Framework 4.8 版本

**编译**：
```bash
cd scripts
build_net48.bat
```

**发布文件夹**：`Release_Package/`（包含 EXE + 所有 DLL）

详细说明：`编译使用指南_NET48.md`

---

## 📦 技术栈

### .NET 8.0 版本
- **框架**: .NET 8.0
- **UI**: WinForms
- **Excel**: EPPlus 7.0.5
- **优化**: Google OR-Tools 9.9.3963

### .NET Framework 4.8 版本
- **框架**: .NET Framework 4.8
- **UI**: WinForms
- **Excel**: EPPlus 4.5.3
- **优化**: Google OR-Tools 7.8.0

---

## 💡 如何选择版本？

### 选择 .NET 8.0 版本，如果：
- ✅ 你希望获得一个独立的 EXE 文件
- ✅ 目标机器是 Windows 10/11
- ✅ 你有 .NET 8.0 SDK
- ✅ 你需要最小的分发文件

### 选择 .NET Framework 4.8 版本，如果：
- ✅ 目标机器可能已预装 .NET Framework 4.8
- ✅ 你使用 Visual Studio 2019/2022
- ✅ 企业环境，统一使用 .NET Framework
- ✅ 需要更好的兼容性

---

## 📋 核心功能

1. ✅ Excel 导入/导出
2. ✅ 产品管理（增删改查）
3. ✅ 月度需求数据录入
4. ✅ 月末库存目标设置
5. ✅ 约束优化计算（总供应量上限 + 目标销售额）
6. ✅ 结果可视化展示

---

## 📚 文档

### 通用文档
- **项目说明.md** - 完整的功能和技术文档
- **Excel模板说明.md** - Excel 导入导出模板

### .NET 8.0 版本文档
- **快速启动.md** - 程序员快速上手
- **scripts/使用说明.md** - 脚本使用指南
- **PROJECT_DELIVERY.md** - 项目交付说明

### .NET Framework 4.8 版本文档
- **编译使用指南_NET48.md** - .NET Framework 4.8 编译指南

---

## ⚙️ 项目结构

```
InventoryOptimizationSystem/
├── Models/                 # 数据模型（两个版本共用）
├── Services/               # 业务逻辑
├── UI/                     # 窗体界面（两个版本共用）
├── Program.cs              # 程序入口（两个版本不同）
- ├── ProductInventoryOptimizer.csproj  # .NET 8.0 项目文件
- ├── ProductInventoryOptimizer_NET48.csproj  # .NET 4.8 项目文件
- ├── App.config            # .NET 4.8 配置
├── scripts/               # 构建脚本
│   ├── install.bat        # .NET 8.0 一键构建
│   ├── build.ps1          # .NET 8.0 PowerShell 构建
│   ├── create_zip.ps1     # .NET 8.0 创建 ZIP
│   ├── build_net48.bat    # .NET 4.8 构建脚本
│   └── 使用说明.md       # 脚本使用指南
└── 文档/                   # 完整的文档
```

---

## 🎊 开始使用

**第1步**：选择你需要的版本

**第2步**：阅读对应版本的编译指南
- .NET 8.0：`快速启动.md`
- .NET Framework 4.8：`编译使用指南_NET48.md`

**第3步**：运行构建脚本

**第4步**：复制发布文件夹到目标机器

---

**两个版本功能完全相同，选择适合你的即可！** 🚀
