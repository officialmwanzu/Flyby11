using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management; //HATING WMI NEEDS IMPROVEMENT!

namespace Flyoobe
{
    // Just Pure info provider: shows common Boot Menu / BIOS keys by vendor.
    // No tool launch; the provider only displays a small dialog with helpful hints.

    public sealed class BootMenuInfoProvider : IInstallProvider
    {
        public string Id => "bootmenu-info";
        public string DisplayName => "Boot Menu keys (info)";
        public string HomepageUrl => null;
        public string DirectDownloadUrl => null;
        public string[] ExactExeNames => Array.Empty<string>();
        public string[] WildcardExePatterns => Array.Empty<string>();
        public bool TypicallyNeedsIso => false;
        public bool IsExternalTool => false;

        public string Hint =>
            "Shows common Boot Menu and BIOS/UEFI keys for popular manufacturers. " +
            "Helpful when you want to boot from a USB installer.";

        public string ShowOptionsAndBuildArgs(IWin32Window owner, LastSelections last)
        {
            // Build and show info dialog; nothing to run afterwards.
            using (var dlg = new BootMenuInfoDialog())
            {
                dlg.ShowDialog(owner);
            }
            return null;
        }

        // ------------------ self-contained dialog (no designer) ------------------
        private sealed class BootMenuInfoDialog : Form
        {
            private readonly TextBox _txt;
            private readonly Button _btnCopy;
            private readonly Button _btnRecovery;
            private readonly Button _btnClose;

            public BootMenuInfoDialog()
            {
                Text = "Boot Menu keys (info)";
                StartPosition = FormStartPosition.CenterParent;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;
                ShowInTaskbar = false;
                AutoSize = true;
                AutoSizeMode = AutoSizeMode.GrowAndShrink;
                Padding = new Padding(12);

                // Main multiline textbox (read-only)
                _txt = new TextBox
                {
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Vertical,
                    WordWrap = false,
                    Width = 520,
                    Height = 360
                };

                // Buttons
                _btnCopy = new Button { Text = "Copy to clipboard", AutoSize = true };
                _btnRecovery = new Button { Text = "Open Recovery settings", AutoSize = true };
                _btnClose = new Button { Text = "Close", AutoSize = true, DialogResult = DialogResult.OK };

                _btnCopy.Click += (s, e) => { try { Clipboard.SetText(_txt.Text); } catch { } };
                _btnRecovery.Click += (s, e) => { ToolHelpers.OpenUri("ms-settings:recovery"); };

                var btnPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Dock = DockStyle.Fill, Margin = new Padding(0, 8, 0, 0) };
                btnPanel.Controls.Add(_btnClose);
                btnPanel.Controls.Add(_btnRecovery);
                btnPanel.Controls.Add(_btnCopy);

                var root = new TableLayoutPanel { ColumnCount = 1, AutoSize = true, Dock = DockStyle.Fill };
                root.Controls.Add(_txt);
                root.Controls.Add(btnPanel);
                Controls.Add(root);

                AcceptButton = _btnClose;
                CancelButton = _btnClose;

                // Populate content
                _txt.Text = BuildContent();
            }

            // Returns vendor + model (if available) and a table of common keys.
            private string BuildContent()
            {
                // Known keys by vendor (not exhaustive, but practical)
                var map = new List<(string Vendor, string BootMenu, string BiosSetup)>
                {
                    ("Acer",      "F12",         "F2"),
                    ("ASUS",      "Esc or F8",   "Del or F2"),
                    ("Dell",      "F12",         "F2"),
                    ("HP",        "Esc or F9",   "Esc or F10"),
                    ("Lenovo",    "F12 (Fn+F12)", "F1 or F2"),
                    ("MSI",       "F11",         "Del"),
                    ("Gigabyte",  "F12",         "Del"),
                    ("ASRock",    "F11",         "F2 or Del"),
                    ("Toshiba",   "F12",         "F2"),
                    ("Sony",      "F11 or Assist", "F2"),
                    ("Samsung",   "Esc or F12",  "F2"),
                    ("Microsoft", "Vol-Down + Power (Surface)", "Vol-Up + Power (UEFI)")
                };

                // Try to read Manufacturer/Model (WMI)
                var (manufacturer, model) = GetSystemVendor();
                var sb = new StringBuilder();

                // Header
                sb.AppendLine("Boot Menu & BIOS/UEFI keys");
                sb.AppendLine("---------------------------");
                if (!string.IsNullOrWhiteSpace(manufacturer) || !string.IsNullOrWhiteSpace(model))
                {
                    sb.AppendLine($"Detected: {TrimOrUnknown(manufacturer)} {TrimOrUnknown(model)}");
                }
                else
                {
                    sb.AppendLine("Detected: (unknown manufacturer/model)");
                }
                sb.AppendLine();

                // If we have a known vendor, show it first
                var primary = map.FirstOrDefault(x => manufacturer != null && manufacturer.IndexOf(x.Vendor, StringComparison.OrdinalIgnoreCase) >= 0);
                if (!string.IsNullOrEmpty(primary.Vendor))
                {
                    sb.AppendLine($"★ Recommended for your device ({primary.Vendor}):");
                    sb.AppendLine($"   Boot Menu: {primary.BootMenu}");
                    sb.AppendLine($"   BIOS/UEFI: {primary.BiosSetup}");
                    sb.AppendLine();
                }

                // Table of common keys
                sb.AppendLine("Common keys by vendor:");
                sb.AppendLine("Vendor        | Boot Menu        | BIOS/UEFI");
                sb.AppendLine("--------------+-------------------+----------------");
                foreach (var entry in map)
                {
                    sb.AppendLine(
                        $"{entry.Vendor.PadRight(12)}| {entry.BootMenu.PadRight(18)}| {entry.BiosSetup}");
                }

                sb.AppendLine();
                sb.AppendLine("Tips:");
                sb.AppendLine("- Press and hold the key right after power-on, before the Windows logo.");
                sb.AppendLine("- On some laptops you may need to hold Fn with the function key (e.g., Fn+F12).");
                sb.AppendLine("- If you cannot reach the menu, use 'Open Recovery settings' and choose Advanced startup → UEFI Firmware Settings.");
                return sb.ToString();
            }

            private static (string Manufacturer, string Model) GetSystemVendor()
            {
                try
                {
                    using (var searcher = new ManagementObjectSearcher("SELECT Manufacturer, Model FROM Win32_ComputerSystem"))
                    {
                        foreach (ManagementObject mo in searcher.Get())
                        {
                            var m = (mo["Manufacturer"] as string) ?? "";
                            var model = (mo["Model"] as string) ?? "";
                            return (m.Trim(), model.Trim());
                        }
                    }
                }
                catch { /* WMI not available or permission issue */ }
                return (null, null);
            }

            private static string TrimOrUnknown(string s) => string.IsNullOrWhiteSpace(s) ? "(unknown)" : s.Trim();
        }
    }
}
