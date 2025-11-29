using System.Windows.Forms;

namespace Views
{
    public static class SwitchView
    {
        public static Control PreviousView { get; private set; }
        public static Control DefaultView { get; set; } // Store the default view (panelMain)

        // Switch to a new view
        public static void SetView(Control newView, Panel targetPanel)
        {
            // If there's already a view in the panel, save it as PreviousView
            if (targetPanel.Controls.Count > 0)
            {
                // Only save the previous view if it's not the default view (panelMain)
                if (PreviousView == null)
                {
                    PreviousView = targetPanel.Controls[0]; // Store current view as PreviousView
                }
            }

            newView.Dock = DockStyle.Fill;
            targetPanel.Controls.Clear();
            targetPanel.Controls.Add(newView); // Add new view to the panel
        }

        // Go back to the previous view
        public static void GoBack(Panel targetPanel)
        {
            if (PreviousView != null)
            {
                targetPanel.Controls.Clear(); // Clear the current view
                targetPanel.Controls.Add(PreviousView); // Add the previous view back
                PreviousView.Dock = DockStyle.Fill;
                PreviousView = null; // Reset PreviousView after returning to it
            }
            else if (DefaultView != null)
            {
                // If there's no PreviousView, return to the default (panelMain)
                targetPanel.Controls.Clear();
                targetPanel.Controls.Add(DefaultView);
                DefaultView.Dock = DockStyle.Fill;
            }
        }
    }
}
