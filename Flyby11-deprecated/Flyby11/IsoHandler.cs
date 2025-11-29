using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flyby11
{
    public class IsoHandler
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly Action<string> _updateStatus;

        public IsoHandler(Action<string> updateStatus)
        {
            _updateStatus = updateStatus;
        }

        public async Task HandleIso(string isoPath, bool experimentalEnabled)
        {
            try
            {
                // Block Win10 ISOs
                string isoFileName = Path.GetFileName(isoPath);
                if (Regex.IsMatch(isoFileName, @"(?i)(win10|windows\s*10)"))
                {
                    _updateStatus("Error: Windows 10 ISOs are not supported.");
                    MessageBox.Show("Windows 10 ISOs are not supported. Please use a Windows 11 ISO.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _updateStatus(Locales.Strings._debugStatusMounting); //Mounting the ISO... Hang tight!

                // Ensure the ISO path is properly quoted
                string quotedIsoPath = $"\"{isoPath}\"";

                // Pass the quoted path to ExecutePowerShellCommand
                string driveLetter = await MountIsoAndGetDriveLetter(isoPath);

                if (string.IsNullOrEmpty(driveLetter))
                {
                    _updateStatus(Locales.Strings._debugStatusMountingFailed); //Failed to mount the ISO. Please try again.
                    MessageBox.Show(Locales.Strings._debugStatusMountingFailed, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _updateStatus(Locales.Strings._debugStatusMountingSuccess); //ISO mounted successfully! Let's get this Windows 11 ready!

                string setupPath = Path.Combine(driveLetter, "sources", "setupprep.exe");
                if (File.Exists(setupPath))
                {
                    await RunSetupWithAdminRights(setupPath, experimentalEnabled);
                }
                else
                {
                    _updateStatus($"{Locales.Strings._debugStatusSetupFileNotFound} {driveLetter}. Aborting."); // Setup file not found in
                    MessageBox.Show($"{Locales.Strings._debugStatusSetupFileNotFound} {driveLetter}",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                _updateStatus($"{Locales.Strings._debugStatusHandleIsoEx} {ex.Message}"); //Oops! Something went wrong:
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<string> MountIsoAndGetDriveLetter(string isoPath)
        {
            try
            {
                _updateStatus("Mounting ISO...");

                // PowerShell script to mount ISO and get the assigned drive letter
                string script = $@"
$iso = Mount-DiskImage -ImagePath '{isoPath}' -PassThru
$volumes = Get-Volume -DiskImage $iso
$volumes.DriveLetter
";

                using (var process = new Process())
                {
                    process.StartInfo.FileName = "powershell.exe";
                    process.StartInfo.Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();

                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();

                    await process.WaitForExitAsync();

                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        _updateStatus("PowerShell error during mount: " + error);
                        return null;
                    }

                    string driveLetter = output.Trim();
                    if (!string.IsNullOrWhiteSpace(driveLetter))
                    {
                        _updateStatus($"ISO mounted successfully at {driveLetter}:\\");
                        return $"{driveLetter}:\\";
                    }
                    else
                    {
                        _updateStatus("Failed to retrieve drive letter after mounting ISO.");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                _updateStatus("Exception during mounting: " + ex.Message);
                return null;
            }
        }

        private async Task RunSetupWithAdminRights(string setupPath, bool experimentalEnabled)
        {
            try
            {
                _updateStatus(Locales.Strings._debugInstallRunElevated); //Starting the setup process with elevated privileges...

                // Default argument
                string arguments = "/Product Server";

                // If experimental mode is enabled, append additional setup parameters
                if (experimentalEnabled)
                {
                    arguments += " /Compat IgnoreWarning /MigrateDrivers All";
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"Start-Process '{setupPath}' -ArgumentList '{arguments}' -Verb runas\"",
                    Verb = "runas",
                    UseShellExecute = true,
                    CreateNoWindow = false
                };

                _updateStatus(Locales.Strings._debugStatusRunning); //Almost there! We're getting the setup ready...

                using (var process = Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        await process.WaitForExitAsync();
                    }
                }
                // Final status message
                string finalStatus = Locales.Strings._debugInstallReady;

                if (experimentalEnabled)
                {
                    finalStatus += " (Advanced mode enabled: Compatibility checks bypassed)";
                }

                _updateStatus(finalStatus);

                //Windows 11 installation can now proceed. Please follow the instructions in the setup window.
                MessageBox.Show(Locales.Strings.msg_InstallReady, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _updateStatus($"Error: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Create unattend.xml file in the sources\$OEM$\$$\Panther directory
        /// </summary>
        /// <param name="usbDrive">The USB drive where the unattend.xml will be created.</param>
        public void CreateUnattendXml(string usbDrive)
        {
            string unattendDir = Path.Combine(usbDrive, "sources", "$OEM$", "$$", "Panther");
            Directory.CreateDirectory(unattendDir); // Create the directory if it doesn't exist

            string unattendPath = Path.Combine(unattendDir, "unattend.xml");

            // Check if the file already exists
            if (!File.Exists(unattendPath))
            {
                // Create the unattend.xml content
                string xmlContent = @"<unattend xmlns='urn:schemas-microsoft-com:unattend'>
<settings pass='disabled'>
<component xmlns:wcm='http://schemas.microsoft.com/WMIConfig/2002/State' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' name='Microsoft-Windows-Setup' processorArchitecture='amd64' language='neutral' publicKeyToken='31bf3856ad364e35' versionScope='nonSxS'>
<UserData>
<ProductKey>
<Key/>
</ProductKey>
</UserData>
<RunSynchronous>
<RunSynchronousCommand wcm:action='add'>
<Order>1</Order>
<Path>reg add HKLM\SYSTEM\Setup\LabConfig /v BypassTPMCheck /t REG_DWORD /d 1 /f</Path>
</RunSynchronousCommand>
<RunSynchronousCommand wcm:action='add'>
<Order>2</Order>
<Path>reg add HKLM\SYSTEM\Setup\LabConfig /v BypassSecureBootCheck /t REG_DWORD /d 1 /f</Path>
</RunSynchronousCommand>
<RunSynchronousCommand wcm:action='add'>
<Order>3</Order>
<Path>reg add HKLM\SYSTEM\Setup\LabConfig /v BypassRAMCheck /t REG_DWORD /d 1 /f</Path>
</RunSynchronousCommand>
</RunSynchronous>
</component>
</settings>
<settings pass='specialize'>
<component xmlns:wcm='http://schemas.microsoft.com/WMIConfig/2002/State' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' name='Microsoft-Windows-Deployment' processorArchitecture='amd64' language='neutral' publicKeyToken='31bf3856ad364e35' versionScope='nonSxS'>
<RunSynchronous>
<RunSynchronousCommand wcm:action='add'>
<Order>1</Order>
<Path>reg add HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\OOBE /v BypassNRO /t REG_DWORD /d 1 /f</Path>
</RunSynchronousCommand>
</RunSynchronous>
</component>
</settings>
</unattend>";

                File.WriteAllText(unattendPath, xmlContent);
                _updateStatus("Created unattend.xml in the sources\\$OEM$\\$$\\Panther directory.");
            }
            else
            {
                _updateStatus("unattend.xml already exists in the sources\\$OEM$\\$$\\Panther directory.");

                // Display a message box to inform the user
                MessageBox.Show("The patch has already been applied. The 'unattend.xml' file already exists in the specified directory.",
                                "Patch Already Applied",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Downloads a specified tool (Media Creation Tool or Installation Assistant) from a provided URL.
        /// </summary>
        /// <param name="downloadUrl">The URL of the tool to download.</param>
        /// <param name="fileName">The name of the file to save.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DownloadMediaTool(string downloadUrl, string fileName)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Executable Files (*.exe)|*.exe";
                saveFileDialog.Title = $"Save {fileName}";
                saveFileDialog.FileName = fileName;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string destinationPath = saveFileDialog.FileName;

                    try
                    {
                        _updateStatus($"Preparing to download {fileName}...");
                        _updateStatus("Download may take some time, please wait...");

                        var response = await httpClient.GetAsync(downloadUrl);
                        response.EnsureSuccessStatusCode();

                        using (var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            await response.Content.CopyToAsync(fileStream);
                        }

                        _updateStatus($"{fileName} download completed successfully!");

                        // Start the downloaded tool
                        _updateStatus($"Starting {fileName}...");
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = destinationPath,
                            UseShellExecute = true // To run .exe files
                        });
                    }
                    catch (Exception ex)
                    {
                        _updateStatus($"An error occurred while downloading {fileName}: {ex.Message}");
                    }
                }
                else
                {
                    _updateStatus($"{fileName} download canceled by user.");
                }
            }
        }
    }

    public static class ProcessExtensions
    {
        public static async Task WaitForExitAsync(this Process process)
        {
            var tcs = new TaskCompletionSource<bool>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(true);
            await tcs.Task;
        }
    }
}
