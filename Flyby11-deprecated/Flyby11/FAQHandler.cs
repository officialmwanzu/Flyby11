using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Flyby11
{
    public class FAQHandler
    {
        // Reference to the panel that will display the FAQ
        private readonly Panel _faqPanel;

        // Delegate to update the app's status label (from main form)
        private readonly Action<string> _updateStatus;

        private FlowLayoutPanel _faqContentPanel;

        // List of individual FAQ entry steps
        private readonly List<Action<FlowLayoutPanel>> _faqSteps = new List<Action<FlowLayoutPanel>>();

        private int _currentFaqStep = 0;

        public FAQHandler(Panel faqPanel, Action<string> updateStatus)
        {
            _faqPanel = faqPanel;
            _updateStatus = updateStatus;
        }

        // Initializes the FAQ view with basic content only
        public void InitializeFAQ()
        {
            _faqPanel.AutoScroll = true;
            _faqPanel.Controls.Clear();

            _faqContentPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10),
                FlowDirection = FlowDirection.TopDown,

                WrapContents = false
            };

            // Add static FAQ content (header, ISO info, etc.)
            AddTitle(_faqContentPanel);
            AddISOFAQ(_faqContentPanel);

            _faqPanel.Controls.Add(_faqContentPanel);


            PrepareOtherFAQSteps();  // Prepare additional entries without adding them yet
            AddNextFAQStep(); // Show second FAQ entry , Donate
        }

        // Adds the next FAQ item from the list
        public void AddNextFAQStep()
        {
            if (_currentFaqStep < _faqSteps.Count)
            {
                _faqSteps[_currentFaqStep](_faqContentPanel);
                _currentFaqStep++;

                // Scroll to the newly added element
                var last = _faqContentPanel.Controls[_faqContentPanel.Controls.Count - 1];
                _faqContentPanel.ScrollControlIntoView(last);
                // Add some space below the last element
                var m = last.Margin;
                last.Margin = new Padding(m.Left, m.Top, m.Right, 20);
            }
            else
            {
                _updateStatus("Well, it looks like I've done all I can here. If you need more help, you might want to reach out to the developer, Belim.");
            }
        }

        // Fill list with FAQ methods – each step adds one block of content
        private void PrepareOtherFAQSteps()
        {
            _faqSteps.Clear();
            _currentFaqStep = 0;

            _faqSteps.Add(AddFaq2);
            _faqSteps.Add(AddFaq3);
            _faqSteps.Add(AddFaq4);
            _faqSteps.Add(AddFaq5);
            _faqSteps.Add(AddFaq6);
            _faqSteps.Add(AddFaq7);
        }

        // Adds the title section
        private void AddTitle(FlowLayoutPanel parent)
        {
            AddLabel(parent, Locales.Strings.faq_header, new Font("Segoe UI Variable Display", 10.5f, FontStyle.Bold), DockStyle.Top, ContentAlignment.MiddleCenter, 11);
        }

        // Adds the ISO download information (Q1/A1)
        private void AddISOFAQ(FlowLayoutPanel parent)
        {
            AddLabel(parent, Locales.Strings.faq_q1, new Font("Segoe UI Variable Display", 10.5f, FontStyle.Bold), ContentAlignment.TopLeft, 11);
            AddLabel(parent, Locales.Strings.faq_a1, new Font("Segoe UI Variable Display", 11), ContentAlignment.TopLeft, 11);


            var downloadOptions = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI Variable Display", 10.5f),
                Width = 300,
                Padding = new Padding(5)
            };

            downloadOptions.Items.Add("Select Download Source"); // Add default placeholder item
            downloadOptions.Items.Add(Locales.Strings.faq_downloadMS);  // Index 1
            downloadOptions.Items.Add(Locales.Strings.faq_downloadFido); // Index 2
            downloadOptions.Items.Add("Media Creation Tool (Beginner-Friendly)");  // Index 3

            downloadOptions.SelectedIndex = 0;  // Set default selection

            downloadOptions.SelectedIndexChanged += (s, e) =>
            {
                switch (downloadOptions.SelectedIndex)
                {
                    case 1:
                        // Open Microsoft ISO download page
                        Process.Start("https://www.microsoft.com/software-download/windows11");
                        break;
                    case 2:
                        // Download and run Fido script
                        DownloadAndRunFidoScript("https://raw.githubusercontent.com/pbatard/Fido/refs/heads/master/Fido.ps1");
                        break;
                    case 3:
                        // Handle Media Creation Tool download process
                        HandleMediaCreationToolDownload();
                        break;
                }
            };

            parent.Controls.Add(downloadOptions);

            /// <summary>
            /// Downloads and executes the Fido PowerShell script.
            /// </summary>
            void DownloadAndRunFidoScript(string scriptUrl)
            {
                string tempScriptPath = Path.Combine(Path.GetTempPath(), "Fido.ps1");

                using (var client = new WebClient())
                {
                    try
                    {
                        client.DownloadFile(scriptUrl, tempScriptPath);
                       _updateStatus("Fido script downloaded successfully. Running it now...");

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "powershell.exe",
                            Arguments = $"-ExecutionPolicy Bypass -File \"{tempScriptPath}\"",
                            Verb = "runas",
                            CreateNoWindow = true,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        _updateStatus("Error downloading or running Fido: " + ex.Message);
                    }
                }
            }

            /// <summary>
            /// Handles download, optional tutorial, and execution of the Media Creation Tool.
            /// </summary>
            void HandleMediaCreationToolDownload()
            {
                var infoResult = MessageBox.Show(
                    "With the Media Creation Tool, you can either upgrade your PC or create an ISO file.\n\nImportant: In the step 'Choose which media to use', make sure to select 'ISO file'.\n\nIf you're unsure, would you like to watch a recommended YouTube tutorial on how to do this?",
                    "Media Creation Tool Information",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (infoResult == DialogResult.Yes)
                {
                    Process.Start("https://www.youtube.com/watch?v=dNl1FHajW9w");
                }

                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Title = "Save Media Creation Tool";
                    saveDialog.Filter = "Executable|*.exe";
                    saveDialog.FileName = "MediaCreationTool.exe";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        using (var client = new WebClient())
                        {
                            try
                            {
                                client.DownloadFile("https://go.microsoft.com/fwlink/?linkid=2156295", saveDialog.FileName);
                                MessageBox.Show("Media Creation Tool downloaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                var runResult = MessageBox.Show("Do you want to run the Media Creation Tool now?", "Run Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (runResult == DialogResult.Yes)
                                {
                                    Process.Start(saveDialog.FileName);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error downloading the Media Creation Tool: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }
        // Adds any generic label to the flow panel
        private void AddLabel(FlowLayoutPanel parent, string text, Font font, ContentAlignment alignment, int padding = 0)
        {
            var label = new Label
            {
                Text = text,
                Font = font,
                Padding = new Padding(padding),
                TextAlign = alignment,
                AutoSize = true,
                UseCompatibleTextRendering = true
            };
            parent.Controls.Add(label);
        }

        // Overload for labels with DockStyle
        private void AddLabel(FlowLayoutPanel parent, string text, Font font, DockStyle dock, ContentAlignment alignment, int padding = 0)
        {
            var label = new Label
            {
                Text = text,
                Font = font,
                Padding = new Padding(padding),
                Dock = dock,
                TextAlign = alignment,
                AutoSize = true,
                UseCompatibleTextRendering = true,
            };
            parent.Controls.Add(label);
        }

        // Adds FAQ entry with question + answer + link
        private void AddFAQItem(FlowLayoutPanel parent, string question, string answer, string linkText, string linkUrl)
        {
            AddLabel(parent, question, new Font("Segoe UI Variable Display", 10.5f, FontStyle.Bold), ContentAlignment.TopLeft, 11);
            AddLabel(parent, answer, new Font("Segoe UI Variable Display", 11), ContentAlignment.TopLeft, 11);

            var linkLabel = new LinkLabel
            {
                Text = linkText,
                Font = new Font("Segoe UI Variable Display", 10.5f, FontStyle.Underline),
                LinkColor = Color.Blue,
                AutoSize = true,
                Padding = new Padding(10),
                TextAlign = ContentAlignment.TopLeft,
                UseCompatibleTextRendering = true,
            };

            linkLabel.LinkClicked += (sender, e) => Process.Start(linkUrl);
            parent.Controls.Add(linkLabel);
        }

        // -- Individual FAQ entries --
        // Donation
        private void AddFaq2(FlowLayoutPanel panel)
        {
            var donateContainer = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.FromArgb(255, 182, 193),
                Padding = new Padding(10),
                Margin = new Padding(0, 10, 0, 10),
                WrapContents = false
            };

            var qLabel = new Label
            {
                Text = Locales.Strings.faq_q2,
                Font = new Font("Comic Sans MS", 11.5f, FontStyle.Bold),
                AutoSize = true,
                Padding = new Padding(0, 0, 0, 5),
                UseCompatibleTextRendering = true
            };

            var aLabel = new Label
            {
                Text = Locales.Strings.faq_a2,
                Font = new Font("Segoe UI Variable Display", 11),
                AutoSize = true,
                Padding = new Padding(0, 0, 0, 5),
                UseCompatibleTextRendering = true
            };

            var donateLink = new LinkLabel
            {
                Text = Locales.Strings.faq_a2Link,
                Font = new Font("Comic Sans MS", 10.5f, FontStyle.Underline),
                LinkColor = Color.MediumBlue,
                AutoSize = true,
                UseCompatibleTextRendering = true
            };
            donateLink.LinkClicked += (sender, e) => Process.Start("https://www.paypal.com/donate?hosted_button_id=MY7HX4QLYR4KG");

            donateContainer.Controls.Add(qLabel);
            donateContainer.Controls.Add(aLabel);
            donateContainer.Controls.Add(donateLink);

            // Add to parent panel
            panel.Controls.Add(donateContainer);
        }

        private void AddFaq3(FlowLayoutPanel panel)
        {
            AddLabel(panel, Locales.Strings.faq_q3, new Font("Segoe UI Variable Display", 10.5f, FontStyle.Bold), ContentAlignment.TopLeft, 11);
            AddLabel(panel, Locales.Strings.faq_a3, new Font("Segoe UI Variable Display", 11), ContentAlignment.TopLeft, 11);
        }

        private void AddFaq4(FlowLayoutPanel panel)
        {
            AddLabel(panel, Locales.Strings.faq_q4, new Font("Segoe UI Variable Display", 10.5f, FontStyle.Bold), ContentAlignment.TopLeft, 11);
            AddLabel(panel, Locales.Strings.faq_a4, new Font("Segoe UI Variable Display", 11), ContentAlignment.TopLeft, 11);
        }

        private void AddFaq5(FlowLayoutPanel panel)
        {
            AddLabel(panel, Locales.Strings.faq_q5, new Font("Segoe UI Variable Display", 10.5f, FontStyle.Bold), ContentAlignment.TopLeft, 11);
            AddLabel(panel, Locales.Strings.faq_a5, new Font("Segoe UI Variable Display", 11), ContentAlignment.TopLeft, 11);
        }

        private void AddFaq6(FlowLayoutPanel panel)
        {
            AddLabel(panel, Locales.Strings.faq_q6, new Font("Segoe UI Variable Display", 10.5f, FontStyle.Bold), ContentAlignment.TopLeft, 11);
            AddLabel(panel, Locales.Strings.faq_a6, new Font("Segoe UI Variable Display", 11), ContentAlignment.TopLeft, 11);
        }

        private void AddFaq7(FlowLayoutPanel panel)
        {
            AddFAQItem(panel, Locales.Strings.faq_q7, Locales.Strings.faq_a7, Locales.Strings.faq_a7Link, "https://github.com/BuiltByBel");
        }
    }
}
