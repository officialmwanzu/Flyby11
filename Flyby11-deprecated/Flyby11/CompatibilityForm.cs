using System;
using System.Drawing;
using System.Windows.Forms;

namespace Flyby11
{
    public partial class CompatibilityForm : Form
    {

        public CompatibilityForm(bool hasPopcnt, bool hasSse42)
        {
            InitializeComponent();
            InitLayout(hasPopcnt, hasSse42);
        }

        private void InitLayout(bool hasPopcnt, bool hasSse42)
        {
            this.Text = "Flyby11 - Windows 11 Compatibility Check via CpuCheckNative.dll";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10);

            // icon font for check marks
            var iconFont = new Font("Segoe MDL2 Assets", 18);

            // Label for CPU POPCNT Support
            var lblPopcnt = new Label { Text = "CPU POPCNT:", Location = new Point(30, 30), AutoSize = true };
            var iconPopcnt = new Label
            {
                Text = hasPopcnt ? "\uE73D" : "\uE10A",  // ✔ or ❌
                Font = iconFont,
                ForeColor = hasPopcnt ? Color.ForestGreen : Color.Crimson,
                Location = new Point(280, 30),
                AutoSize = true
            };

            // Label for SSE4.2 Support
            var lblSse42 = new Label { Text = "SSE4.2:", Location = new Point(30, 70), AutoSize = true };
            var iconSse42 = new Label
            {
                Text = hasSse42 ? "\uE73D" : "\uE10A",
                Font = iconFont,
                ForeColor = hasSse42 ? Color.ForestGreen : Color.Crimson,
                Location = new Point(280, 70),
                AutoSize = true
            };

            // Label for Upgrade Probability
            var lblProb = new Label
            {
                Text = "Upgrade Probability:",
                Location = new Point(30, 120),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var result = new Label
            {
                Text = (hasPopcnt && hasSse42) ? "Very High" : "Low",
                Location = new Point(250, 120),
                ForeColor = (hasPopcnt && hasSse42) ? Color.ForestGreen : Color.Crimson,
                AutoSize = true
            };

            // Adding the info about upgrade status
            var infoLabel = new Label
            {
                Text = "Note: If both POPCNT and SSE4.2 are supported, your upgrade chances are very high.\n" +
                       "Otherwise, you may face issues while upgrading.",
                Location = new Point(30, 160),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Gray
            };

            // additional info
            var additionalInfoLabel = new Label
            {
                Text = "This app successfully helped upgrade over half a million PCs to Windows 11!\n" +
                       "Fewer e-waste, lower costs, and no need for a new setup. " +
                       "Stay updated. Best wishes, Belim. ",
                Dock = DockStyle.Bottom,
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.DimGray
            };

            // W10 support-end countdown
            var supportEndDate = new DateTime(2025, 10, 14);
            var daysLeft = (supportEndDate - DateTime.Today).Days;

            var countdownLabel = new Label
            {
                Text = $"⚠️ Windows 10 support ends in {daysLeft} days – on October 14, 2025!",
                Dock = DockStyle.Bottom,
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.Red,
            };


            // Button Upgrade
            var upgradeButton = new Button
            {
                Text = "Start Upgrade Now!",
                Dock = DockStyle.Bottom,
                AutoSize = true,
                Size = new Size(180, 40),
                BackColor = Color.ForestGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            upgradeButton.FlatAppearance.BorderSize = 0;
            upgradeButton.Click += UpgradeButton_Click;


            this.Controls.Add(lblPopcnt);
            this.Controls.Add(iconPopcnt);
            this.Controls.Add(lblSse42);
            this.Controls.Add(iconSse42);
            this.Controls.Add(lblProb);
            this.Controls.Add(result);
            this.Controls.Add(infoLabel);

            this.Controls.Add(upgradeButton);
            this.Controls.Add(countdownLabel);
            // this.Controls.Add(additionalInfoLabel);

        }


        private void UpgradeButton_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("You are good to go! Proceeding with the upgrade...", "Upgrade Started", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();

        }
    }
}
