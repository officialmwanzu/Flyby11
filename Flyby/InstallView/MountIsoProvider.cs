using System.Windows.Forms;

namespace Flyoobe
{
    // Lets the user pick an ISO and mounts it via PowerShell (Mount-DiskImage).
    public sealed class MountIsoProvider : IInstallProvider
    {
        public string Id => "mountiso";
        public string DisplayName => "Mount ISO… (built-in)";
        public string HomepageUrl => null;
        public string DirectDownloadUrl => null;
        public string[] ExactExeNames => System.Array.Empty<string>();
        public string[] WildcardExePatterns => System.Array.Empty<string>();
        public bool TypicallyNeedsIso => false;
        public bool IsExternalTool => false;
        public string Hint => "Pick a Windows ISO and mount it using PowerShell.";

        public string ShowOptionsAndBuildArgs(IWin32Window owner, LastSelections last)
        {
            using (var ofd = new OpenFileDialog { Filter = "ISO files (*.iso)|*.iso|All files (*.*)|*.*" })
            {
                if (ofd.ShowDialog(owner) != DialogResult.OK) return null;
                var isoPath = ofd.FileName;

                var ps = "-NoProfile -Command \"Mount-DiskImage -ImagePath '" + isoPath + "' -PassThru | Out-Null\"";
                if (!ToolHelpers.Run("powershell.exe", ps, asAdmin: true))
                {
                    MessageBox.Show(owner, "Could not mount ISO. Try manual mount via Explorer (Right-click → Mount).",
                        "Mount failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // remember last ISO for other providers (just optional)
                if (last != null) last.LastIsoPath = isoPath;
            }
            return null; // handled
        }
    }
}
