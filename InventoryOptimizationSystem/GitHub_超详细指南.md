# GitHub Actions 自动编译 - 超详细指南

**一步步教你如何上传代码并下载编译好的程序**

---

## 📋 准备工作

### 前置条件

- ✅ GitHub 账号（如果没有，免费注册：https://github.com/signup）
- ✅ 下载好的项目包：`InventoryOptimizer_WithGitHub.tar.gz`
- ✅ 解压到任意文件夹（例如：`C:\Projects\InventoryOptimizerSystem`）

---

## 🎯 第1步：登录 GitHub

### 1.1 访问 GitHub

打开浏览器，访问：
```
https://github.com
```

### 1.2 登录

- 点击右上角的 **"Sign in"** 按钮
- 输入你的用户名和密码
- 点击 **"Sign in"**

---

## 🎯 第2步：创建新仓库

### 2.1 创建仓库

- 登录后，点击右上角的 **"+"** 按钮
- 选择 **"New repository"**

或者直接访问：
```
https://github.com/new
```

### 2.2 填写仓库信息

在 "Create a new repository" 页面：

1. **Repository name**（仓库名）
   - 输入：`inventory-optimizer`
   - 或任何你喜欢的名字
   - 注意：只能用小写字母、数字、连字符

2. **Description**（描述，可选）
   - 输入：`产品销量优化与库存管理系统`
   - 可选，留空也可以

3. **Public / Private**（公开/私有）
   - 选择 **"Public"**（任何人可见）
   - 或选择 **"Private"**（只有你能看到）
   - 推荐选择 "Private"

4. **其他选项**（保持默认）
   - ✅ 不要勾选 "Add a README file"
   - ✅ 不要勾选 "Add .gitignore"
   - ✅ 不要勾选 "Choose a license"

### 2.3 创建仓库

点击绿色按钮：**"Create repository"**

---

## 🎯 第3步：上传代码

### 3.1 访问仓库页面

创建成功后，你会看到仓库页面，地址类似：
```
https://github.com/your-username/inventory-optimizer
```

**记住这个地址！后面会用到。**

### 3.2 上传文件

在仓库页面，找到并点击：
```
uploading an existing file
```

这个链接通常在页面中间的提示框里。

### 3.3 拖拽上传

1. 点击后，会进入上传页面
2. 找到你解压的 `InventoryOptimizerSystem` 文件夹
3. **拖拽整个文件夹到上传区域**

或者：
- 点击 **"choose your files"** 按钮
- 浏览到项目文件夹
- 选择所有文件和文件夹

### 3.4 填写提交信息

在 "Commit changes" 部分：

1. **Commit message**（提交信息）
   - 输入：`Initial commit - Product Inventory Optimizer`
   - 这是必须填写的

2. **Description**（描述，可选）
   - 输入：`初始提交 - 产品库存优化系统`
   - 可选，留空也可以

3. **Commit directly to the main branch**（直接提交到主分支）
   - 保持默认选中

### 3.5 提交上传

点击绿色按钮：**"Commit changes"**

**等待上传完成**（根据网络速度，可能需要 10-30 秒）

---

## 🎯 第4步：查看 Actions 编译状态

### 4.1 访问 Actions 页面

上传完成后，GitHub 会自动触发编译！

在你的仓库页面：

1. 点击顶部导航栏的 **"Actions"** 标签
   - Actions 标签在仓库名、Issues 等标签旁边

### 4.2 查看工作流

在 Actions 页面，你会看到：

- **"Build Windows EXE"** 工作流
- 状态可能是：
  - 🔵 ⏳ **Pending** - 等待中（刚触发）
  - 🟡 🔵 **In progress** - 编译中
  - 🟢 ✅ **Success** - 编译成功
  - 🔴 ❌ **Failed** - 编译失败

### 4.3 查看编译进度

点击 **"Build Windows EXE"** 工作

你会看到以下步骤：

1. ✅ ⭕ **Checkout code** - 检出代码
2. ✅ ⭕ **Setup .NET** - 安装 .NET 8.0 SDK
3. ✅ ⭕ **Restore dependencies** - 还原 NuGet 包
4.4. ✅ ⭕ **Build project** - 编译项目
5. ✅ ⭕ **Publish as standalone EXE** - 打包为 EXE
6. ✅ ⭕ **Get version info** - 获取版本信息
7. ✅ ⭕ **Create release package** - 创建发布包
8. ✅ ⭕ **Upload artifact** - 上传产物
9. ✅ ⭕ **Create ZIP archive** - 创建 ZIP 压缩包
10. ✅ ⭕ **Upload ZIP artifact** - 上传 ZIP 包
11. ✅ ⭕ **Release summary** - 生成总结

**符号说明**：
- ✅ 灰色勾：等待中
- 🟢 绿色勾：完成
- 🔴 红色叉：失败
- ⏳ 旋转图标：进行中

### 4.4 等待编译完成

**预期时间**：
- 通常 2-4 分钟
- 取决于 GitHub 服务器负载

**刷新页面**：
- 如果页面没自动更新，可以手动刷新
- 点击浏览器刷新按钮，或按 F5

---

## 🎯 第5步：下载编译好的程序

### 5.1 确认编译成功

等到所有步骤都显示 **✅ 绿色勾**

在页面顶部会显示：
```
✅ Build Windows EXE
```

### 5.2 找到 Artifacts 部分

滚动到页面最底部，找到：
```
Artifacts
```

部分

在 Artifacts 下面，你会看到两个文件：

1. **ProductInventoryOptimizer-Windows-x64-ZIP**
   - 这是 ZIP 压缩包，推荐下载
   - 包含所有文件

2. **ProductInventoryOptimizer-Windows-x64**
   - 这是未压缩的文件
   - 包含 EXE 和文档

### 5.3 下载 ZIP 包

点击 **"ProductInventoryOptimizer-Windows-x64-ZIP"**

浏览器会开始下载 ZIP 文件。

**文件大小**：约 80-120 MB

### 5.4 解压 ZIP 文件

下载完成后：

1. 找到下载的 ZIP 文件
2. 右键点击 ZIP 文件
3. 选择 **"解压到当前文件夹"** 或 **"Extract Here"**
4. 或使用 WinRAR / 7-Zip 等工具解压

解压后，你会看到：

```
ProductInventoryOptimizer.exe  ← 主程序
快速启动.md                     ← 使用指南
Excel模板说明.md                ← Excel 模板说明
项目说明.md                     ← 完整文档
build_info.txt                  ← 构建信息
```

---

## 🎯 第6步：运行程序

### 6.1 双击运行

找到并双击：
```
ProductInventoryOptimizer.exe
```

### 6.2 首次运行

程序会启动，你会看到：

**主界面**：
- 选项卡式布局
  - **产品管理** - 显示 80 个示例产品
  - **月度数据** - 显示月度需求数据
  - **计算结果** - 优化计算结果

- 工具栏：
  - **导入Excel** - 导入外部 Excel 文件
  - **导出结果** - 导出计算结果到 Excel
  - **开始计算** - 执行优化计算

- 参数面板：
  - **本月可供应总数** - 输入供应量上限
  - **目标销售额（元）** - 输入目标销售额

### 6.3 测试计算

1. 确保"本月可供应总数"有值（默认 10000）
2. 确保"目标销售额"有值（默认 500000）
3. 点击 **"开始计算"** 按钮
4. 等待计算完成（< 1 秒）
5. 查看"计算结果"选项卡

---

## 🔧 如果编译失败

### 情况 1：Actions 未触发

**现象**：上传代码后，看不到 Actions 运行

**解决**：

1. 访问仓库 → **Settings** → **Actions** → **General**
2. 找到 **"Actions permissions"** 部分
3. 选择 **"Allow all actions"**
4. 保存设置
5. 重新触发编译（点击 **"Run workflow"** 按钮）

---

### 情况 2：编译失败

**现象**：Actions 显示 ❌ Failed

**查看错误**：

1. 点击失败的构建
2. 查看失败的步骤（红色叉标记）
3. 点击该步骤展开日志
4. 查看错误信息

**常见错误**：

#### 错误 A：NuGet 包还原失败

```
error: Unable to load the service index for source https://api.nuget.org/v3/index.json
```

**解决**：
- 检查网络连接
- 检查代理设置
- 稍后重试

#### 错误 B：编译错误

```
error CSxxx: 某处有语法错误
```

**解决**：
- 查看编译错误详情
- 可能是代码问题
- 联系我修复

---

## 🆘 其他操作

### 手动触发编译

如果编译没自动触发：

1. 访问仓库 → **Actions** 标签
2. 点击 **"Build Windows EXE"** 工作流
3. 点击 **"Run workflow"** 按钮
4. 选择分支（main）
5. 点击 **"Run workflow"**

### 查看历史构建

在 Actions 页面，可以看到所有历史构建记录，包括：
- 构建时间
- 构建状态
- 提交信息
- 可以重新下载旧的构建产物

### 删除旧构建产物

默认保留 90 天。如果需要清理：
1. 访问仓库 → **Settings** → **Actions** → **Artifacts**
2. 选择要删除的构建产物
3. 点击 **"Delete"** 按钮

---

## 📚 更多帮助

### GitHub 官方文档

- GitHub Actions 文档：https://docs.github.com/en/actions
- 上传文件指南：https://docs.github.com/en/get-started/uploading-files-on-github

### 项目文档

- `项目说明.md` - 完整功能文档
- `Excel模板说明.md` - Excel 导入导出模板
- `快速启动.md` - 快速使用指南

---

## ✅ 成功！

如果你按照这个指南一步步操作，应该能成功：

1. ✅ 创建 GitHub 仓库
2. ✅ 上传项目代码
3. ✅ 等待自动编译（2-4 分钟）
4. ✅ 下载编译好的 EXE 文件
5. ✅ 解压并运行程序

**总用时**：约 10-15 分钟

---

## 🎉 开始使用

**准备好了吗？**

按照上面的步骤，从第1步开始，一步步操作！

遇到问题随时告诉我，我会帮你解决！
