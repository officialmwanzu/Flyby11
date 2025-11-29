namespace Flyby11
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelDragDrop = new System.Windows.Forms.Panel();
            this.dropdownOptions = new System.Windows.Forms.ComboBox();
            this.btnExperience = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.panelFAQ = new System.Windows.Forms.Panel();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.chkAdvancedMode = new System.Windows.Forms.CheckBox();
            this.panelDragDrop.SuspendLayout();
            this.panelContainer.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelDragDrop
            // 
            this.panelDragDrop.AllowDrop = true;
            this.panelDragDrop.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelDragDrop.BackColor = System.Drawing.Color.Transparent;
            this.panelDragDrop.Controls.Add(this.dropdownOptions);
            this.panelDragDrop.Controls.Add(this.btnExperience);
            this.panelDragDrop.Controls.Add(this.statusLabel);
            this.panelDragDrop.Location = new System.Drawing.Point(57, 190);
            this.panelDragDrop.Name = "panelDragDrop";
            this.panelDragDrop.Size = new System.Drawing.Size(436, 256);
            this.panelDragDrop.TabIndex = 0;
            this.panelDragDrop.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelDragDrop_DragDrop);
            this.panelDragDrop.DragEnter += new System.Windows.Forms.DragEventHandler(this.panelDragDrop_DragEnter);
            this.panelDragDrop.Paint += new System.Windows.Forms.PaintEventHandler(this.panelDragDrop_Paint);
            // 
            // dropdownOptions
            // 
            this.dropdownOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropdownOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropdownOptions.Font = new System.Drawing.Font("Segoe UI Variable Display", 11.5F);
            this.dropdownOptions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.dropdownOptions.FormattingEnabled = true;
            this.dropdownOptions.Location = new System.Drawing.Point(51, 205);
            this.dropdownOptions.Name = "dropdownOptions";
            this.dropdownOptions.Size = new System.Drawing.Size(348, 28);
            this.dropdownOptions.TabIndex = 507;
            this.dropdownOptions.SelectedIndexChanged += new System.EventHandler(this.dropdownOptions_SelectedIndexChanged);
            // 
            // btnExperience
            // 
            this.btnExperience.AutoEllipsis = true;
            this.btnExperience.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(239)))), ((int)(((byte)(237)))));
            this.btnExperience.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExperience.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExperience.FlatAppearance.BorderSize = 0;
            this.btnExperience.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnExperience.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 12F, System.Drawing.FontStyle.Bold);
            this.btnExperience.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.btnExperience.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExperience.Location = new System.Drawing.Point(21, 195);
            this.btnExperience.Name = "btnExperience";
            this.btnExperience.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.btnExperience.Size = new System.Drawing.Size(387, 49);
            this.btnExperience.TabIndex = 506;
            this.btnExperience.TabStop = false;
            this.btnExperience.UseCompatibleTextRendering = true;
            this.btnExperience.UseVisualStyleBackColor = false;
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.statusLabel.AutoEllipsis = true;
            this.statusLabel.BackColor = System.Drawing.Color.Transparent;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(39, 35);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(352, 142);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "Move ISO here";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.statusLabel.UseCompatibleTextRendering = true;
            // 
            // panelFAQ
            // 
            this.panelFAQ.AutoScroll = true;
            this.panelFAQ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.panelFAQ.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelFAQ.Location = new System.Drawing.Point(589, 0);
            this.panelFAQ.Name = "panelFAQ";
            this.panelFAQ.Size = new System.Drawing.Size(331, 609);
            this.panelFAQ.TabIndex = 207;
            // 
            // panelContainer
            // 
            this.panelContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.panelContainer.Controls.Add(this.panelMain);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 0);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(589, 609);
            this.panelContainer.TabIndex = 209;
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.panelMain.Controls.Add(this.chkAdvancedMode);
            this.panelMain.Controls.Add(this.panelDragDrop);
            this.panelMain.Location = new System.Drawing.Point(5, 3);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(580, 596);
            this.panelMain.TabIndex = 1;
            // 
            // chkAdvancedMode
            // 
            this.chkAdvancedMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAdvancedMode.AutoSize = true;
            this.chkAdvancedMode.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAdvancedMode.Location = new System.Drawing.Point(8, 566);
            this.chkAdvancedMode.Name = "chkAdvancedMode";
            this.chkAdvancedMode.Size = new System.Drawing.Size(397, 17);
            this.chkAdvancedMode.TabIndex = 1;
            this.chkAdvancedMode.Text = "Enable advanced upgrade mode (bypass compatibility and driver checks)";
            this.chkAdvancedMode.UseVisualStyleBackColor = true;
            this.chkAdvancedMode.CheckedChanged += new System.EventHandler(this.chkAdvancedMode_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(920, 609);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.panelFAQ);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Flyby11 Upgrading Assistant";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.panelDragDrop.ResumeLayout(false);
            this.panelContainer.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelDragDrop;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Panel panelFAQ;
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Button btnExperience;
        private System.Windows.Forms.ComboBox dropdownOptions;
        private System.Windows.Forms.CheckBox chkAdvancedMode;
    }
}
