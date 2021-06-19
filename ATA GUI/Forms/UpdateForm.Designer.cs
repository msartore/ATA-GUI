
namespace ATA_GUI
{
    partial class UpdateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateForm));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelLog = new System.Windows.Forms.Label();
            this.labelWarning = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(43, 35);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(249, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // labelLog
            // 
            this.labelLog.AutoSize = true;
            this.labelLog.Location = new System.Drawing.Point(40, 77);
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(0, 13);
            this.labelLog.TabIndex = 1;
            // 
            // labelWarning
            // 
            this.labelWarning.AutoSize = true;
            this.labelWarning.Location = new System.Drawing.Point(40, 101);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(0, 13);
            this.labelWarning.TabIndex = 41;
            // 
            // UpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 125);
            this.Controls.Add(this.labelWarning);
            this.Controls.Add(this.labelLog);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UpdateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UpdateForm";
            this.Shown += new System.EventHandler(this.UpdateForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label labelLog;
        private System.Windows.Forms.Label labelWarning;
    }
}