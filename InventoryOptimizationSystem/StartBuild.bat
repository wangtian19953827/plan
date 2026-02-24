@echo off
REM Keep window open using /k instead of /c
cmd /k (
chcp 65001 >nul
echo.
echo ================================================= ==========================================================
echo   Start PowerShell Build Script
echo ================================================= ==========================================================
echo.

REM Check if build script exists
if not exist "Build.ps1" (
    echo [Error] Build script not found: Build.ps1
    echo.
    echo Please make sure this script is in the project root folder (same as ProductInventoryOptimizer.csproj)
    echo.
    pause
    exit /b 1
)

echo [Start] Running build script...
echo.

REM Temporarily set PowerShell execution policy and run script
powershell -ExecutionPolicy Bypass -Command "& {Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process; .\Build.ps1}"

echo.
echo ================================================= ==========================================================
echo   Script execution completed
echo ================================================= ==========================================================
echo.
)
