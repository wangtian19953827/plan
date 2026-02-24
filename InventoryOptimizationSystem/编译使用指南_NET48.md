# 产品销量优化与库存管理系统 (.NET Framework 4.8 版本)

**版本**: 1.0.0  
**运行环境**: Windows 10/11 + .NET Framework 4.8  
**状态**: ✅ 已完成

---

## 🎯 版本说明

本版本使用 **.NET Framework 4.8**，相比 .NET 8.0 版本的优势：

### 优点 ✅
- ✅ Windows 10/11 已预装 .NET Framework 4.8
- ✅ 目标机器无需额外安装 .NET Runtime
- ✅ 兼容性更好，适合企业内部分发
- ✅ 使用 Visual Studio 2019/2022 即可编译

### 缺点 ⚠️
- ⚠️ 需要 Visual Studio 2017+ 才能编译
- ⚠️ EXE 文件较大（包含依赖 DLL）
- ⚠️ 需要单独打包依赖 DLL

---

## 🚀 快速开始

### 方式一：使用自动化脚本（推荐）

**前提条件**：
- ✅ 已安装 Visual Studio 2017/2019/2022
- ✅ 已安装 .NET Framework 4.8

**步骤**：
```bash
cd scripts
build_net48.bat
```

**脚本会自动完成**：
1. ✅ 检测 MSBuild
2. ✅ 还原 NuGet 包
3. ✅ 编译项目
4. ✅ 整理发布文件到 `Release_Package/` 目录
5. ✅ 自动打开发布文件夹

---

### 方式二：使用 Visual Studio

**步骤**：
1. 打开 Visual Studio 2019/2022
2. 双击 `ProductInventoryOptimizer.csproj`
3. Visual Studio 会自动还原 NuGet 包
4. 选择配置为 "Release"
5. 点击"生成" → "生成解决方案"（或 Ctrl+Shift+B）

**编译输出**：
```
bin\Release\ProductInventoryOptimizer.exe
```

---

### 方式三：使用 MSBuild 命令行

```bash
# 还原 NuGet 包（如果需要）
nuget restore ProductInventoryOptimizer.csproj

# 编译项目
msbuild ProductInventoryOptimizer.csproj /p:Configuration=Release
```

---

## 📦 发布文件夹结构

运行 `build_net48.bat` 后：

```
Release_Package/
├── ProductInventoryOptimizer.exe    (主程序)
├── EPPlus.dll                     (Excel处理库)
├── Google.OrTools.dll             (优化求解库)
├── Google.OrTools.linear_solver.dll
├── Google.Protobuf.dll
├── ... (其他依赖 DLL)
├── 快速启动.md
├── Excel模板说明.md
└── 项目说明.md
```

**整个 `Release_Package` 文件夹**可以复制到其他 Windows 机器使用。

---

## ⚙️ 环境要求

### 开发环境
- ✅ Windows 10/11
- ✅ Visual Studio 2017/2019/2022（Community/Professional/Enterprise）
- ✅ .NET Framework 4.8 SDK（Visual Studio 安装时选择）

### 运行环境（目标机器）
- ✅ Windows 10/11
- ✅ .NET Framework 4.8（通常已预装）
- ✅ 4GB+ 内存（推荐 8GB）
- ✅ 500MB+ 硬盘空间

### 检查 .NET Framework 4.8 是否已安装

**方法 1**：运行程序，如果提示缺少 .NET Framework，按照提示安装

**方法 2**：控制面板 → 程序和功能 → 启用或关闭 Windows 功能

**方法 3**：下载 .NET Framework 4.8 Runtime
- 下载地址：https://dotnet.microsoft.com/download/dotnet-framework/thank-you/net48-developer-pack-offline

---

## 🔧 编译问题排查

### 问题 1: 提示"找不到 MSBuild"

**原因**：未安装 Visual Studio 或未将 MSBuild 添加到 PATH

**解决方法**：
1. 安装 Visual Studio 2019/2022
2. 确保在安装时选择 ".NET 桌架 4.8 开发工具"
3. 或使用 Visual Studio Developer Command Prompt

### 问题 2: NuGet 包还原失败

**原因**：NuGet.exe 不存在或网络问题

**解决方法**：
1. 使用 Visual Studio 打开项目，它会自动还原 NuGet 包
2. 或手动下载 NuGet.exe：
   - 下载：https://www.nuget.org/downloads
   - 放到 `packages/` 目录

### 问题 3: 提示"缺少引用"

**原因**：NuGet 包未正确还原

**解决方法**：
1. 在 Visual Studio 中右键项目 → "还原 NuGet 包"
2. 或删除 `packages/` 目录，重新还原

### 问题 4: Google.OrTools DLL 加载失败

**原因**：依赖 DLL 未正确复制

**解决方法**：
1. 确保 `bin\Release\` 目录包含所有必要的 DLL
2. 或将整个 `bin\Release\` 目录复制到发布文件夹

---

## 📋 NuGet 包依赖

项目依赖以下 NuGet 包（已配置在 .csproj 中）：

```xml
<PackageReference Include="EPPlus" Version="4.5.3" />
<PackageReference Include="Google.OrTools" Version="7.8.0" />
```

**注意**：.NET Framework 4.8 使用较旧版本的包以确保兼容性。

### 包说明

| 包名 | 版本 | 用途 |
|------|------|------|
| EPPlus | 4.5.3 | Excel 导入导出（适用于 .NET 4.8） |
| Google.OrTools | 7.8.0 | 优化求解算法（适用于 .NET 4.8） |

---

## 📊 文件大小参考

| 文件类型 | 大小 |
|----------|------|
| ProductInventoryOptimizer.exe | ~100-300 KB |
| 依赖 DLL | ~30-50 MB |
| 文档文件 | ~100 KB |
| **总计** | **~30-50 MB** |

---

## 🎯 推荐使用场景

### 场景 1：开发测试
```bash
# 使用 Visual Studio 打开项目
# 按 F5 调试运行
```

### 场景 2：内部发布
```bash
# 1. 编译发布版本
msbuild ProductInventoryOptimizer.csproj /p:Configuration=Release

# 2. 复制 bin\Release\ 文件夹到共享目录
```

### 场景 3：最终分发
```bash
# 运行打包脚本
scripts\build_net48.bat

# 将 Release_Package 文件夹打包为 ZIP
```

---

## 🔄 与 .NET 8.0 版本的差异

| 特性 | .NET 8.0 版本 | .NET Framework 4.8 版本 |
|------|---------------|-------------------------|
| 目标机器要求 | 任意（独立 EXE） | Windows 10/11（需要 .NET 4.8） |
| EXE 大小 | ~80-120 MB（单文件） | ~100-300 KB（需依赖 DLL） |
| 编译工具 | .NET SDK 8.0 | Visual Studio 2017+ |
| 包版本 | EPPlus 7.0 + OR-Tools 9.9 | EPPlus 4.5.3 + OR-Tools 7.8.0 |
| 部署方式 | 单 EXE 文件 | EXE + DLL 文件夹 |
| 兼容性 | 新系统 | 旧系统（Win7+） |

---

## 📚 详细文档

- **项目说明.md** - 完整的功能和技术文档
- **快速启动.md** - 程序员快速上手
- **Excel模板说明.md** - Excel 导入导出模板
- **编译使用指南.md**（本文件）- .NET Framework 4.8 版本说明

---

## ❓ 常见问题

### Q1: 能否在 Windows 7 上运行？

**A**: 可以，但需要安装 .NET Framework 4.8（Windows 7 需要额外安装）

### Q2: 打包后的 EXE 离开发机器还能运行吗？

**A**: 可以，但需要一起复制所有依赖 DLL 到 `Release_Package` 文件夹

### Q3: 是否可以减少文件大小？

**A**: 可以尝试以下方法：
- 使用 ILMerge 合并 DLL（不推荐，可能有问题）
- 只复制必要的 DLL（需要测试）

### Q4: Visual Studio 哪个版本可以编译？

**A**: Visual Studio 2017、2019、2022 都可以（推荐 2019/2022）

### Q5: 编译时出现大量警告？

**A**: .NET Framework 4.8 项目可能出现一些兼容性警告，可以忽略

---

## 🎊 完成

**.NET Framework 4.8 版本已准备就绪！**

**下一步**：

1. ✅ 安装 Visual Studio 2019/2022（如果还未安装）
2. ✅ 打开项目：双击 `ProductInventoryOptimizer.csproj`
3. ✅ 运行脚本：`scripts\build_net48.bat`
4. ✅ 复制 `Release_Package` 文件夹到目标机器

---

**祝构建顺利！** 🚀
