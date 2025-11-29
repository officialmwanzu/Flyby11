using System.Windows.Forms;

namespace Flyoobe
{
    public sealed class VentoyProvider : IInstallProvider
    {
        public string Id => "ventoy";
        public string DisplayName => "Ventoy";
        public string HomepageUrl => "https://www.ventoy.net/en/download.html";
        public string DirectDownloadUrl => null; // Ventoy ships as ZIP/installer, no stable EXE direct link
        public string[] ExactExeNames => new[] { "Ventoy2Disk.exe" };
        public string[] WildcardExePatterns => new[] { "Ventoy*Disk*.exe" };
        public bool IsExternalTool => true;   // needs browsing/resolving
        public bool TypicallyNeedsIso => false;

        // Custom hint for the UI
        public string Hint =>
            "Ventoy creates a USB drive where you can later copy multiple ISO files. " +
            "You don't need to select an ISO during setup.";

        // Add advanced CLI support!?
        public string ShowOptionsAndBuildArgs(IWin32Window owner, LastSelections last)
        {
            // Ventoy doesn't really use CLI args for normal installation.
            // Just launch the GUI and let the user configure inside.
            return string.Empty;
        }
    }
}
