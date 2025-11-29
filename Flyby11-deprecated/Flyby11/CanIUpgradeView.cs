using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;
using Views;

namespace Flyby11
{
    public partial class CanIUpgradeView : UserControl
    {
        private IsoHandler isoHandler;

        public CanIUpgradeView()
        {
            InitializeComponent();
            isoHandler = new IsoHandler(UpdateStatusLabel);
            InitializeLocalizedStrings();
            SetStyle();
        }

        private void InitializeLocalizedStrings()
        {
            lblPtHdr.Text = Locales.Strings.ciuView_lblPtHdr;
            lblPtHdr2.Text = Locales.Strings.ciuView_lblPtHdr2;
            lblPtAns.Text = Locales.Strings.ciuView_lblPtAns;
            lblPtAns2.Text = Locales.Strings.ciuView_lblPtAns2;
            lblPtAns3.Text = Locales.Strings.ciuView_lblPtAns3;
            lblPtAns4.Text = Locales.Strings.ciuView_lblPtAns4;
            lblPtAns5.Text = Locales.Strings.ciuView_lblPtAns5;
            linkPtAns2.Text = Locales.Strings.ciuView_linkPtAns2; // PC Health Check app
            linkPtAns3.Text = Locales.Strings.ciuView_linkPtAns3; // Hardware requirements

        }

        private void SetStyle()
        {
            // Segoe MDL2 Assets
            btnBack.Text = "\uE72B";
        }

        // Update the status label
        private void UpdateStatusLabel(string message)
        {
            statusLabel.Text = message;
            statusLabel.Refresh(); // Ensure the label updates in real-time
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            SwitchView.GoBack(this.Parent as Panel);
        }

        private async void linkPCHealthCheckApp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string installAssistantUrl = "https://github.com/rcmaehl/WhyNotWin11/releases/download/2.6.1.1/WhyNotWin11.exe";
            await isoHandler.DownloadMediaTool(installAssistantUrl, "WhyNotWin11.exe");
        }

        private void linkHardwareSpecs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.microsoft.com/windows/windows-11-specifications?ocid=smc_marvel_ups_support_win11");
        }

        private void btnWindowsUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Open Windows Update settings
                Process.Start(new ProcessStartInfo
                {
                    FileName = "ms-settings:windowsupdate",
                    UseShellExecute = true
                });

                // Trigger update check via PowerShell
                Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "-Command \"Start-WindowsUpdate -Confirm:$false\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
                UpdateStatusLabel("Checking for updates...");
            }
            catch (Exception ex)
            {
                UpdateStatusLabel($"Error starting Windows Update: {ex.Message}");
            }
        }

        private void btnPatch_Click(object sender, EventArgs e)
        {
            // Check if the application is running with Administrator privileges
            if (!IsRunningAsAdministrator())
            {
                // Request elevation (UAC prompt) and restart the application with admin rights
                UpdateStatusLabel("This application needs to be run as Administrator. Restarting as Administrator...");
                RunAsAdministrator();
                return;
            }

            UpdateStatusLabel("Checking patch status...");

            // Check if the patch has already been applied
            if (IsPatchApplied())
            {
                UpdateStatusLabel("Patch is already applied. No further action needed.");
            }
            else
            {
                // If patch not applied, attempt to apply the patch
                UpdateStatusLabel("Applying patch...");
                if (ApplyPatch())
                {
                    UpdateStatusLabel("Patch applied successfully! You are ready for the upgrade.");
                }
                else
                {
                   // UpdateStatusLabel("Failed to apply the patch. Please try again.");
                }
            }
        }

        // Helper method to check if the application is running as Administrator
        private bool IsRunningAsAdministrator()
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        // Helper method to restart the application with Administrator privileges
        private void RunAsAdministrator()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = Application.ExecutablePath,
                Verb = "runas" // This triggers the UAC prompt
            };
            Process.Start(processStartInfo);
            Application.Exit(); // Exit the current application to prevent further execution
        }

        private bool IsPatchApplied()
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c reg query \"HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\HwReqChk\" /v HwReqChkVars",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        UpdateStatusLabel($"Registry query failed: {error}");
                        return false;
                    }

                    if (output.Contains("HwReqChkVars"))
                    {
                        UpdateStatusLabel("Patch is already applied.");
                        return true;
                    }

                    UpdateStatusLabel("Patch is not applied.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                UpdateStatusLabel($"Error checking patch status: {ex.Message}");
                return false;
            }
        }

        private bool ApplyPatch()
        {
            try
            {
                string[] commands = new[]
                {
            "reg add \"HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\HwReqChk\" /f",
            "reg add \"HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\HwReqChk\" /v HwReqChkVars /t REG_MULTI_SZ /d \"SQ_SecureBootCapable=TRUE\\0SQ_SecureBootEnabled=TRUE\\0SQ_TpmVersion=2\\0SQ_RamMB=8192\" /f",
            "reg delete \"HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\CompatMarkers\" /f",
            "reg delete \"HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Shared\" /f",
            "reg delete \"HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\TargetVersionUpgradeExperienceIndicators\" /f",
            "reg add \"HKLM\\SYSTEM\\Setup\\MoSetup\" /f /v AllowUpgradesWithUnsupportedTPMOrCPU /t REG_DWORD /d 1"
        };

                List<Process> processes = new List<Process>();

                foreach (string command in commands)
                {
                    Process process = new Process();
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c {command}",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    process.Start();
                    processes.Add(process);
                }

                foreach (Process process in processes)
                {
                    process.WaitForExit();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        // UpdateStatusLabel($"Error applying patch: {error}");
                        return false;
                    }

                    if (!string.IsNullOrWhiteSpace(output))
                    {
                        UpdateStatusLabel($"{output}");
                    }
                }

                UpdateStatusLabel("Patch applied successfully!");
                return true;
            }
            catch (Exception ex)
            {
                UpdateStatusLabel($"Error applying patch: {ex.Message}");
                return false;
            }
        }

        private void lblPtHdr2_Click(object sender, EventArgs e)
        {

        }
    }
}
