using Flyoobe;
using System;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

public sealed class NativeResetProvider : IInstallProvider
{
    public string Id => "winreset";
    public string DisplayName => "Windows Reset (built-in)";
    public string HomepageUrl => "ms-settings:recovery";
    public string DirectDownloadUrl => null;
    public string[] ExactExeNames => new[] { "systemreset.exe" }; // only Doku
    public string[] WildcardExePatterns => Array.Empty<string>();
    public bool TypicallyNeedsIso => false;
    public bool IsExternalTool => false;

    public string Hint =>
        "Built-in Windows reset. Inside the wizard choose 'Keep my files' or 'Remove everything' and optionally 'Cloud download'.";

    public string ShowOptionsAndBuildArgs(IWin32Window owner, LastSelections last)
    {
        using (var dlg = new ResetOptionsDialog())
        {
            if (dlg.ShowDialog(owner) != DialogResult.OK)
                return null; // canceled > host does not do anything

            switch (dlg.SelectedMode)
            {
                case ResetMode.ResetWizard:
                    if (!ToolHelpers.Confirm(owner,
                        "Start 'Reset this PC'? This may remove apps/data depending on your selection."))
                        return null;

                    if (!ToolHelpers.Run("systemreset.exe", "-factoryreset", true))
                        ToolHelpers.Run("systemreset.exe", "", true);
                    return null; //RunSelectedAsync() should not start anything else

                case ResetMode.RebootToWinRE:
                    ToolHelpers.Run("shutdown.exe", "/r /o /t 0 /f", true);
                    return null;

                case ResetMode.OpenSettings:
                    ToolHelpers.OpenUri("ms-settings:recovery");
                    return null;

                default:
                    return null;
            }
        }
    }

    private enum ResetMode { ResetWizard, RebootToWinRE, OpenSettings }

    private sealed class ResetOptionsDialog : Form
    {
        private RadioButton rbWizard, rbWinre, rbSettings;
        private Button btnOk, btnCancel;

        public ResetMode SelectedMode =>
            rbWizard.Checked ? ResetMode.ResetWizard :
            rbWinre.Checked ? ResetMode.RebootToWinRE :
                               ResetMode.OpenSettings;

        public ResetOptionsDialog()
        {
            Text = "Windows Reset options";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            ShowInTaskbar = false;
            AutoSize = true; AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Padding = new Padding(12);

            var root = new TableLayoutPanel { ColumnCount = 1, AutoSize = true, Dock = DockStyle.Fill };

            rbWizard = new RadioButton { Text = "Open Reset wizard (systemreset.exe -factoryreset)", Checked = true, AutoSize = true };
            rbWinre = new RadioButton { Text = "Reboot into Advanced Startup (WinRE)", AutoSize = true };
            rbSettings = new RadioButton { Text = "Open Recovery Settings", AutoSize = true };

            root.Controls.Add(rbWizard);
            root.Controls.Add(rbWinre);
            root.Controls.Add(rbSettings);

            root.Controls.Add(new Label
            {
                AutoSize = true,
                MaximumSize = new System.Drawing.Size(520, 0),
                Margin = new Padding(0, 8, 0, 0),
                Text = "Note: Windows does not provide supported CLI switches to pre-select 'Keep my files', 'Remove everything' or 'Cloud download'. Choose them in the wizard."
            });

            var pnlButtons = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Margin = new Padding(0, 12, 0, 0) };
            btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK, AutoSize = true };
            btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, AutoSize = true, Margin = new Padding(6, 0, 0, 0) };
            pnlButtons.Controls.Add(btnOk);
            pnlButtons.Controls.Add(btnCancel);
            root.Controls.Add(pnlButtons);

            Controls.Add(root);
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }
    }
}
