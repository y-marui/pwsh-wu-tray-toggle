Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

# --- レジストリパス確保関数 ---
function Ensure-RegistryPath {
    param([string]$Path)
    if (-not (Test-Path $Path)) {
        New-Item -Path $Path -Force -ErrorAction SilentlyContinue | Out-Null
    }
}

# --- アイコン生成関数 ---
function Get-MyIcon {
    param([string]$mode)
    $bmp = New-Object Drawing.Bitmap 64, 64
    $g = [Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = 'AntiAlias'
    
    if ($mode -eq 'Running') {
        $p = New-Object Drawing.Pen ([Drawing.Color]::RoyalBlue), 8
        $g.DrawArc($p, 10, 10, 44, 44, 0, 300)
        $pts = @(
            (New-Object Drawing.Point 54, 32),
            (New-Object Drawing.Point 44, 42),
            (New-Object Drawing.Point 64, 42)
        )
        $g.FillPolygon([Drawing.Brushes]::RoyalBlue, $pts)
    }
    else {
        $p = New-Object Drawing.Pen ([Drawing.Color]::Red), 10
        $g.DrawLine($p, 10, 10, 54, 54)
        $g.DrawLine($p, 54, 10, 10, 54)
    }
    
    $g.Dispose()
    return [Drawing.Icon]::FromHandle($bmp.GetHicon())
}

# --- 通知アイコン設定 ---
$ni = New-Object Windows.Forms.NotifyIcon
$ni.Visible = $true

# --- 表示更新関数 ---
function Update-Display {
    $regPath = 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU'
    
    # レジストリパスが存在しない場合のデフォルト値
    $noAutoUpdate = 0
    
    if (Test-Path $regPath) {
        $reg = Get-ItemProperty -Path $regPath -Name 'NoAutoUpdate' -ErrorAction SilentlyContinue
        if ($reg -and $null -ne $reg.NoAutoUpdate) {
            $noAutoUpdate = $reg.NoAutoUpdate
        }
    }
    
    if ($noAutoUpdate -eq 1) {
        $ni.Icon = Get-MyIcon 'Stopped'
        $ni.Text = 'WU: 停止中 (制御モード)'
    }
    else {
        $ni.Icon = Get-MyIcon 'Running'
        $ni.Text = 'WU: 稼働中 (通常モード)'
    }
}

# --- メニュー ---
$cms = New-Object Windows.Forms.ContextMenuStrip

$mCheck = $cms.Items.Add('現在の状態を確認')
$cms.Items.Add('-') | Out-Null
$mStop = $cms.Items.Add('停止 (制御開始)')
$mStart = $cms.Items.Add('再開 (通常)')
$cms.Items.Add('-') | Out-Null
$mExit = $cms.Items.Add('終了')

# --- 状態確認 ---
$mCheck.Add_Click({
    $regPath = 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU'
    $noAutoUpdate = 0
    
    if (Test-Path $regPath) {
        $reg = Get-ItemProperty -Path $regPath -Name 'NoAutoUpdate' -ErrorAction SilentlyContinue
        if ($reg -and $null -ne $reg.NoAutoUpdate) {
            $noAutoUpdate = $reg.NoAutoUpdate
        }
    }
    
    $svc = Get-Service wuauserv -ErrorAction SilentlyContinue
    
    $regText = if ($noAutoUpdate -eq 1) { '1 (停止)' } else { '0 (稼働中)' }
    $svcText = if ($svc) {
        if ($svc.Status -eq 'Running') { '実行中' } else { '停止中' }
    } else {
        'サービス不明'
    }
    
    [Windows.Forms.MessageBox]::Show(
        "【現在の詳細状態】`nポリシー(NoAutoUpdate): $regText`nサービス状態: $svcText",
        "WU 状態確認"
    )
})

# --- 停止 ---
$mStop.Add_Click({
    $cmd = @'
$path = 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU'
if (-not (Test-Path $path)) {
    New-Item -Path $path -Force | Out-Null
}
Set-ItemProperty -Path $path -Name 'NoAutoUpdate' -Value 1 -Type DWord
Stop-Service wuauserv -Force -ErrorAction SilentlyContinue
'@
    Start-Process powershell -ArgumentList "-NoProfile -Command $cmd" -Verb RunAs -Wait
    Start-Sleep -Seconds 1
    Update-Display
})

# --- 再開 ---
$mStart.Add_Click({
    $cmd = @'
$path = 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU'
if (-not (Test-Path $path)) {
    New-Item -Path $path -Force | Out-Null
}
Set-ItemProperty -Path $path -Name 'NoAutoUpdate' -Value 0 -Type DWord
Set-Service wuauserv -StartupType Manual -ErrorAction SilentlyContinue
Start-Service wuauserv -ErrorAction SilentlyContinue
'@
    Start-Process powershell -ArgumentList "-NoProfile -Command $cmd" -Verb RunAs -Wait
    Start-Sleep -Seconds 1
    Update-Display
})

# --- 終了 ---
$mExit.Add_Click({
    $ni.Visible = $false
    $ni.Dispose()
    [Windows.Forms.Application]::Exit()
})

$ni.ContextMenuStrip = $cms
Update-Display

# --- 常駐実行 ---
$f = New-Object Windows.Forms.Form
$f.ShowInTaskbar = $false
$f.WindowState = 'Minimized'
[Windows.Forms.Application]::Run($f)