@echo off
chcp 65001 >nul
echo ======================================================
echo   产品销量优化与库存管理系统 - 自动打包脚本
echo ======================================================
echo.

REM 检查是否安装了 .NET SDK
where dotnet >nul 2>nul
if %errorlevel% neq 0 (
    echo [错误] 未检测到 .NET SDK
    echo.
    echo 请先安装 .NET 8.0 SDK:
    echo https://dotnet.microsoft.com/download/dotnet/8.0
    echo.
    pause
    exit /b 1
)

echo [信息] 检测到 .NET SDK
dotnet --version
echo.

REM 还原 NuGet 包
echo [1/4] 还原 NuGet 包...
dotnet restore
if %errorlevel% neq 0 (
    echo [错误] 还原包失败
    pause
    exit /b 1
)
echo [完成] NuGet 包还原成功
echo.

REM 编译项目
echo [2/4] 编译项目...
dotnet build -c Release
if %errorlevel% neq 0 (
    echo [错误] 编译失败
    pause
    exit /b 1
)
echo [完成] 编译成功
echo.

REM 打包为独立 EXE
echo [3/4] 打包为独立 EXE 文件...
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
if %errorlevel% neq 0 (
    echo [错误] 打包失败
    pause
    exit /b 1
)
echo [完成] 打包成功
echo.

REM 创建发布文件夹
set PUBLISH_DIR=Release_Package
if exist "%~dp0%PUBLISH_DIR%" rd /s /q "%~dp0%PUBLISH_DIR%"
mkdir "%~dp0%PUBLISH_DIR%"

REM 复制文件
echo [4/4] 整理发布文件...
copy /Y "%~dp0bin\Release\net8.0-windows\win-x64\publish\ProductInventoryOptimizer.exe" "%~dp0%PUBLISH_DIR\" >nul
copy /Y "%~dp0快速启动.md" "%~dp0%PUBLISH_DIR\" >nul
copy /Y "%~dp0Excel模板说明.md" "%~dp0%PUBLISH_DIR\" >nul
copy /Y "%~dp0项目说明.md" "%~dp0%PUBLISH_DIR\" >nul

echo [完成] 发布文件整理完成
echo.

REM 显示结果
echo ======================================================
echo   打包完成！
echo ======================================================
echo.
echo 发布文件位于:
echo %~dp0%PUBLISH_DIR%\
echo.
echo 主要文件:
echo   - ProductInventoryOptimizer.exe (主程序)
echo   - 快速启动.md (使用指南)
echo   - Excel模板说明.md (Excel导入说明)
echo   - 项目说明.md (完整文档)
echo.
echo 您可以直接运行 Release_Package\ProductInventoryOptimizer.exe
echo 或将整个 Release_Package 文件夹打包分发
echo.
echo 按任意键打开发布文件夹...
pause >nul

explorer "%~dp0%PUBLISH_DIR%"
