# 产品销量优化与库存管理系统

**Windows 桌面端应用，用于产品销量优化与库存管理**

---

## 🎉 重要提示：使用 GitHub Actions 自动编译

由于服务器环境限制，**最简单的方式是使用 GitHub Actions 自动编译**！

### 🚀 三步获取可执行文件：

1. **创建 GitHub 仓库**
   - 访问 https://github.com/new
   - 创建新仓库

2. **上传代码**
   - 拖拽项目文件夹到 GitHub 网页
   - 或使用 Git 命令行上传

3. **下载编译好的程序**
   - GitHub 会自动开始编译
   - 等待 2-3 分钟
   - 在 Actions 页面下载 EXE 文件

**详细说明**：查看 `GitHub_Actions_说明.md`

---

## 📖 项目版本

本项目提供两个版本：

| 版本 | 说明 |
|------|------|
| .NET 8.0 | 独立 EXE，无需安装 .NET（推荐）|
| .NET Framework 4.8 | Windows 10/11 已预装，兼容性好 |

---

## 🎯 使用 GitHub Actions（强烈推荐）

### 为什么推荐？

- ✅ **无需本地编译** - GitHub 服务器自动编译
- ✅ **无需安装 .NET SDK** - GitHub 环境已配置
- ✅ **自动触发** - 推送代码自动编译
- ✅ **版本管理** - 每次提交都有对应的构建产物
- ✅ **免费使用** - GitHub Actions 提供免费额度

### 快速开始

**步骤 1**：创建 GitHub 仓库

访问 https://github.com/new

**步骤 2**：上传代码

拖拽 `InventoryOptimizationSystem` 文件夹到 GitHub 网页

**步骤 3**：下载程序

- 访问仓库 → Actions 标签
- 等待 "Build Windows EXE" 完成（2-3 分钟）
- 下载 Artifacts 中的 ZIP 文件
- 解压并运行 `ProductInventoryOptimizer.exe`

**详细指南**：`GitHub_Actions_说明.md`

---

## 📦 本地编译（可选）

如果你想在本地编译：

### .NET 8.0 版本

```bash
# 1. 安装 .NET 8.0 SDK
https://dotnet.microsoft.com/download/dotnet/8.0

# 2. 打开项目
双击 ProductInventoryOptimizer.csproj

# 3. 编译
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# 4. EXE 位置
bin\Release\net8.0-windows\win-x64\publish\ProductInventoryOptimizer.exe
```

### .NET Framework 4.8 版本

1. 使用 Visual Studio 2019/2022 打开项目
2. 选择"Release"配置
3. 点击"生成解决方案"
4. 输出：`bin\Release\ProductInventoryOptimizer.exe`

**详细指南**：
- .NET 8.0：`快速启动.md`
- .NET 4.8：`编译使用指南_NET48.md`

---

## 📚 完整文档

### GitHub Actions 相关
- **GitHub_Actions_说明.md** - GitHub Actions 自动编译完整指南

### 项目文档
- **项目说明.md** - 完整功能和技术文档
- **README.md** - 本文件
- **Excel模板说明.md** - Excel 导入导出模板

### 编译指南
- **快速启动.md** - .NET 8.0 版本
- **编译使用指南_NET48.md** - .NET 4.8 版本
- **脚本使用指南.md** - 编译脚本使用说明

---

## 💡 推荐方案总结

| 场景 | 推荐方式 |
|------|----------|
| **快速获取程序** | GitHub Actions 自动编译（最简单！）|
| **个人开发测试** | Visual Studio + .NET 8.0 |
| **企业内部分发** | .NET Framework 4.8 |
| **自动化构建** | GitHub Actions |

---

## 🎯 开始使用

### 方式一：GitHub Actions（推荐）

1. 创建 GitHub 仓库
2. 上传代码
3. 等待自动编译
4. 下载 EXE

查看：`GitHub_Actions_说明.md`

---

### 方式二：本地编译

#### .NET 8.0
- 运行 `Build.ps1`（或 `StartBuild.bat`）
- 或使用命令：`dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true`

查看：`快速启动.md`

#### .NET Framework 4.8
- 使用 Visual Studio 打开项目
- 选择 Release 配置
- 生成解决方案

查看：`编译使用指南_NET48.md`

---

## 📊 功能列表

- ✅ Excel 导入/导出
- ✅ 产品管理（增删改查）
- ✅ 月度需求数据录入
- ✅ 月末库存目标设置
- ✅ 约束优化计算（Google OR-Tools）
- ✅ 结果可视化展示

---

## 🌐 GitHub Actions 工作流

已配置 `.github/workflows/build-windows.yml`：

- ✅ 自动触发（代码推送）
- ✅ 手动触发（Actions 页面）
- ✅ Windows 编译环境
- ✅ 生成独立 EXE
- ✅ 自动上传 Artifacts
- ✅ 保留 90 天

---

## 📞 需要帮助？

### GitHub Actions 问题
查看：`GitHub_Actions_说明.md` 中的故障排查部分

### 本地编译问题
- .NET 8.0：查看 `快速启动.md`
- .NET 4.8：查看 `编译使用指南_NET48.md`

### 功能使用问题
查看：`项目说明.md`

---

**推荐使用 GitHub Actions，3步获取程序！** 🚀
