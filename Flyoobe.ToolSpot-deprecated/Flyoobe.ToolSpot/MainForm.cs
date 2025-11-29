/// This tool is part of my Flyoobe app

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Flyoobe.ToolSpot
{


    public partial class MainForm : Form

    { // Path of the text DB
        public string DatabasePath { get; set; } =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "spot.txt");

        // In-memory list of all tools
        private readonly List<ToolItem> _all = new List<ToolItem>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDatabase();
            //ApplyFilter(); // show all initially
            textSearch.Focus();
        }

        private void SetStatus(string text)
        {
            if (labelStatus != null) labelStatus.Text = text;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                // close it
                var form = this.FindForm();
                form?.Close();
            }
        }

        private void listResults_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { LaunchSelected(); e.Handled = true; }
        }

        private void listResults_DoubleClick(object sender, EventArgs e) => LaunchSelected();

        private void textSearch_TextChanged(object sender, EventArgs e) => ApplyFilter();

        /// <summary>
        /// Reads toolspot.txt (Name;Command). Ignores empty and comment lines.
        /// </summary>
        private void LoadDatabase()
        {
            _all.Clear();

            if (!File.Exists(DatabasePath))
            {
                SetStatus($"Database not found: {DatabasePath}");
                return;
            }

            foreach (var line in File.ReadAllLines(DatabasePath))
            {
                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;
                if (trimmed.StartsWith("#")) continue;

                var item = ParseLine(trimmed);
                if (item != null) _all.Add(item);
            }

            SetStatus($"Loaded {_all.Count} tools.");
        }

        /// <summary>
        /// Parses "Name;Command" lines. Returns null if malformed.
        /// </summary>
        private ToolItem ParseLine(string line)
        {
            var parts = line.Split(new[] { ';' }, 2);
            if (parts.Length != 2) return null;

            var name = parts[0].Trim();
            var cmd = parts[1].Trim();
            if (name.Length == 0 || cmd.Length == 0) return null;

            return new ToolItem(name, cmd);
        }

        /// <summary>
        /// Filters list by substring (case-insensitive) from textSearch.
        /// </summary>
        private void ApplyFilter()
        {
            var q = (textSearch.Text ?? string.Empty).Trim().ToLower();
            var list = string.IsNullOrEmpty(q)
                ? _all
                : _all.Where(t => t.Name.ToLower().Contains(q)).ToList();

            listResults.BeginUpdate();
            listResults.Items.Clear();
            foreach (var t in list) listResults.Items.Add(t);
            listResults.EndUpdate();

            if (listResults.Items.Count > 0) listResults.SelectedIndex = 0;
            SetStatus(listResults.Items.Count > 0 ? $"{listResults.Items.Count} result(s)" : "No results");
        }

        /// <summary>
        /// Launches the selected tool's command. Supports URIs, .msc/.cpl, exe + args.
        /// </summary>
        private void LaunchSelected()
        {
            if (!(listResults.SelectedItem is ToolItem t)) return;

            try
            {
                // Split command into FileName + Arguments (handles quoted paths)
                SplitCommand(t.Command, out var fileName, out var args);

                var psi = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = args,
                    UseShellExecute = true // required for ms-settings:, .msc/.cpl
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not launch: " + ex.Message, "Quick Tools",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Splits a command line into executable and arguments.
        /// Examples:
        ///   notepad.exe
        ///   "C:\Path With Spaces\app.exe" --flag
        ///   chrome.exe --settings
        /// For URIs / shell verbs (e.g., ms-settings:...), no split is needed.
        /// </summary>
        private void SplitCommand(string command, out string fileName, out string args)
        {
            command = command.Trim();

            // If it's a URI-like command (ms-settings:, http:, etc.), pass as-is.
            if (command.Contains(":") && !command.Contains("\\"))
            {
                fileName = command;
                args = string.Empty;
                return;
            }

            // Quoted path case: "C:\Program Files\App\app.exe" arg1 arg2
            if (command.StartsWith("\""))
            {
                int end = command.IndexOf('"', 1);
                if (end > 0)
                {
                    fileName = command.Substring(1, end - 1);
                    args = command.Substring(end + 1).TrimStart();
                    return;
                }
            }

            // Unquoted: split on first space
            int space = command.IndexOf(' ');
            if (space > 0)
            {
                fileName = command.Substring(0, space);
                args = command.Substring(space + 1);
            }
            else
            {
                fileName = command;
                args = string.Empty;
            }
        }

        private class ToolItem
        {
            public string Name { get; }
            public string Command { get; }

            public ToolItem(string name, string command)
            { Name = name; Command = command; }

            public override string ToString() => Name; // shown in ListBox
        }

        private void textSearch_Click(object sender, EventArgs e)
        {
            textSearch.Clear();
        }
    }
}
