# Product Sales Optimization and Inventory Management System - One-click Build Script (.NET 8.0)
# Enhanced version with better error handling

Write-Host ""
Write-Host "========================================================================" -ForegroundColor Cyan
Write-Host "        Product Sales Optimization and Inventory Management" -ForegroundColor Yellow
Write-Host "              One-click Build Script (.NET 8.0)" -ForegroundColor Yellow
Write-Host "========================================================================" -ForegroundColor Cyan
Write-Host ""

# Check if in project directory
$csprojPath = "ProductInventoryOptimizer.csproj"
Write-Host "[Check] Looking for project file..." -ForegroundColor Gray
if (-not (Test-Path $csprojPath)) {
    Write-Host "[ERROR] Project file not found: $csprojPath" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please ensure this script is in project root folder" -ForegroundColor Yellow
    Write-Host "(same folder as ProductInventoryOptimizer.csproj)" -ForegroundColor Yellow
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}
Write-Host "[OK] Project file found" -ForegroundColor Green
Write-Host ""

# Check .NET 8.0 SDK
Write-Host "[Step 1/5] Checking .NET 8.0 SDK..." -ForegroundColor Yellow
try {
    $dotnetVersionOutput = & dotnet --version 2>&1 | Out-String
    $dotnetVersion = $dotnetVersionOutput.Trim()
    
    if ($LASTEXITCODE -eq 0 -and $dotnetVersion) {
        Write-Host "[OK] .NET SDK detected: $dotnetVersion" -ForegroundColor Green
    } else {
        throw "Not installed"
    }
} catch {
    Write-Host "[ERROR] .NET 8.0 SDK not detected" -ForegroundColor Red
    Write-Host ""
    Write-Host "========================================================================" -ForegroundColor Cyan
    Write-Host "                      Install .NET 8.0 SDK" -ForegroundColor Yellow
    Write-Host "========================================================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "This program requires .NET 8.0 SDK to build." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Download: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Cyan
    Write-Host ""
    
    $response = Read-Host "Press Enter to open download page, or enter Q to quit"
    if ($response -ne "Q") {
        Start-Process "https://dotnet.microsoft.com/download/dotnet/8.0"
        Write-Host ""
        Write-Host "Download page opened. Please install .NET 8.0 SDK and run this script again." -ForegroundColor Yellow
        Write-Host ""
        Read-Host "Press Enter to exit"
    }
    exit 1
}
Write-Host ""

# Check .NET version
Write-Host "[Check] Verifying .NET version..." -ForegroundColor Gray
if (-not ($dotnetVersion -match "^8\.")) {
    Write-Host "[WARNING] Detected .NET version is not 8.0: $dotnetVersion" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "This script requires .NET 8.0 SDK" -ForegroundColor Yellow
    Write-Host "Please visit: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Cyan
    Write-Host ""
    $response = Read-Host "Continue to try building? (May fail) (Y/N): "
    if ($response -ne "Y" -and $response -ne "y") {
        exit 1
    }
} else {
    Write-Host "[OK] .NET version check passed" -ForegroundColor Green
}
Write-Host ""

# Build project
Write-Host "========================================================================" -ForegroundColor Cyan
Write-Host "                          Starting Build" -ForegroundColor Yellow
Write-Host "========================================================================" -ForegroundColor Cyan
Write-Host ""

# Clean old build
Write-Host "[Step 2/5] Cleaning old build..." -ForegroundColor Yellow
try {
    if (Test-Path "bin") { 
        Remove-Item -Path "bin" -Recurse -Force 
        Write-Host "[OK] bin folder removed" -ForegroundColor Gray
    }
    if (Test-Path "obj") { 
        Remove-Item -Path "obj" -Recurse -Force 
        Write-Host "[OK] obj folder removed" -ForegroundColor Gray
    }
    Write-Host "[OK] Clean completed" -ForegroundColor Green
} catch {
    Write-Host "[WARNING] Clean failed (but continuing): $_" -ForegroundColor Yellow
}
Write-Host ""

# Restore NuGet packages
Write-Host "[Step 3/5] Restoring NuGet packages..." -ForegroundColor Yellow
Write-Host "This may take a moment..." -ForegroundColor Gray
Write-Host ""

try {
    $restoreOutput = & dotnet restore --no-cache 2>&1 | Out-String
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[ERROR] NuGet package restore failed" -ForegroundColor Red
        Write-Host ""
        Write-Host "Error output:" -ForegroundColor Yellow
        Write-Host $restoreOutput
        Write-Host ""
        Write-Host "Possible solutions:" -ForegroundColor Yellow
        Write-Host "1. Check network connection" -ForegroundColor White
        Write-Host "2. Check proxy settings" -ForegroundColor White
        Write-Host "3. Try using Visual Studio to open the project (VS will restore packages automatically)" -ForegroundColor White
        Write-Host ""
        Read-Host "Press Enter to exit"
        exit 1
    }
    Write-Host "[OK] NuGet packages restored successfully" -ForegroundColor Green
} catch {
    Write-Host "[ERROR] NuGet package restore failed with exception: $_" -ForegroundColor Red
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}
Write-Host ""

# Build project
Write-Host "[Step 4/5] Building project..." -ForegroundColor Yellow
Write-Host "Compiling source code..." -ForegroundColor Gray
Write-Host ""

try {
    $buildOutput = & dotnet build -c Release 2>&1 | Out-String
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[ERROR] Build failed" -ForegroundColor Red
        Write-Host ""
        Write-Host "Build output:" -ForegroundColor Yellow
        Write-Host $buildOutput
        Write-Host ""
        Write-Host "Please check if code has errors" -ForegroundColor Yellow
        Write-Host ""
        Read-Host "Press Enter to exit"
        exit 1
    }
    Write-Host "[OK] Build completed successfully" -ForegroundColor Green
} catch {
    Write-Host "[ERROR] Build failed with exception: $_" -ForegroundColor Red
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}
Write-Host ""

# Publish as standalone EXE
Write-Host "[Step 5/5] Publishing as standalone EXE file..." -ForegroundColor Yellow
Write-Host "This is the last step and may take 1-2 minutes..." -ForegroundColor Yellow
Write-Host "Please be patient..." -ForegroundColor Gray
Write-Host ""

try {
    $publishOutput = & dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true 2>&1 | Out-String
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[ERROR] Publish failed" -ForegroundColor Red
        Write-Host ""
        Write-Host "Publish output:" -ForegroundColor Yellow
        Write-Host $publishOutput
        Write-Host ""
        Read-Host "Press Enter to exit"
        exit 1
    }
    Write-Host "[OK] Publish completed successfully" -ForegroundColor Green
} catch {
    Write-Host "[ERROR] Publish failed with exception: $_" -ForegroundColor Red
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}
Write-Host ""

# Find generated EXE
$exePath = "bin\Release\net8.0-windows\win-x64\publish\ProductInventoryOptimizer.exe"
if (-not (Test-Path $exePath)) {
    Write-Host "[WARNING] EXE file not found at expected location: $exePath" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Searching for EXE file..." -ForegroundColor Yellow
    
    $foundExes = Get-ChildItem -Recurse -Filter "ProductInventoryOptimizer.exe" -ErrorAction SilentlyContinue
    if ($foundExes -and $foundExes.Count -gt 0) {
        $exePath = $foundExes[0].FullName
        Write-Host "[OK] Found EXE at: $exePath" -ForegroundColor Green
    } else {
        Write-Host "[ERROR] No EXE file found!" -ForegroundColor Red
        Write-Host ""
        Write-Host "Please check the build log above for errors" -ForegroundColor Yellow
        Write-Host ""
        Read-Host "Press Enter to exit"
        exit 1
    }
}

if (Test-Path $exePath) {
    $fileInfo = Get-Item $exePath
    $fileSizeMB = [math]::Round($fileInfo.Length / 1MB, 2)
    
    Write-Host "========================================================================" -ForegroundColor Cyan
    Write-Host "                           BUILD SUCCESS!" -ForegroundColor Green
    Write-Host "========================================================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Executable location:" -ForegroundColor White
    Write-Host "  $exePath" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "File size: $fileSizeMB MB" -ForegroundColor Yellow
    Write-Host "Created: $($fileInfo.CreationTime)" -ForegroundColor Gray
    Write-Host ""
    
    Write-Host "========================================================================" -ForegroundColor Cyan
    Write-Host "                            Next Steps" -ForegroundColor Yellow
    Write-Host "========================================================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "  [1] Run program" -ForegroundColor White
    Write-Host "  [2] Open file location" -ForegroundColor White
    Write-Host "  [3] Exit" -ForegroundColor White
    Write-Host ""
    
    $response = ""
    while ($response -notin @("1", "2", "3")) {
        $response = Read-Host "Please select (1-3): "
    }
    
    if ($response -eq "1") {
        Write-Host ""
        Write-Host "Starting program..." -ForegroundColor Yellow
        Write-Host ""
        try {
            Start-Process $exePath
            Write-Host "[OK] Program started successfully!" -ForegroundColor Green
        } catch {
            Write-Host "[ERROR] Failed to start program: $_" -ForegroundColor Red
        }
    } elseif ($response -eq "2") {
        Write-Host ""
        Write-Host "Opening folder..." -ForegroundColor Yellow
        $folder = Split-Path $exePath -Parent
        explorer $folder
    }
    
    Write-Host ""
} else {
    Write-Host "[ERROR] Generated EXE file not found" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please check the build log above for errors" -ForegroundColor Yellow
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host "========================================================================" -ForegroundColor Cyan
Write-Host "                           Thank You" -ForegroundColor Green
Write-Host "========================================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Tip: You can directly run ProductInventoryOptimizer.exe" -ForegroundColor Yellow
Write-Host "     No need to install .NET Runtime (included in EXE)" -ForegroundColor Yellow
Write-Host ""
Read-Host "Press Enter to exit"
