@echo off
chcp 65001 >nul
echo ======================================================
echo   产品销量优化与库存管理系统 - 开发版构建脚本
echo ======================================================
echo.

REM 检查 .NET SDK
where dotnet >nul 2>nul
if %errorlevel% neq 0 (
    echo [错误] 未检测到 .NET SDK
    pause
    exit /b 1
)

echo [信息] 检测到 .NET SDK: 
dotnet --version
echo.

REM 还原包
echo [1/3] 还原 NuGet 包...
dotnet restore
if %errorlevel% neq 0 (
    echo [错误] 还原失败
    pause
    exit /b 1
)

REM 编译
echo [2/3] 编译项目...
dotnet build -c Debug
if %errorlevel% neq 0 (
    echo [错误] 编译失败
    pause
    exit /b 1
)

REM 运行
echo [3/3] 运行程序...
echo.
dotnet run --project "%~dp0ProductInventoryOptimizer.csproj"

pause
