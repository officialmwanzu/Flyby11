using System;
using System.Windows.Forms;

namespace Flyoobe
{
    // Native provider to reboot directly into UEFI/BIOS setup using "shutdown.exe /r /fw /t 0".
    // If /fw is not supported on the machine, we offer a fallback to Recovery Settings (WinRE path to UEFI Firmware Settings).

    public sealed class RebootToUefiProvider : IInstallProvider
    {
        public string Id => "uefi-reboot";
        public string DisplayName => "Reboot into UEFI firmware (BIOS)";
        public string HomepageUrl => null;
        public string DirectDownloadUrl => null;
        public string[] ExactExeNames => new[] { "shutdown.exe" };   // informational
        public string[] WildcardExePatterns => Array.Empty<string>();
        public bool TypicallyNeedsIso => false;
        public bool IsExternalTool => false;

        public string Hint =>
            "Restarts the PC straight into UEFI/BIOS setup (if supported by the firmware). " +
            "Useful to change boot order before a clean install.";

        public string ShowOptionsAndBuildArgs(IWin32Window owner, LastSelections last)
        {
            // Confirm with the user (explicit reboot).
            if (!ToolHelpers.Confirm(owner,
                "Reboot into UEFI/BIOS now? The PC will restart immediately."))
            {
                return null; // canceled
            }

            // Try the native fast path first: /r /fw /t 0
            var ok = ToolHelpers.Run("shutdown.exe", "/r /fw /t 0", asAdmin: true);
            if (!ok)
            {
                // Not supported or blocked on this device > offer a fallback....
                var choice = MessageBox.Show(owner,
                    "Direct firmware reboot was not supported on this device.\r\n\r\n" +
                    "Open Recovery settings so you can choose 'UEFI Firmware Settings' after a restart?",
                    "UEFI reboot fallback",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);

                if (choice == DialogResult.OK)
                {
                    // Settings > System > Recovery (so we can go to Advanced startup > UEFI Firmware Settings)
                    ToolHelpers.OpenUri("ms-settings:recovery");
                }
            }

            // We handled everything here; host should not try to launch an external EXE.
            return null;
        }
    }
}
