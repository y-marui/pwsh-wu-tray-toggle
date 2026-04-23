.PHONY: install uninstall

install:
	powershell -NoProfile -ExecutionPolicy Bypass -File src/install.ps1

uninstall:
	powershell -NoProfile -Command "$$desktop = [Environment]::GetFolderPath('Desktop'); Remove-Item (Join-Path $$desktop 'WU_TrayIcon.lnk') -ErrorAction SilentlyContinue; Write-Host 'Uninstalled WU_TrayIcon.lnk'"
