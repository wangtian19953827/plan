# GitHub Actions 自动编译说明

## 🚀 使用 GitHub Actions 自动编译

由于服务器环境限制，无法直接编译。我为你配置了 **GitHub Actions**，可以自动编译并生成 Windows EXE 文件！

---

## 📋 快速开始

### 第1步：创建 GitHub 仓库

1. 访问 https://github.com/new
2. 创建一个新仓库（Public 或 Private 都可以）
3. 记下仓库地址，例如：
   ```
   https://github.com/your-username/your-repo.git
   ```

---

### 第2步：上传代码

**方式 A：使用 GitHub Desktop**

1. 下载并安装 GitHub Desktop
2. File → Clone repository
3. 拖拽项目文件夹到 GitHub Desktop
4. 提交并推送

**方式 B：使用 Git 命令行**

```bash
# 1. 初始化 Git 仓库
cd InventoryOptimizationSystem
git init

# 2. 添加所有文件
git add .

# 3. 提交
git commit -m "Initial commit"

# 4. 添加远程仓库
git remote add origin https://github.com/your-username/your-repo.git

# 5. 推送
git push -u origin main
```

**方式 C：GitHub 网页上传**

1. 打开你的 GitHub 仓库
2. 点击 "uploading an existing file"
3. 拖拽整个 `InventoryOptimizationSystem` 文件夹
4. 填写提交信息
5. 点击 "Commit changes"

---

### 第3步：触发编译

**自动触发**：

代码推送到 GitHub 后，GitHub Actions 会自动开始编译！

**手动触发**：

1. 访问你的 GitHub 仓库
2. 点击 "Actions" 标签
3. 点击 "Build Windows EXE" 工作流
4. 点击 "Run workflow" 按钮
5. 选择分支，点击 "Run workflow"

---

## 🔍 查看编译进度

### 查看构建状态

1. 访问你的 GitHub 仓库
2. 点击 "Actions" 标签
3. 查看 "Build Windows EXE" 工作流的运行状态

**状态**：
- ⏳ Pending - 等待中
- 🔵 In progress - 编译中（约2-3分钟）
- ✅ Success - 编译成功
- ❌ Failed - 编译失败

---

### 查看编译日志

1. 点击正在运行的构建
2. 点击不同的步骤查看详细日志
3. 如果失败，查看错误信息

---

## 📥 下载编译好的 EXE

### 方式一：下载 Artifacts（推荐）

1. 等待编译完成（✅ Success）
2. 点击完成的构建
3. 滚动到页面底部
4. 找到 **"Artifacts"** 部分
5. 下载 `ProductInventoryOptimizer-Windows-x64-ZIP`
6. 解压 ZIP 文件
7. 运行 `ProductInventoryOptimizer.exe`

---

### 方式二：从 Release 下载

创建 Release 后会自动上传：

1. 访问你的 GitHub 仓库
2. 点击 "Releases"
3. 找到最新的 Release
4. 下载附件

---

## 📊 构建产物

GitHub Actions 会生成以下文件：

| 文件名 | 说明 |
|--------|------|
| `ProductInventoryOptimizer.exe` | 独立可执行文件（~80-120 MB）|
| `快速启动.md` | 使用指南 |
| `Excel模板说明.md` | Excel 模板说明 |
| `项目说明.md` | 完整文档 |
. | `build_info.txt` | 构建信息 |

---

## ⚙️ GitHub Actions 工作流

我已为你配置了 `.github/workflows/build-windows.yml` 文件：

**触发条件**：
- 代码推送到 main/master 分支
- 手动触发
- 创建 Release

**构建步骤**：
1. ✅ 检出代码
2. ✅ 安装 .NET 8.0 SDK
3. ✅ 还原 NuGet 包
4. ✅ 编译项目
5. ✅ 打包为独立 EXE
6. ✅ 上传 Artifacts
7. ✅ 创建 ZIP 下载包

---

## 💡 优势

使用 GitHub Actions 编译的优势：

| 优势 | 说明 |
|------|------|
| ✅ 无需本地编译 | GitHub 服务器自动编译 |
| ✅ 无需安装 .NET SDK | GitHub 环境已配置 |
| ✅ 自动触发 | 推送代码自动编译 |
| ✅ 版本管理 | 每次提交都有对应的构建产物 |
| ✅ 便于分发 | 直接从 GitHub 下载 |
| ✅ 免费使用 | GitHub Actions 提供免费额度 |

---

## 🔧 故障排查

### 问题 1：GitHub Actions 未触发

**原因**：仓库未启用 GitHub Actions

**解决**：
1. 访问仓库 → Settings → Actions
2. 选择 "Allow all actions"

---

### 问题 2：编译失败

**解决**：
1. 点击失败的构建
2. 查看失败步骤的日志
3. 复制错误信息
4. 修复代码后重新推送

---

### 问题 3：下载不到 Artifacts

**原因**：编译未完成或失败

**解决**：
1. 检查构建状态（必须是 ✅ Success）
2. 等待编译完成（约2-3分钟）
3. 刷新页面

---

## 🎯 完整流程

```
1. 创建 GitHub 仓库
   ↓
2. 上传项目代码
   ↓
3. GitHub Actions 自动开始编译
   ↓
4. 等待 2-3 分钟
   ↓
5. 下载 Artifacts
   ↓
6. 解压 ZIP 文件
   ↓
7. 运行 ProductInventoryOptimizer.exe
```

---

## ✅ 开始使用

1. 创建 GitHub 仓库
2. 上传代码
3. 等待自动编译完成
4. 下载并运行！

---

**GitHub 会自动为你编译，无需本地环境！** 🎉
