@echo off
setlocal enabledelayedexpansion
chcp 65001 >nul 2>&1

title ii Reborn Installer // [#---------] Getting directory
color 0e

set "steamPath1=C:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag"
set "steamPath2=D:\SteamLibrary\steamapps\common\Gorilla Tag"
set "steamPath3=C:\Program Files\Oculus\Software\Software\another-axiom-gorilla-tag"
set "steamPath4=D:\Steam\steamapps\common\Gorilla Tag"

if exist "!steamPath1!" (
    set "gamePath=!steamPath1!"
    goto :gotpath
)
if exist "!steamPath2!" (
    set "gamePath=!steamPath2!"
    goto :gotpath
)
if exist "!steamPath3!" (
    set "gamePath=!steamPath3!"
    goto :gotpath
)
if exist "!steamPath4!" (
    set "gamePath=!steamPath4!"
    goto :gotpath
)

color 0c
set /p gamePath=Gorilla Tag directory not found. Enter it manually:
set gamePath=%gamePath:"=%
if not exist "!gamePath!" (
    echo Invalid directory.
    pause
    exit /b
)

:gotpath
color 0e
cls
title ii Reborn Installer // [###-------] Downloading BepInEx

echo.
echo  ██ ██     ██████  ███████ ██████   ██████  ██████  ███    ██ 
echo  ██ ██     ██   ██ ██      ██   ██ ██    ██ ██   ██ ████   ██ 
echo  ██ ██     ██████  █████   ██████  ██    ██ ██████  ██ ██  ██ 
echo  ██ ██     ██   ██ ██      ██   ██ ██    ██ ██   ██ ██  ██ ██ 
echo  ██ ██     ██   ██ ███████ ██████   ██████  ██   ██ ██   ████ 
echo.
echo        ii Reborn - Installer
echo        github.com/iireborn/menu
echo.

curl -L -f -# "https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.4/BepInEx_win_x64_5.4.23.4.zip" -o BPNX54234.zip

if errorlevel 1 (
    color 0c
    echo.
    echo Failed to download BepInEx, check your internet connection.
    pause
    exit /b
)

powershell -command "Expand-Archive -Path 'BPNX54234.zip' -DestinationPath '%gamePath%' -Force"

cls
title ii Reborn Installer // [####------] Creating directories
echo Creating BepInEx directories...
if not exist "%gamePath%\BepInEx\config" mkdir "%gamePath%\BepInEx\config"
if not exist "%gamePath%\BepInEx\plugins" mkdir "%gamePath%\BepInEx\plugins"

cls
title ii Reborn Installer // [#######---] Downloading menu
echo Downloading latest release of ii Reborn...

for /f "usebackq delims=" %%i in (`powershell -NoProfile -Command "(Invoke-WebRequest -UseBasicParsing -Uri 'https://github.com/iireborn/menu/raw/refs/heads/main/menuversion.json').Content | ConvertFrom-Json | Select-Object -ExpandProperty downloadUrl"`) do (
    set pluginUrl=%%i
)

if "%pluginUrl%"=="" (
    color 0c
    echo.
    echo Failed to get latest release of menu, please report to Discord
    pause
    exit /b
)

for /r "%gamePath%\BepInEx\plugins" %%i in (ii*.dll) do (
    del /f /q "%%i"
)

curl -L -f -# "%pluginUrl%" -o "%gamePath%\BepInEx\plugins\ii.Reborn.dll"

if errorlevel 1 (
    color 0c
    echo.
    echo Failed to download the menu, please report to Discord
    pause
    exit /b
)

cls
title ii Reborn Installer // [##########] Finished

echo.
echo  ██ ██     ██████  ███████ ██████   ██████  ██████  ███    ██ 
echo  ██ ██     ██   ██ ██      ██   ██ ██    ██ ██   ██ ████   ██ 
echo  ██ ██     ██████  █████   ██████  ██    ██ ██████  ██ ██  ██ 
echo  ██ ██     ██   ██ ██      ██   ██ ██    ██ ██   ██ ██  ██ ██ 
echo  ██ ██     ██   ██ ███████ ██████   ██████  ██   ██ ██   ████ 
echo.
echo        ii Reborn - Installer
echo.

echo Congratulations, you now have the menu!
echo.
echo Launch Gorilla Tag and the menu will load automatically.
echo.
del "BPNX54234.zip" >nul 2>&1
pause
