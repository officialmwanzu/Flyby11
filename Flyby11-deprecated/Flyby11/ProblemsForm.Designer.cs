namespace Flyby11
{
    partial class ProblemsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewIssues = new System.Windows.Forms.DataGridView();
            this.ErrorCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Link = new System.Windows.Forms.DataGridViewLinkColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIssues)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewIssues
            // 
            this.dataGridViewIssues.AllowUserToAddRows = false;
            this.dataGridViewIssues.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewIssues.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridViewIssues.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewIssues.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewIssues.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewIssues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewIssues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ErrorCode,
            this.Link});
            this.dataGridViewIssues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewIssues.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridViewIssues.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewIssues.Name = "dataGridViewIssues";
            this.dataGridViewIssues.ReadOnly = true;
            this.dataGridViewIssues.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Variable Display", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewIssues.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewIssues.RowHeadersVisible = false;
            this.dataGridViewIssues.Size = new System.Drawing.Size(645, 377);
            this.dataGridViewIssues.TabIndex = 0;
            this.dataGridViewIssues.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewIssues_CellContentClick);
            // 
            // ErrorCode
            // 
            this.ErrorCode.FillWeight = 149.2386F;
            this.ErrorCode.HeaderText = "ErrorCode";
            this.ErrorCode.Name = "ErrorCode";
            this.ErrorCode.ReadOnly = true;
            // 
            // Link
            // 
            this.Link.FillWeight = 50.76142F;
            this.Link.HeaderText = "Link";
            this.Link.Name = "Link";
            this.Link.ReadOnly = true;
            // 
            // ProblemsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 377);
            this.Controls.Add(this.dataGridViewIssues);
            this.Name = "ProblemsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Troubleshoot compatibility issues";
            this.Load += new System.EventHandler(this.ProblemsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIssues)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewIssues;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErrorCode;
        private System.Windows.Forms.DataGridViewLinkColumn Link;
    }
}
