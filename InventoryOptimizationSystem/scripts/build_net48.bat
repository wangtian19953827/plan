@echo off
chcp 65001 >nul
echo ======================================================
echo   产品销量优化与库存管理系统 (.NET 4.8)
echo   自动构建脚本
echo ======================================================
echo.

REM 检查 MSBuild
where msbuild >nul 2>nul
if %errorlevel% neq 0 (
    echo [错误] 未检测到 MSBuild
    echo.
    echo 请确保已安装 Visual Studio 2017/2019/2022
    echo 或 .NET Framework 4.8 SDK
    echo.
    pause
    exit /b 1
)

echo [信息] 检测到 MSBuild
msbuild -version 2>nul
if %errorlevel% neq 0 (
    echo [提示] 无法获取 MSBuild 版本
) else (
    echo [信息] MSBuild 版本:
    msbuild -version
)
echo.

REM 还原 NuGet 包
echo [1/4] 还原 NuGet 包...
"%~dp0..\packages\NuGet.exe" restore "%~dp0..\ProductInventoryOptimizer.csproj" -PackagesDirectory "%~dp0..\packages"
if %errorlevel% neq 0 (
    echo [提示] NuGet 包还原可能失败，尝试继续...
)

REM 检查 NuGet.exe
if not exist "%~dp0..\packages\NuGet.exe" (
    echo [警告] NuGet.exe 不存在，请先下载 NuGet.exe
    echo.
    echo 下载地址: https://www.nuget.org/downloads
    echo.
    echo 或者使用 Visual Studio 打开项目，它会自动还原 NuGet 包
    echo.
    choice /C YN /N /M "是否继续构建 (可能失败)? (Y/N): "
    if %errorlevel% neq 1 (
        exit /b 1
    )
)

echo [完成] NuGet 包还原
echo.

REM 清理旧的构建
echo [2/4] 清理旧构建...
msbuild "%~dp0..\ProductInventoryOptimizer.csproj" /t:Clean /p:Configuration=Release
echo [完成] 清理完成
echo.

REM 编译项目
echo [3/4] 编译项目...
msbuild "%~dp0..\ProductInventoryOptimizer.csproj" /t:Build /p:Configuration=Release /p:Platform="AnyCPU"
if %errorlevel% neq 0 (
    echo [错误] 编译失败
    echo.
    echo 请检查:
    echo 1. 是否安装了 Visual Studio 2017+
    echo 2. 是否安装了 .NET Framework 4.8
    echo 3. NuGet 包是否正确还原
    echo.
    pause
    exit /b 1
)
echo [完成] 编译成功
echo.

REM 创建发布文件夹
set PUBLISH_DIR=%~dp0..\Release_Package
if exist "%PUBLISH_DIR%" rd /s /q "%PUBLISH_DIR%"
mkdir "%PUBLISH_DIR%"

REM 复制文件
echo [4/4] 整理发布文件...
copy /Y "%~dp0..\bin\Release\ProductInventoryOptimizer.exe" "%PUBLISH_DIR%\" >nul
copy /Y "%~dp0..\快速启动.md" "%PUBLISH_DIR%\" >nul
copy /Y "%~dp0..\Excel模板说明.md" "%PUBLISH_DIR%\" >nul
copy /Y "%~dp0..\项目说明.md" "%PUBLISH_DIR%\" >nul

REM 复制依赖 DLL
for /r "%~dp0..\bin\Release" %%f in (*.dll) do (
    copy /Y "%%f" "%PUBLISH_DIR%\" >nul
)

echo [完成] 发布文件整理完成
echo.

REM 显示结果
echo ======================================================
echo   打包完成！
echo ======================================================
echo.
echo 发布文件位于:
echo %PUBLISH_DIR%\
echo.
echo 主要文件:
dir /B "%PUBLISH_DIR%" | findstr /E ".exe .dll .md"
echo.
echo 您可以复制 Release_Package 文件夹到其他 Windows 机器使用
echo 注意: 目标机器需要安装 .NET Framework 4.8 (Windows 10/11 已预装)
echo.
echo 按任意键打开发布文件夹...
pause >nul

explorer "%PUBLISH_DIR%"
