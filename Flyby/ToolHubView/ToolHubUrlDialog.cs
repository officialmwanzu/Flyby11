using System;
using System.Diagnostics;
using System.Windows.Forms;

/// <summary>
/// Dialog for entering a .ps1 URL to install an extension.
/// Also provides a link to the community Extensions Store (GitHub).
/// </summary>
namespace Flyoobe.ToolHub
{
    public class InputDialog : Form
    {
        private TextBox textBox;
        private Button btnOk;
        private Button btnCancel;
        private LinkLabel linkStore;

        public string InputText => textBox.Text;

        public InputDialog(string title, string prompt)
        {
            Text = title;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            MaximizeBox = false;
            Width = 420;
            Height = 200;
            AutoSize = true;
            var lblPrompt = new Label
            {
                Left = 10,
                Top = 15,
                Width = 380,
                AutoSize = true,
                Text = prompt
            };
            Controls.Add(lblPrompt);

            textBox = new TextBox
            {
                Left = 10,
                Top = 40,
                Width = 380,
                AutoSize = true,
            };
            Controls.Add(textBox);

            // Store link
            linkStore = new LinkLabel
            {
                Left = 10,
                Top = 70,
                Width = 380,
                AutoSize = true,
                Text = "Browse Extensions Store (GitHub)"
            };
            linkStore.LinkClicked += (s, e) =>
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/builtbybel/Flyoobe/blob/main/Flyoobe.Extensions/EXTENSIONS.md",
                    UseShellExecute = true
                });
            };
            Controls.Add(linkStore);

            btnOk = new Button
            {
                Text = "OK",
                Left = 220,
                Width = 70,
                Top = 110,
                AutoSize = true,
                DialogResult = DialogResult.OK
            };
            Controls.Add(btnOk);

            btnCancel = new Button
            {
                Text = "Cancel",
                Left = 300,
                Width = 70,
                Top = 110,
                AutoSize = true,
                DialogResult = DialogResult.Cancel
            };
            Controls.Add(btnCancel);

            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }
    }
}
