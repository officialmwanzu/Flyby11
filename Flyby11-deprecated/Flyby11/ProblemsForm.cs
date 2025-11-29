using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Flyby11
{
    public partial class ProblemsForm : Form
    {
        private readonly Dictionary<string, string> _issues = new Dictionary<string, string>()
{
    {"Can't upgrade to Windows 11 – Error 0x80888002","https://github.com/builtbybel/Flyby11/discussions/135"},
    { "Upgrading without losing all programs and settings", "https://github.com/builtbybel/Flyby11/discussions/54"},
    { "Upgrade Driver issue 0xC1900101", "https://github.com/builtbybel/Flyby11/issues/157"},
    { "We are enable to complete your request at this time", "https://github.com/builtbybel/Flyby11/issues/158"},
    { "Upgrade succeeded, but future updates not possible ", "https://github.com/builtbybel/Flyby11/discussions/144"},
    { "We couldn't install Windows Server - We've set your PC back the way it was Error 0xC1900101 - 0x20017", "https://github.com/builtbybel/Flyby11/issues/103 "},
    { "Mounting ISO fails", "https://github.com/builtbybel/Flyby11/issues/156"},
    { "Can't find your issue? Browse the community discussions for help", "https://github.com/builtbybel/Flyby11/discussions"},
};

        public ProblemsForm()
        {
            InitializeComponent();
        }

        private void ProblemsForm_Load(object sender, EventArgs e)
        {
            foreach (var kv in _issues)
            {
                dataGridViewIssues.Rows.Add(kv.Key, "Details…");
            }
        }

        private void dataGridViewIssues_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewIssues.Columns[e.ColumnIndex] is DataGridViewLinkColumn &&
    e.RowIndex >= 0)
            {
                var code = dataGridViewIssues.Rows[e.RowIndex].Cells["ErrorCode"].Value.ToString();
                if (_issues.TryGetValue(code, out var url))
                {
                    System.Diagnostics.Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
            }
        }
    }
}
