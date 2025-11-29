# Developer Essentials – Quick access to developer tools and features
# Host: console
# Options: Enable Developer Mode; Install WSL; Open Windows Features (silent); Open Device Manager (silent)

param([string]$choice)

switch ($choice) {
    "Enable Developer Mode" {
        Write-Output "Enabling Developer Mode..."
        reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock" /t REG_DWORD /f /v "AllowDevelopmentWithoutDevLicense" /d 1
        Write-Output "✔ Developer Mode enabled."
    }

    "Install WSL" {
        Write-Output "Installing Windows Subsystem for Linux..."
        Start-Process "wsl.exe" -ArgumentList "--install" -Verb runAs
    }

    "Open Windows Features" {
        Write-Output "Opening Windows Features dialog..."
        Start-Process "optionalfeatures.exe"
    }

    "Open Device Manager" {
        Write-Output "Opening Device Manager..."
        Start-Process "devmgmt.msc"
    }

    default {
        Write-Output "Unknown option: $choice"
    }
}
