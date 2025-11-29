namespace Flyby11
{
    partial class ClippyUI
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
            this.components = new System.ComponentModel.Container();
            this.assetClippy = new System.Windows.Forms.PictureBox();
            this.lblClosePane = new System.Windows.Forms.Label();
            this.lblHelp = new System.Windows.Forms.Label();
            this.tt = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.assetClippy)).BeginInit();
            this.SuspendLayout();
            // 
            // assetClippy
            // 
            this.assetClippy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assetClippy.Location = new System.Drawing.Point(0, 0);
            this.assetClippy.Name = "assetClippy";
            this.assetClippy.Size = new System.Drawing.Size(163, 160);
            this.assetClippy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.assetClippy.TabIndex = 0;
            this.assetClippy.TabStop = false;
            this.assetClippy.MouseDown += new System.Windows.Forms.MouseEventHandler(this.assetClippy_MouseDown);
            this.assetClippy.MouseMove += new System.Windows.Forms.MouseEventHandler(this.assetClippy_MouseMove);
            // 
            // lblClosePane
            // 
            this.lblClosePane.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClosePane.AutoEllipsis = true;
            this.lblClosePane.AutoSize = true;
            this.lblClosePane.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblClosePane.Font = new System.Drawing.Font("Segoe MDL2 Assets", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClosePane.ForeColor = System.Drawing.Color.DimGray;
            this.lblClosePane.Location = new System.Drawing.Point(137, 9);
            this.lblClosePane.Name = "lblClosePane";
            this.lblClosePane.Size = new System.Drawing.Size(19, 13);
            this.lblClosePane.TabIndex = 4;
            this.lblClosePane.Text = "...";
            this.lblClosePane.Click += new System.EventHandler(this.lblClosePane_Click);
            // 
            // lblHelp
            // 
            this.lblHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHelp.AutoEllipsis = true;
            this.lblHelp.AutoSize = true;
            this.lblHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblHelp.Font = new System.Drawing.Font("Segoe MDL2 Assets", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHelp.ForeColor = System.Drawing.Color.DeepPink;
            this.lblHelp.Location = new System.Drawing.Point(112, 9);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(19, 13);
            this.lblHelp.TabIndex = 5;
            this.lblHelp.Text = "...";
            this.tt.SetToolTip(this.lblHelp, "Need more help?");
            this.lblHelp.Click += new System.EventHandler(this.lblHelp_Click);
            // 
            // tt
            // 
            this.tt.IsBalloon = true;
            // 
            // ClippyUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(163, 160);
            this.ControlBox = false;
            this.Controls.Add(this.lblHelp);
            this.Controls.Add(this.lblClosePane);
            this.Controls.Add(this.assetClippy);
            this.Name = "ClippyUI";
            this.Opacity = 0.9D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            ((System.ComponentModel.ISupportInitialize)(this.assetClippy)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox assetClippy;
        private System.Windows.Forms.Label lblClosePane;
        private System.Windows.Forms.Label lblHelp;
        private System.Windows.Forms.ToolTip tt;
    }
}
