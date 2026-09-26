$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'
try { [Net.ServicePointManager]::SecurityProtocol = [Net.ServicePointManager]::SecurityProtocol -bor 3072 } catch {} # TLS 1.2 for PS 5.1

$ManifestUrl = 'https://github.com/iireborn/menu/raw/refs/heads/main/menuversion.json'
$BepInExUrl  = 'https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.4/BepInEx_win_x64_5.4.23.4.zip'
$ScriptUrl   = 'https://github.com/iireborn/menu/raw/refs/heads/main/install.ps1'

function Fail($msg) { Write-Host "`n$msg" -ForegroundColor Red; Read-Host 'Press Enter to exit'; exit 1 }

Write-Host ''
Write-Host '  ii Reborn - Installer' -ForegroundColor Yellow
Write-Host '  github.com/iireborn/menu'
Write-Host ''

# -- locate game (a candidate only counts if the game exe is actually there) --
$candidates = @(
    'C:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag',
    'D:\SteamLibrary\steamapps\common\Gorilla Tag',
    'C:\Program Files\Oculus\Software\Software\another-axiom-gorilla-tag',
    'D:\Steam\steamapps\common\Gorilla Tag'
)
$found = @($candidates | Where-Object { Test-Path "$($_)\Gorilla Tag.exe" })
if ($found.Count -eq 0) {
    $gamePath = (Read-Host 'Gorilla Tag directory not found. Enter it manually').Trim('"')
    if (-not (Test-Path $gamePath)) { Fail 'Invalid directory.' }
} elseif ($found.Count -eq 1) {
    $gamePath = $found[0]
} else {
    Write-Host 'Multiple Gorilla Tag installations found:' -ForegroundColor Yellow
    for ($i = 0; $i -lt $found.Count; $i++) {
        Write-Host ('  [{0}] {1}' -f ($i + 1), $found[$i])
    }
    $pick = 0
    while ($true) {
        $answer = (Read-Host "Choose [1-$($found.Count)] (Enter = 1)").Trim()
        if ($answer -eq '') { $pick = 1; break }
        if ([int]::TryParse($answer, [ref]$pick) -and $pick -ge 1 -and $pick -le $found.Count) { break }
        Write-Host 'Invalid choice.' -ForegroundColor Red
    }
    $gamePath = $found[$pick - 1]
}
Write-Host "Game directory: $gamePath`n"

# -- version manifest (fetched early so we fail before touching anything) --
Write-Host 'Checking the latest version...' -ForegroundColor Cyan
try {
    $manifest  = (Invoke-WebRequest -UseBasicParsing -Uri $ManifestUrl).Content | ConvertFrom-Json
    $pluginUrl = $manifest.downloadUrl
} catch { Fail 'Failed to fetch the version manifest.' }
if ([string]::IsNullOrEmpty($pluginUrl)) { Fail 'Manifest did not contain a downloadUrl.' }

# -- already up to date here? maybe this is the wrong game folder --
$menuDll = "$gamePath\BepInEx\plugins\ii.Reborn.dll"
if (-not [string]::IsNullOrEmpty($manifest.sha256) -and (Test-Path $menuDll)) {
    $localHash = ''
    try { $localHash = (Get-FileHash -Path $menuDll -Algorithm SHA256).Hash } catch {}
    if ($localHash -eq $manifest.sha256) {
        Write-Host ''
        Write-Host 'ii Reborn is already updated in this folder.' -ForegroundColor Yellow
        Write-Host 'Have you installed it but nothing showed up in game? Then this installer is probably'
        Write-Host 'using the wrong game folder!'
        Write-Host ''
        Write-Host 'To find the right one in Steam:'
        Write-Host '  1. Open Steam and right-click Gorilla Tag'
        Write-Host '  2. Manage -> Browse local files   (a folder window opens)'
        Write-Host '  3. Click the address bar and copy the full path'
        Write-Host ''
        $alt = (Read-Host 'Paste the full folder path to "Gorilla Tag" here (or press Enter to keep this folder)').Trim().Trim('"')
        if ($alt -ne '') {
            if (Test-Path $alt) {
                $gamePath = $alt
                Write-Host "Game directory: $gamePath`n"
            } else {
                Write-Host 'That path does not exist - keeping the folder found earlier.' -ForegroundColor Red
            }
        }
    }
}

# -- bepinex --
Write-Host 'Downloading BepInEx...' -ForegroundColor Cyan
$zip = Join-Path $env:TEMP 'iireborn-bepinex.zip'
try {
    Invoke-WebRequest -UseBasicParsing -Uri $BepInExUrl -OutFile $zip

    Write-Host 'Extracting BepInEx...' -ForegroundColor Cyan

    # NOTE: Expand-Archive (PS 5.1) misjoins dot leading entries (e.g. '.doorstop_version' -> 'Gorilla Tag.doorstop_version').
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $archive = [System.IO.Compression.ZipFile]::OpenRead($zip)
    try {
        foreach ($entry in $archive.Entries) {
            if ([string]::IsNullOrEmpty($entry.Name)) { continue }   # directory stubs
            $rel = $entry.FullName -replace '/', '\'
            $target = $gamePath + '\' + $rel
            $slash = $rel.LastIndexOf('\')
            if ($slash -ge 0) {
                $targetDir = $gamePath + '\' + $rel.Substring(0, $slash)
                if (-not (Test-Path $targetDir)) { New-Item -ItemType Directory -Force -Path $targetDir | Out-Null }
            }
            [System.IO.Compression.ZipFileExtensions]::ExtractToFile($entry, $target, $true)
        }
    } finally { $archive.Dispose() }
} catch {
    # extraction failed (typically write access denied) -> one elevated retry
    if ($env:IIREBORN_ELEVATED -eq '1') { Fail "Failed to download/extract BepInEx ($($_.Exception.Message))" }
    Write-Host "BepInEx step failed: $($_.Exception.Message)" -ForegroundColor Yellow
    Write-Host 'Relaunching as administrator - accept the UAC prompt!!!' -ForegroundColor Yellow
    $child = '-NoProfile -Command "$env:IIREBORN_ELEVATED=''1''; irm ''' + $ScriptUrl + ''' | iex"'
    try {
        Start-Process powershell -Verb RunAs -ArgumentList $child
    } catch {
        Fail 'Administrator access was declined. Re-run the installer and accept the UAC prompt.'
    }
    exit
}
Remove-Item $zip -ErrorAction SilentlyContinue

New-Item -ItemType Directory -Force -Path "$gamePath\BepInEx\config", "$gamePath\BepInEx\plugins" | Out-Null

# -- clean stale menu DLLs and directories --
Get-ChildItem -Path "$gamePath\BepInEx\plugins" -Filter 'ii*.dll' -Recurse -File -ErrorAction SilentlyContinue |
    Remove-Item -Force -ErrorAction SilentlyContinue
Get-ChildItem -Path "$gamePath\BepInEx\plugins" -Filter 'ii*' -Directory -ErrorAction SilentlyContinue |
    Remove-Item -Recurse -Force -ErrorAction SilentlyContinue

# -- menu --
Write-Host 'Downloading ii Reborn...' -ForegroundColor Cyan
try {
    Invoke-WebRequest -UseBasicParsing -Uri $pluginUrl -OutFile "$gamePath\BepInEx\plugins\ii.Reborn.dll"
} catch { Fail "Failed to download the menu ($($_.Exception.Message))" }

Write-Host ''
Write-Host 'Congratulations, you now have the menu!' -ForegroundColor Green
Write-Host 'Launch Gorilla Tag and the menu will load automatically.'
Write-Host ''
Read-Host 'All good, press Enter to exit or close this window'
