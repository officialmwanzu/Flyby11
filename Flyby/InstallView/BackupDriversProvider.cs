using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Flyoobe
{
    // Provider that exports all installed drivers into a fixed subfolder named "DriversBackup"
    // INSIDE the parent folder chosen by the user, using:
    //   pnputil.exe /export-driver * <chosenParent>\DriversBackup
    // Behavior:
    // - Prompts user to choose a parent folder (defaults to C:\).
    // - Always creates/uses "<chosenParent>\DriversBackup".
    // - Runs pnputil elevated.
    // - Shows a completion summary with INF package count.

    public sealed class BackupDriversProvider : IInstallProvider
    {
        public string Id => "drivers-backup";
        public string DisplayName => "Backup installed drivers (choose folder)";
        public string HomepageUrl => null;
        public string DirectDownloadUrl => null;
        public string[] ExactExeNames => new[] { "pnputil.exe" };
        public string[] WildcardExePatterns => Array.Empty<string>();
        public bool TypicallyNeedsIso => false;
        public bool IsExternalTool => false;

        public string Hint =>
                    "Exports all currently installed device drivers (INF + binaries) to a folder you choose. " +
            "After a clean install, point Device Manager to that folder to restore drivers even without internet.";

        public string ShowOptionsAndBuildArgs(IWin32Window owner, LastSelections last)
        {
            // Default suggestion for the parent, the actual export target will be <parent>\DriversBackup
            var defaultParent = @"C:\";
            string parentPath;

            // pick the PARENT folder where the 'DriversBackup' subfolder will be created
            using (var fbd = new FolderBrowserDialog
            {
                Description = "Choose the PARENT folder. A 'DriversBackup' subfolder will be created inside it.",
                ShowNewFolderButton = true,
                SelectedPath = defaultParent
            })
            {
                var dlgResult = fbd.ShowDialog(owner);
                if (dlgResult != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    return null; //user cancelled

                parentPath = fbd.SelectedPath.Trim();
                try
                {
                    //Normalize parent path
                    parentPath = Path.GetFullPath(parentPath);
                }
                catch
                { //Keep raw selection if normalization fails (we'll error out later if invalid)
                     }
            }

            // Build the fixed subfolder path: <parent>\DriversBackup (always appended)
            string target = Path.Combine(parentPath, "DriversBackup");

            // Confirm before running the elevated export
            if (!ToolHelpers.Confirm(owner,
                "Export all installed drivers to:\r\n\r\n" + target +
                "\r\n\r\nThis will run 'pnputil /export-driver *' with administrative rights."))
                return null;

            // Ensure the target DriversBackup folder exists
            try
            {
                Directory.CreateDirectory(target);
            }
            catch (Exception ex)
            {
                MessageBox.Show(owner,
                    "Could not create target folder:\r\n" + target + "\r\n\r\n" + ex.Message,
                    "Folder error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            try
            {
                // Start pnputil as an elevated process targeting the 'DriversBackup' subfolder
                var psi = new ProcessStartInfo
                {
                    FileName = "pnputil.exe",
                    Arguments = "/export-driver * \"" + target + "\"",
                    UseShellExecute = true,
                    Verb = "runas", // force elevation
                    CreateNoWindow = true
                };

                using (var proc = Process.Start(psi))
                {
                    if (proc == null)
                    {
                        MessageBox.Show(owner, "Could not start pnputil.exe", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }

                    proc.WaitForExit();
                }

                // Count exported INF files to provide a quick summary
                int infCount = 0;
                try
                {
                    infCount = Directory.GetFiles(target, "*.inf", SearchOption.AllDirectories).Length;
                }
                catch
                {  // Ignore counting errors; we can still show success without the count
                   }

                    MessageBox.Show(owner,
                    "Driver export completed.\r\n\r\n" +
                    $"Target: {target}\r\n" +
                    $"Exported driver packages: {infCount}\r\n\r\n" +
                    "Tip: After reinstall, in Device Manager choose 'Update driver' → 'Browse my computer' " +
                    "and select this folder to restore drivers.",
                    "Export complete",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(owner,
                    "Failed to run pnputil:\r\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;  // returning null indicates no further action needed
        }
    }
}
