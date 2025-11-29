# Description: Gaming Tweaks – optimize Windows 11 for games
# Host: console
# Options: Enable Game Mode; Disable Game Mode; Toggle Hardware GPU Scheduling; Switch Power Plans; Disable Game DVR; Clear Shader Cache

param([string]$choice)

function Is-Admin {
    $id = [Security.Principal.WindowsIdentity]::GetCurrent()
    $pr = New-Object Security.Principal.WindowsPrincipal($id)
    return $pr.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}

switch ($choice) {

    "Enable Game Mode" {
        Write-Output "✔ Enabling Game Mode..."
        Set-ItemProperty -Path "HKCU:\Software\Microsoft\GameBar" -Name "AllowAutoGameMode" -Value 1
    }

    "Disable Game Mode" {
        Write-Output "✔ Disabling Game Mode..."
        Set-ItemProperty -Path "HKCU:\Software\Microsoft\GameBar" -Name "AllowAutoGameMode" -Value 0
    }

    "Toggle Hardware GPU Scheduling" {
        if (-not (Is-Admin)) {
            Write-Output "✘ Requires Administrator. Please run Flyoobe as Admin."
            break
        }

        $path = "HKLM:\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"
        $val = (Get-ItemProperty -Path $path -Name HwSchMode -ErrorAction SilentlyContinue).HwSchMode
        if ($val -eq 2) {
            Set-ItemProperty -Path $path -Name HwSchMode -Value 1 -Type DWord
            Write-Output "✔ Hardware GPU Scheduling disabled (restart required)."
        } else {
            Set-ItemProperty -Path $path -Name HwSchMode -Value 2 -Type DWord
            Write-Output "✔ Hardware GPU Scheduling enabled (restart required)."
        }
    }

    "Switch Power Plans" {
        Write-Output "Available power plans:"
        powercfg /list

        Write-Output ""
        Write-Output "Use one of these commands to switch manually:"
        Write-Output " - Balanced:             powercfg /setactive SCHEME_BALANCED"
        Write-Output " - High Performance:     powercfg /setactive SCHEME_MIN"
        Write-Output " - Ultimate Performance: powercfg /setactive SCHEME_MAX"
    }

    "Disable Game DVR" {
        Write-Output "✔ Disabling Game DVR..."
        Set-ItemProperty -Path "HKCU:\System\GameConfigStore" -Name "GameDVR_Enabled" -Value 0
        Set-ItemProperty -Path "HKCU:\Software\Microsoft\Windows\CurrentVersion\GameDVR" -Name "AppCaptureEnabled" -Value 0
    }

    "Clear Shader Cache" {
        $cache = "$env:LOCALAPPDATA\D3DSCache"
        if (Test-Path $cache) {
            Write-Output "✔ Clearing shader cache..."
            Remove-Item "$cache\*" -Force -Recurse -ErrorAction SilentlyContinue
        } else {
            Write-Output "ℹ Shader cache folder not found."
        }
    }

    default {
        Write-Output "✘ Unknown option: $choice"
    }
}

Write-Output "`nDone. Some changes may require restart to take effect."
