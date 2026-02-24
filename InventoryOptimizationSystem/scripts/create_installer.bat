@echo off
chcp 65001 >nul

echo ======================================================
echo   产品销量优化与库存管理系统 - 创建安装包
echo ======================================================
echo.

set PROJECT_DIR=%~dp0..
set BUILD_DIR=%PROJECT_DIR%\Release_Package
set OUTPUT_DIR=%PROJECT_DIR%\Installer

REM 检查构建目录
if not exist "%BUILD_DIR%" (
    echo [错误] 未找到构建文件
    echo 请先运行 build.bat 进行构建
    echo.
    pause
    exit /b 1
)

echo [信息] 清理旧的安装包...
if exist "%OUTPUT_DIR%" rd /s /q "%OUTPUT_DIR%"
mkdir "%OUTPUT_DIR%"

REM 复制文件
echo [1/3] 复制程序文件...
xcopy /E /I /Y "%BUILD_DIR%" "%OUTPUT_DIR%\ProductInventoryOptimizer" >nul

echo [2/3] 创建安装说明...
(
echo 产品销量优化与库存管理系统 - 安装说明
echo ======================================================
echo.
echo 版本: 1.0.0
echo 更新日期: %date% %time%
echo.
echo ======================================================
echo   快速开始
echo ======================================================
echo.
echo 1. 双击运行 ProductInventoryOptimizer.exe
echo 2. 程序会自动加载示例数据（80个产品）
echo 3. 点击"开始计算"查看优化结果
echo.
echo ======================================================
echo   详细文档
echo ======================================================
echo.
echo - 快速启动.md: 开发者和用户快速上手指南
echo - Excel模板说明.md: Excel导入导出格式说明
echo - 项目说明.md: 完整的功能和技术文档
echo.
echo ======================================================
echo   系统要求
echo ======================================================
echo.
echo - Windows 10 或 Windows 11
echo - 4GB 内存（推荐 8GB）
echo - 500MB 硬盘空间
echo.
echo ======================================================
echo   注意事项
echo ======================================================
echo.
echo - 本程序为独立 EXE，无需安装 .NET
echo - 程序运行在本地，无需联网
echo - 所有数据保存在本地，安全可靠
echo.
) > "%OUTPUT_DIR%\README.txt"

echo [3/3] 创建卸载脚本...
(
echo @echo off
echo chcp 65001 ^>nul
echo.
echo echo ======================================================
echo echo   产品销量优化与库存管理系统 - 卸载
echo echo ======================================================
echo echo.
echo echo 确认要删除程序吗？
echo echo.
echo choice /C YN /N /M "按 Y 确认删除，按 N 取消: "
echo if %%errorlevel%% equ 2 exit /b 0
echo.
echo echo 正在删除...
echo cd /d "%%~dp0409646289168725"
echo cd ..
echo rd /s /q "ProductInventoryOptimizer"
echo.
echo echo 卸载完成！
echo pause
) > "%OUTPUT_DIR%\uninstall.bat"

echo.
echo ======================================================
echo   安装包创建成功！
echo ======================================================
echo.
echo 安装包位于: %OUTPUT_DIR%
echo.
echo 主要内容:
echo   - ProductInventoryOptimizer\  (程序文件)
echo   - README.txt                   (安装说明)
echo   - uninstall.bat                (卸载脚本)
echo.
echo 您可以将 %OUTPUT_DIR% 文件夹打包为 ZIP 分发
echo.
echo 按任意键打开安装包文件夹...
pause >nul

explorer "%OUTPUT_DIR%"
