using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flyby11
{
    public partial class ClippyUI : Form
    {
        private Point mouseDownLocation;
        private Logger logger;
        private MainForm mainForm;

        public ClippyUI(MainForm mainForm, Logger logger)
        {
            InitializeComponent();

            this.mainForm = mainForm;
            this.logger = logger;

            // Initialize ClippyUI appearance and position
            InitializeFormAppearance();
        }

        private void InitializeFormAppearance()
        {
            // Segoe MDL2 Assets
            lblClosePane.Text = "\uE8BB";
            lblHelp.Text = "\uE897";

            // Remove the form border
            this.FormBorderStyle = FormBorderStyle.None;

            // Take a screenshot of the desktop
            Bitmap desktopScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics g = Graphics.FromImage(desktopScreenshot))
            {
                g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, desktopScreenshot.Size);
            }

            // Determine color of the pixel under form
            Point formLocation = this.PointToScreen(Point.Empty);
            Color desktopColor = desktopScreenshot.GetPixel(formLocation.X + this.Width / 2, formLocation.Y + this.Height / 2);

            // Set PictureBox properties
            assetClippy.Size = new Size(150, 150);

            // Make the form transparent
            this.BackColor = desktopColor;
            this.TransparencyKey = desktopColor;

            // Load default Clippy image
            string defaultImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app", "clippy.png");
            if (File.Exists(defaultImagePath))
            {
                assetClippy.Image = Image.FromFile(defaultImagePath);
            }
            else
            {
                LoadRandomCharacter(); // fallback, no picture found
            }
        }

        public void SetClippyPosition(Form mainForm)
        {
            int margin = 10;

            // Get the working area of the screen
            Rectangle screenBounds = Screen.FromControl(mainForm).WorkingArea;

            // Calculate the position of the Clippy UI (right below the main form)
            int x = Math.Min(mainForm.Location.X + mainForm.Width + margin, screenBounds.Right - this.Width);
            int y = Math.Min(mainForm.Location.Y + mainForm.Height - this.Height, screenBounds.Bottom - this.Height);

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(x, y);
        }

        private void LoadRandomCharacter()
        {
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app\\assets");
            string[] searchPatterns = { "clippy*.png", "clippy*.gif" };

            List<string> matchingFiles = new List<string>();
            foreach (string pattern in searchPatterns)
            {
                string[] files = Directory.GetFiles(directoryPath, pattern);
                matchingFiles.AddRange(files);
            }

            if (matchingFiles.Count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(0, matchingFiles.Count);
                string randomClippyImagePath = matchingFiles[randomIndex];
                assetClippy.Image = Image.FromFile(randomClippyImagePath);
            }
            else
            {
                MessageBox.Show("Oops! I can't work if I'm invisible. Please make sure Clippy is in the right place!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void assetClippy_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownLocation = e.Location;
            }
        }

        // EventHandler for the MouseMove event of the Clippy image
        private void assetClippy_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Update position of the SetupForm based on the movement of the Clippy image
                this.Left += e.X - mouseDownLocation.X;
                this.Top += e.Y - mouseDownLocation.Y;

                // Update position of the MainForm relative to the new position of the SetupForm
                mainForm.Left += e.X - mouseDownLocation.X;
                mainForm.Top += e.Y - mouseDownLocation.Y;
            }
        }

        private async void lblClosePane_Click(object sender, EventArgs e)
        {
            logger.Log("Looks like you're giving me the boot! No hard feelings—I'll be here if you need me again.", Color.DeepPink);

            await Task.Delay(1000);

            // Hide Clippy
            this.Hide();
        }

        private void lblHelp_Click(object sender, EventArgs e)
        {
            mainForm._faqHandler.AddNextFAQStep();
        }
    }
}
