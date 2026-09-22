@echo off
setlocal enabledelayedexpansion

title PasswordX - Smart Password Analyzer ^& Generator

:: Change to script directory
cd /d "%~dp0"

echo ==========================================================
echo          PASSWORDX - ONE-CLICK LAUNCHER
echo    Smart Password Analyzer ^& Generator (CyberEDT 2026)
echo ==========================================================
echo.

:: Detect .NET in user profile if present
if exist "%USERPROFILE%\.dotnet\dotnet.exe" (
    set "PATH=%USERPROFILE%\.dotnet;!PATH!"
    set "DOTNET_ROOT=%USERPROFILE%\.dotnet"
)

:: Verify dotnet CLI exists
where dotnet >nul 2>&1
if !ERRORLEVEL! neq 0 (
    goto :DOTNET_NOT_FOUND
)

:: Check if .NET SDK is available
set "SDK_FOUND="
for /f "tokens=*" %%i in ('dotnet --list-sdks 2^>nul') do (
    if not defined SDK_FOUND set "SDK_FOUND=%%i"
)

if not defined SDK_FOUND (
    goto :DOTNET_SDK_NOT_FOUND
)

echo [OK] .NET SDK detected: !SDK_FOUND!
echo.
echo [INFO] Starting PasswordX web server at http://localhost:5000 ...
echo [INFO] Opening your default browser shortly...
echo [INFO] Press Ctrl+C in this window anytime to stop the server.
echo.
echo ----------------------------------------------------------
echo.

:: Open browser in background after 3-second delay to give server time to initialize
start "" cmd /c "timeout /t 3 /nobreak >nul & start http://localhost:5000"

:: Start the application
dotnet run --urls "http://localhost:5000"

if !ERRORLEVEL! neq 0 (
    echo.
    echo ==========================================================
    echo [ERROR] PasswordX stopped with error code !ERRORLEVEL!.
    echo ==========================================================
    echo.
    pause
)

exit /b 0

:DOTNET_NOT_FOUND
echo.
echo ==========================================================
echo [ERROR] .NET command-line tool was not found!
echo ==========================================================
echo.
echo PasswordX requires the .NET 8.0 SDK to build and run.
echo Please download and install .NET 8.0 SDK from:
echo   https://dotnet.microsoft.com/download/dotnet/8.0
echo.
pause
exit /b 1

:DOTNET_SDK_NOT_FOUND
echo.
echo ==========================================================
echo [ERROR] No .NET SDK found on this computer!
echo ==========================================================
echo.
echo While a .NET runtime might be installed, building and running
echo PasswordX requires the .NET 8.0 SDK.
echo.
echo Please download and install .NET 8.0 SDK from:
echo   https://dotnet.microsoft.com/download/dotnet/8.0
echo.
pause
exit /b 1
