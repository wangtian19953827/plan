@echo off
REM 产品销量优化与库存管理系统 - 一键安装脚本
REM 用途: 检查环境并启动构建

chcp 65001 >nul

echo ======================================================
echo   产品销量优化与库存管理系统 - 安装构建
echo ======================================================
echo.

REM 检查是否为 Windows
if not exist "%SystemRoot%\System32\cmd.exe" (
    echo [错误] 此脚本仅支持 Windows 系统
    pause
    exit /b 1
)

echo [信息] 操作系统: Windows
ver
echo.

REM 检查 .NET SDK
echo [检查] 检测 .NET SDK...
where dotnet >nul 2>nul
if %errorlevel% neq 0 (
    echo.
    echo ======================================================
    echo   未检测到 .NET SDK
    echo ======================================================
    echo.
    echo 此程序需要 .NET 8.0 SDK 才能编译。
    echo.
    echo 请选择安装方式:
    echo.
    echo   [1] 打开 .NET 8.0 SDK 下载页面
    echo   [2] 退出
    echo.
    choice /C 12 /N /M "请输入选项 (1-2): "
    
    if %errorlevel% equ 1 (
        start https://dotnet.microsoft.com/download/dotnet/8.0
        echo.
        echo 已打开下载页面，请安装 .NET 8.0 SDK 后重新运行此脚本。
        pause
        exit /b 1
    )
    
    if %errorlevel% equ 2 (
        exit /b 0
    )
) else (
    dotnet --version
    echo [完成] .NET SDK 已安装
    echo.
)

REM 运行构建脚本
echo.
echo ======================================================
echo   开始构建...
echo ======================================================
echo.

call "%~dp0build.bat"

if %errorlevel% neq 0 (
    echo.
    echo [错误] 构建失败
    pause
    exit /b 1
)

echo.
echo ======================================================
echo   构建成功！
echo ======================================================
echo.
echo 发布文件位于:
echo %~dp0Release_Package\
echo.
echo 您现在可以运行 ProductInventoryOptimizer.exe
echo.
pause
