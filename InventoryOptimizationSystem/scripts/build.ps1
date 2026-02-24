# 产品销量优化与库存管理系统 - PowerShell 构建脚本
# 用途: 打包为独立 EXE 文件

$ErrorActionPreference = "Stop"

Write-Host "======================================================" -ForegroundColor Cyan
Write-Host "  产品销量优化与库存管理系统 - 自动打包脚本" -ForegroundColor Cyan
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host ""

# 检查 .NET SDK
Write-Host "[检查] 检测 .NET SDK..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "[✓] 检测到 .NET SDK: $dotnetVersion" -ForegroundColor Green
    } else {
        throw "未安装"
    }
} catch {
    Write-Host "[✗] 未检测到 .NET SDK" -ForegroundColor Red
    Write-Host ""
    Write-Host "请先安装 .NET 8.0 SDK:" -ForegroundColor Yellow
    Write-Host "https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Cyan
    Write-Host ""
    Read-Host "按 Enter 键退出"
    exit 1
}
Write-Host ""

# 项目路径
$projectPath = $PSScriptRoot
$projectFile = Join-Path $projectPath "ProductInventoryOptimizer.csproj"

# 检查项目文件
if (-not (Test-Path $projectFile)) {
    Write-Host "[✗] 找不到项目文件: $projectFile" -ForegroundColor Red
    Read-Host "按 Enter 键退出"
    exit 1
}

# 1. 还原 NuGet 包
Write-Host "[1/4] 还原 NuGet 包..." -ForegroundColor Yellow
dotnet restore $projectFile
if ($LASTEXITCODE -ne 0) {
    Write-Host "[✗] 还原包失败" -ForegroundColor Red
    Read-Host "按 Enter 键退出"
    exit 1
}
Write-Host "[✓] NuGet 包还原成功" -ForegroundColor Green
Write-Host ""

# 2. 清理旧的构建
Write-Host "[2/4] 清理旧构建..." -ForegroundColor Yellow
dotnet clean $projectFile -c Release
Write-Host "[✓] 清理完成" -ForegroundColor Green
Write-Host ""

# 3. 编译项目
Write-Host "[3/4] 编译项目..." -ForegroundColor Yellow
dotnet build $projectFile -c Release --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "[✗] 编译失败" -ForegroundColor Red
    Read-Host "按 Enter 键退出"
    exit 1
}
Write-Host "[✓] 编译成功" -ForegroundColor Green
Write-Host ""

# 4. 打包为独立 EXE
Write-Host "[4/4] 打包为独立 EXE 文件..." -ForegroundColor Yellow
$publishArgs = @(
    "publish",
    $projectFile,
    "-c", "Release",
    "-r", "win-x64",
    "--self-contained",
    "-p:PublishSingleFile=true"
)
& dotnet @publishArgs

if ($LASTEXITCODE -ne 0) {
    Write-Host "[✗] 打包失败" -ForegroundColor Red
    Read-Host "按 Enter 键退出"
    exit 1
}
Write-Host "[✓] 打包成功" -ForegroundColor Green
Write-Host ""

# 查找生成的 EXE
$exePath = Join-Path $projectPath "bin\Release\net8.0-windows\win-x64\publish\ProductInventoryOptimizer.exe"

if (-not (Test-Path $exePath)) {
    Write-Host "[✗] 找不到生成的 EXE 文件" -ForegroundColor Red
    Read-Host "按 Enter 键退出"
    exit 1
}

# 获取文件大小
$fileSize = (Get-Item $exePath).Length / 1MB

# 创建发布目录
$publishDir = Join-Path $projectPath "Release_Package"
if (Test-Path $publishDir) {
    Remove-Item -Path $publishDir -Recurse -Force
}
New-Item -ItemType Directory -Path $publishDir | Out-Null

# 复制文件
Write-Host "整理发布文件..." -ForegroundColor Yellow
Copy-Item -Path $exePath -Destination $publishDir -Force

# 复制文档
$docs = @("快速启动.md", "Excel模板说明.md", "项目说明.md")
foreach ($doc in $docs) {
    $docPath = Join-Path $projectPath $doc
    if (Test-Path $docPath) {
        Copy-Item -Path $docPath -Destination $publishDir -Force
    }
}

Write-Host "[✓] 发布文件整理完成" -ForegroundColor Green
Write-Host ""

# 显示结果
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host "  打包完成！" -ForegroundColor Green
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "发布文件夹:" -ForegroundColor White
Write-Host "  $publishDir" -ForegroundColor Cyan
Write-Host ""
Write-Host "主要文件:" -ForegroundColor White
Write-Host "  - ProductInventoryOptimizer.exe ($([math]::Round($fileSize, 2)) MB)" -ForegroundColor Green
Write-Host "  - 快速启动.md" -ForegroundColor White
Write-Host "  - Excel模板说明.md" -ForegroundColor White
Write-Host "  - 项目说明.md" -ForegroundColor White
Write-Host ""
Write-Host "使用方法:" -ForegroundColor White
Write-Host "  1. 直接运行 ProductInventoryOptimizer.exe" -ForegroundColor Yellow
Write-Host "  2. 或将整个 Release_Package 文件夹打包分发" -ForegroundColor Yellow
Write-Host ""

# 询问是否打开发布文件夹
$response = Read-Host "是否打开发布文件夹? (Y/N)"
if ($response -eq "Y" -or $response -eq "y") {
    explorer $publishDir
}

Write-Host ""
Write-Host "打包完成！" -ForegroundColor Green
