$scriptPath = Join-Path $PSScriptRoot 'tray.ps1'
$ws      = New-Object -ComObject WScript.Shell
$desktop = [Environment]::GetFolderPath('Desktop')
$lnk     = Join-Path $desktop 'WU_TrayIcon.lnk'

$s = $ws.CreateShortcut($lnk)
$s.TargetPath       = 'powershell.exe'
$s.Arguments        = "-NoProfile -STA -WindowStyle Hidden -ExecutionPolicy Bypass -File `"$scriptPath`""
$s.WorkingDirectory = $PSScriptRoot
$s.Description      = 'Windows Update Tray Toggle'
$s.Save()

Write-Host "Installed: $lnk"
