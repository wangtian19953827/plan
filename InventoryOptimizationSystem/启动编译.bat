@echo off
REM 保持窗口打开，使用 /k 而不是 /c
cmd /k (
chcp 65001 >nul
echo.
echo ================================================= ==========================================================
echo   启动 PowerShell 编译脚本
echo ================================================= ==========================================================
echo.

REM 检查是否在项目目录
if not exist "一键编译.ps1" (
    echo [错误] 未找到编译脚本: 一键编译.ps1
    echo.
    echo 请确保此脚本位于项目根目录（与 ProductInventoryOptimizer.csproj 同级）
    echo.
    pause
    exit /b 1
)

echo [开始] 运行编译脚本...
echo.

REM 临时设置 PowerShell 执行策略并运行脚本
powershell -ExecutionPolicy Bypass -Command "& {Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process; .\一键编译.ps1}"

echo.
echo ================================================= ==========================================================
echo   脚本执行完成
echo ================================================= ==========================================================
echo.
)
