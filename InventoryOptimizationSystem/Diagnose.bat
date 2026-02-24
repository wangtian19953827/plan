@echo off
chcp 65001 >nul
echo.
echo =====================================================
echo   PowerShell Diagnostic Test
echo =====================================================
echo.

echo [Test 1] Checking PowerShell availability...
where powershell >nul 2>nul
if %errorlevel% neq 0 (
    echo [ERROR] PowerShell not found
    pause
    exit /b 1
)
echo [OK] PowerShell found
echo.

echo [Test 2] Checking Build.ps1 exists...
if not exist "Build.ps1" (
    echo [ERROR] Build.ps1 not found
    echo.
    echo Current directory:
    cd
    echo.
    echo Files in current directory:
    dir /B *.ps1 *.bat
    echo.
    pause
    exit /b 1
)
echo [OK] Build.ps1 found
echo.

echo [Test 3] Testing PowerShell execution...
powershell -Command "Write-Host 'PowerShell is working'"
if %errorlevel% neq 0 (
    echo [ERROR] PowerShell execution failed
    pause
    exit /b 1
)
echo [OK] PowerShell execution works
echo.

echo [Test 4] Checking execution policy...
for /f "tokens=*" %%i in ('powershell -Command "Get-ExecutionPolicy"') do set POLICY=%%i
echo Current policy: %POLICY%
echo.

echo [Test 5] Testing script with bypass...
echo Running Build.ps1...
echo.

powershell -ExecutionPolicy Bypass -File "Build.ps1"

if %errorlevel% neq 0 (
    echo.
    echo [ERROR] Script failed with error code: %errorlevel%
) else (
    echo.
    echo [OK] Script completed successfully
)

echo.
echo =====================================================
echo   Diagnostic Complete
echo =====================================================
echo.
pause
