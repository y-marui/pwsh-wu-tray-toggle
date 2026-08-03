.PHONY: install uninstall build

build:
	dotnet publish src/WuTrayToggle -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o dist

install: build
	dist/WuTrayToggle.exe --install

uninstall:
	powershell -NoProfile -Command "$$desktop = [Environment]::GetFolderPath('Desktop'); Remove-Item (Join-Path $$desktop 'WU_TrayIcon.lnk') -ErrorAction SilentlyContinue; Write-Host 'Uninstalled WU_TrayIcon.lnk'"
