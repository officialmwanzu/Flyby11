using System.IO;
using System.Windows.Forms;

namespace Flyoobe
{
    // Runs setup.exe from a mounted Windows ISO with upgrade parameters.
    public sealed class InPlaceRepairProvider : IInstallProvider
    {
        public string Id => "inplace";
        public string DisplayName => "Repair upgrade (keeps apps & files)";
        public string HomepageUrl => null;
        public string DirectDownloadUrl => null;
        public string[] ExactExeNames => new[] { "setup.exe" }; // for documentation only
        public string[] WildcardExePatterns => System.Array.Empty<string>();
        public bool TypicallyNeedsIso => true;
        public bool IsExternalTool => false;
        public string Hint =>
       "Starts Windows Setup from a mounted ISO in automatic repair mode. " +
       "This reinstalls Windows while keeping your apps, settings, and personal files. " +
       "Recommended if your system is broken but you want to keep everything.";

        public string ShowOptionsAndBuildArgs(IWin32Window owner, LastSelections last)
        {
            var setupPath = FindSetupExeOnMountedDrives();
            if (setupPath == null)
            {
                MessageBox.Show(owner, "No 'setup.exe' found. Please mount a Windows ISO first.",
                    "Setup not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            if (!ToolHelpers.Confirm(owner, "Start repair upgrade via setup.exe? The PC will restart during the process."))
                return null;

            // Start directly and handle here
            ToolHelpers.Run(setupPath, "/auto upgrade /dynamicupdate enable", asAdmin: true);
            return null; // handled > host should not run anything else
        }

        private static string FindSetupExeOnMountedDrives()
        {
            try
            {
                foreach (var di in DriveInfo.GetDrives())
                {
                    if (di.DriveType == DriveType.CDRom || di.DriveType == DriveType.Removable || di.DriveType == DriveType.Fixed)
                    {
                        var p = Path.Combine(di.Name, "setup.exe");
                        if (File.Exists(p)) return p;
                    }
                }
            }
            catch { }
            return null;
        }
    }
}
