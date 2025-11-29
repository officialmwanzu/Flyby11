using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Flyby11
{
    public class Logger
    {
        private MainForm mainForm;

        public Logger(MainForm mainForm)
        {
            this.mainForm = mainForm ?? throw new ArgumentNullException(nameof(mainForm));
        }

        // Log method for a single string
        public void Log(string message, Color color, float fontSize = 10.5f)
        {
            if (mainForm.InvokeRequired)
            {
                mainForm.Invoke(new Action(() => Log(message, color, fontSize)));
                return;
            }


            AppendMessageToConversation(message, color, fontSize);             // Append message to conversation
        }

        private void AppendMessageToConversation(string message, Color color, float fontSize)
        {
            Label statusLabel = mainForm.Controls.Find("statusLabel", true).FirstOrDefault() as Label;

            if (statusLabel != null)
            {
                statusLabel.Text = ""; 
                statusLabel.Text += "\r\n" + message;
                statusLabel.ForeColor = color;
                statusLabel.Font = new Font(statusLabel.Font.FontFamily, fontSize);
                statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

    }
}
