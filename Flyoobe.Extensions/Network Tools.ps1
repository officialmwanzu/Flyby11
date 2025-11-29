# Network Tools – Common network troubleshooting actions
# Host: console
# Options: Flush DNS; Reset Winsock; Show active connections; Test Ping (8.8.8.8)

param([string]$choice)

switch ($choice) {
    "Flush DNS" {
        Write-Output "Flushing DNS cache..."
        ipconfig /flushdns
    }

    "Reset Winsock" {
        Write-Output "Resetting Winsock..."
        netsh winsock reset
    }

    "Show active connections" {
        Write-Output "Active TCP connections:"
        netstat -ano
    }

    "Test Ping (8.8.8.8)" {
        Write-Output "Pinging Google DNS..."
        ping 8.8.8.8
    }

    default {
        Write-Output "Unknown option: $choice"
    }
}
