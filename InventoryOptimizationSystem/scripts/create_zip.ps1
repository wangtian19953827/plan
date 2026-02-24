# 产品销量优化与库存管理系统 - 创建 ZIP 安装包
# 用途: 将 Release_Package 打包为 ZIP 文件

$ErrorActionPreference = "Stop"

Write-Host "======================================================" -ForegroundColor Cyan
Write-Host "  产品销量优化与库存管理系统 - 创建 ZIP 安装包" -ForegroundColor Cyan
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host ""

# 项目路径
$projectPath = $PSScriptRoot
$buildDir = Join-Path $projectPath "Release_Package"
$installerDir = Join-Path $projectPath "Installer"

# 检查构建目录
if (-not (Test-Path $buildDir)) {
    Write-Host "[错误] 未找到构建文件: $buildDir" -ForegroundColor Red
    Write-Host ""
    Write-Host "请先运行构建脚本:" -ForegroundColor Yellow
    Write-Host "  - build.bat (Windows)" -ForegroundColor White
    Write-Host "  - build.ps1 (PowerShell)" -ForegroundColor White
    Write-Host ""
    Read-Host "按 Enter 键退出"
    exit 1
}

# 清理旧的安装包
if (Test-Path $installerDir) {
    Write-Host "[清理] 删除旧的安装包..." -ForegroundColor Yellow
    Remove-Item -Path $installerDir -Recurse -Force
}

# 创建安装包目录
New-Item -ItemType Directory -Path $installerDir | Out-Null
Write-Host "[创建] 安装包目录: $installerDir" -ForegroundColor Green
Write-Host ""

# 复制文件
Write-Host "[1/4] 复制程序文件..." -ForegroundColor Yellow
$appDir = Join-Path $installerDir "ProductInventoryOptimizer"
Copy-Item -Path $buildDir -Destination $appDir -Recurse -Force
Write-Host "[完成]" -ForegroundColor Green
Write-Host ""

# 创建 README
Write-Host "[2/4] 创建安装说明..." -ForegroundColor Yellow
$readmePath = Join-Path $installerDir "README.txt"
$readmeContent = @"
产品销量优化与库存管理系统 - 安装说明
=====================================================

版本: 1.0.0
更新日期: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')

=====================================================
  快速开始
=====================================================

1. 进入 ProductInventoryOptimizer 文件夹
2. 双击运行 ProductInventoryOptimizer.exe
3. 程序会自动加载示例数据（80个产品）
4. 点击"开始计算"查看优化结果

=====================================================
  详细文档
=====================================================

- 快速启动.md: 开发者和用户快速上手指南
- Excel模板说明.md: Excel导入导出格式说明
- 项目说明.md: 完整的功能和技术文档

=====================================================
  系统要求
=====================================================

- Windows 10 或 Windows 11
- 4GB 内存（推荐 8GB）
- 500MB 硬盘空间

=====================================================
  注意事项
=====================================================

- 本程序为独立 EXE，无需安装 .NET
- 程序运行在本地，无需联网
- 所有数据保存在本地，安全可靠

=====================================================
  技术支持
=====================================================

如遇问题，请参考项目说明.md 文档

版本: 1.0.0
技术: C# + .NET 8.0 + Google OR-Tools

"@

# 设置编码为 UTF-8 with BOM
$utf8BOM = New-Object System.Text.UTF8Encoding $true
[System.IO.File]::WriteAllText($readmePath, $readmeContent, $utf8BOM)
Write-Host "[完成]" -ForegroundColor Green
Write-Host ""

# 创建卸载脚本
Write-Host "[3/4] 创建卸载脚本..." -ForegroundColor Yellow
$uninstallPath = Join-Path $installerDir "uninstall.bat"
$uninstallContent = @"
@echo off
chcp 65001 >nul

echo ======================================================
echo   产品销量优化与库存管理系统 - 卸载
echo ======================================================
echo.

echo 确认要删除程序吗？
echo.

choice /C YN /N /M "按 Y 确认删除，按 N 取消: "
if %%errorlevel%% equ 2 exit /b 0

echo.
echo 正在删除...
echo.

cd /d "%%~dp0ProductInventoryOptimizer"
cd ..

rd /s /q "ProductInventoryOptimizer"

del /f /q "uninstall.bat"
del /f /q "README.txt"

echo.
echo 卸载完成！
echo.

pause
"@
[System.IO.File]::WriteAllText($uninstallPath, $uninstallContent, $utf8BOM)
Write-Host "[完成]" -ForegroundColor Green
Write-Host ""

# 创建 ZIP 文件
Write-Host "[4/4] 创建 ZIP 安装包..." -ForegroundColor Yellow
$version = "1.0.0"
$zipFileName = "ProductInventoryOptimizer_v${version}_$((Get-Date).ToString('yyyyMMdd')).zip"
$zipPath = Join-Path $projectPath $zipFileName

# 检查是否已存在同名 ZIP
if (Test-Path $zipPath) {
    Write-Host "[警告] 已存在同名 ZIP 文件，将覆盖" -ForegroundColor Yellow
    Remove-Item -Path $zipPath -Force
}

# 压缩为 ZIP（.NET 4.5+ 方法）
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::CreateFromDirectory($installerDir, $zipPath)

# 获取文件大小
$zipSize = (Get-Item $zipPath).Length / 1MB

Write-Host "[完成]" -ForegroundColor Green
Write-Host ""

# 显示结果
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host "  ZIP 安装包创建成功！" -ForegroundColor Green
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "ZIP 文件位置:" -ForegroundColor White
Write-Host "  $zipPath" -ForegroundColor Cyan
Write-Host ""
Write-Host "文件大小: $([math]::Round($zipSize, 2)) MB" -ForegroundColor Yellow
Write-Host ""
Write-Host "使用方法:" -ForegroundColor White
Write-Host "  1. 将 $zipFileName 分发给用户" -ForegroundColor Yellow
Write-Host "  2. 用户解压 ZIP 文件" -ForegroundColor Yellow
Write-Host "  3. 运行 ProductInventoryOptimizer\ProductInventoryOptimizer.exe" -ForegroundColor Yellow
Write-Host ""

# 询问是否打开所在文件夹
$response = Read-Host "是否打开所在文件夹? (Y/N)"
if ($response -eq "Y" -or $response -eq "y") {
    explorer (Split-Path $zipPath -Parent)
}

Write-Host ""
Write-Host "安装包创建完成！" -ForegroundColor Green
