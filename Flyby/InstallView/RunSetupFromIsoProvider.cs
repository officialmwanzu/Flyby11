using System.IO;
using System.Windows.Forms;

namespace Flyoobe
{
    // Launches setup.exe from mounted ISO without parameters.
    public sealed class RunSetupFromIsoProvider : IInstallProvider
    {
        public string Id => "setupfromiso";
        public string DisplayName => "Run Windows Setup from ISO (full wizard)";
        public string HomepageUrl => null;
        public string DirectDownloadUrl => null;
        public string[] ExactExeNames => new[] { "setup.exe" };
        public string[] WildcardExePatterns => System.Array.Empty<string>();
        public bool TypicallyNeedsIso => true;
        public bool IsExternalTool => false;
        public string Hint =>
            "Opens the full Windows Setup wizard from a mounted ISO. " +
            "You can choose between Upgrade or Clean install and set partitions manually. " +
            "Recommended if you want complete control over the installation process.";

        public string ShowOptionsAndBuildArgs(IWin32Window owner, LastSelections last)
        {
            var setup = FindSetupExeOnMountedDrives();
            if (setup == null)
            {
                MessageBox.Show(owner, "No mounted Windows ISO with 'setup.exe' found.",
                    "Setup missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            if (!ToolHelpers.Confirm(owner, "Run setup.exe from the mounted ISO?"))
                return null;

            ToolHelpers.Run(setup, "", asAdmin: true);
            return null; 
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
