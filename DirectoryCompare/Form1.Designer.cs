namespace DirectoryCompare
{
    partial class MainForm
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
            this.FirstDirectoryPath = new System.Windows.Forms.TextBox();
            this.SecondDirectoryPath = new System.Windows.Forms.TextBox();
            this.FirstBrowse = new System.Windows.Forms.Button();
            this.SecondBrowse = new System.Windows.Forms.Button();
            this.FoundOutput = new System.Windows.Forms.TextBox();
            this.Compare = new System.Windows.Forms.Button();
            this.SummaryOutput = new System.Windows.Forms.TextBox();
            this.NotFoundOutput = new System.Windows.Forms.TextBox();
            this.Browser = new System.Windows.Forms.FolderBrowserDialog();
            this.Conclusion = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // FirstDirectoryPath
            // 
            this.FirstDirectoryPath.Location = new System.Drawing.Point(12, 12);
            this.FirstDirectoryPath.Name = "FirstDirectoryPath";
            this.FirstDirectoryPath.Size = new System.Drawing.Size(367, 20);
            this.FirstDirectoryPath.TabIndex = 0;
            // 
            // SecondDirectoryPath
            // 
            this.SecondDirectoryPath.Location = new System.Drawing.Point(12, 38);
            this.SecondDirectoryPath.Name = "SecondDirectoryPath";
            this.SecondDirectoryPath.Size = new System.Drawing.Size(367, 20);
            this.SecondDirectoryPath.TabIndex = 1;
            // 
            // FirstBrowse
            // 
            this.FirstBrowse.Location = new System.Drawing.Point(385, 10);
            this.FirstBrowse.Name = "FirstBrowse";
            this.FirstBrowse.Size = new System.Drawing.Size(75, 23);
            this.FirstBrowse.TabIndex = 2;
            this.FirstBrowse.Text = "Browse...";
            this.FirstBrowse.UseVisualStyleBackColor = true;
            this.FirstBrowse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // SecondBrowse
            // 
            this.SecondBrowse.Location = new System.Drawing.Point(385, 36);
            this.SecondBrowse.Name = "SecondBrowse";
            this.SecondBrowse.Size = new System.Drawing.Size(75, 23);
            this.SecondBrowse.TabIndex = 3;
            this.SecondBrowse.Text = "Browse...";
            this.SecondBrowse.UseVisualStyleBackColor = true;
            this.SecondBrowse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // FoundOutput
            // 
            this.FoundOutput.BackColor = System.Drawing.SystemColors.Window;
            this.FoundOutput.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.FoundOutput.Location = new System.Drawing.Point(12, 64);
            this.FoundOutput.Multiline = true;
            this.FoundOutput.Name = "FoundOutput";
            this.FoundOutput.ReadOnly = true;
            this.FoundOutput.Size = new System.Drawing.Size(162, 173);
            this.FoundOutput.TabIndex = 4;
            this.FoundOutput.Text = "Found Output...";
            // 
            // Compare
            // 
            this.Compare.Location = new System.Drawing.Point(342, 64);
            this.Compare.Name = "Compare";
            this.Compare.Size = new System.Drawing.Size(115, 37);
            this.Compare.TabIndex = 5;
            this.Compare.Text = "Compare";
            this.Compare.UseVisualStyleBackColor = true;
            this.Compare.Click += new System.EventHandler(this.Compare_Click);
            // 
            // SummaryOutput
            // 
            this.SummaryOutput.BackColor = System.Drawing.SystemColors.Window;
            this.SummaryOutput.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.SummaryOutput.Location = new System.Drawing.Point(342, 107);
            this.SummaryOutput.Multiline = true;
            this.SummaryOutput.Name = "SummaryOutput";
            this.SummaryOutput.ReadOnly = true;
            this.SummaryOutput.Size = new System.Drawing.Size(115, 130);
            this.SummaryOutput.TabIndex = 6;
            this.SummaryOutput.Text = "Summary Output...";
            // 
            // NotFoundOutput
            // 
            this.NotFoundOutput.BackColor = System.Drawing.SystemColors.Window;
            this.NotFoundOutput.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.NotFoundOutput.Location = new System.Drawing.Point(180, 64);
            this.NotFoundOutput.Multiline = true;
            this.NotFoundOutput.Name = "NotFoundOutput";
            this.NotFoundOutput.ReadOnly = true;
            this.NotFoundOutput.Size = new System.Drawing.Size(156, 173);
            this.NotFoundOutput.TabIndex = 7;
            this.NotFoundOutput.Text = "Not Found Output...";
            // 
            // Conclusion
            // 
            this.Conclusion.BackColor = System.Drawing.SystemColors.Window;
            this.Conclusion.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.Conclusion.Location = new System.Drawing.Point(12, 243);
            this.Conclusion.Multiline = true;
            this.Conclusion.Name = "Conclusion";
            this.Conclusion.ReadOnly = true;
            this.Conclusion.Size = new System.Drawing.Size(445, 40);
            this.Conclusion.TabIndex = 8;
            this.Conclusion.Text = "Conclusion...";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 293);
            this.Controls.Add(this.Conclusion);
            this.Controls.Add(this.NotFoundOutput);
            this.Controls.Add(this.SummaryOutput);
            this.Controls.Add(this.Compare);
            this.Controls.Add(this.FoundOutput);
            this.Controls.Add(this.SecondBrowse);
            this.Controls.Add(this.FirstBrowse);
            this.Controls.Add(this.SecondDirectoryPath);
            this.Controls.Add(this.FirstDirectoryPath);
            this.Name = "MainForm";
            this.Text = "Directory Compare";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox FirstDirectoryPath;
        private System.Windows.Forms.TextBox SecondDirectoryPath;
        private System.Windows.Forms.Button FirstBrowse;
        private System.Windows.Forms.Button SecondBrowse;
        private System.Windows.Forms.TextBox FoundOutput;
        private System.Windows.Forms.Button Compare;
        private System.Windows.Forms.TextBox SummaryOutput;
        private System.Windows.Forms.TextBox NotFoundOutput;
        private System.Windows.Forms.FolderBrowserDialog Browser;
        private System.Windows.Forms.TextBox Conclusion;
    }
}

