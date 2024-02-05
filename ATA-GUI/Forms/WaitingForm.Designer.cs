namespace ATA_GUI.Forms
{
    partial class WaitingForm
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
            labelLoading = new System.Windows.Forms.Label();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            backgroundWorkerMessages = new System.ComponentModel.BackgroundWorker();
            SuspendLayout();
            // 
            // labelLoading
            // 
            labelLoading.AutoSize = true;
            labelLoading.Location = new System.Drawing.Point(105, 70);
            labelLoading.Name = "labelLoading";
            labelLoading.Size = new System.Drawing.Size(85, 15);
            labelLoading.TabIndex = 3;
            labelLoading.Text = "Loading data...";
            labelLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar1
            // 
            progressBar1.Location = new System.Drawing.Point(43, 34);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(214, 23);
            progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            progressBar1.TabIndex = 2;
            // 
            // backgroundWorkerMessages
            // 
            backgroundWorkerMessages.WorkerReportsProgress = true;
            backgroundWorkerMessages.DoWork += backgroundWorkerMessages_DoWork;
            backgroundWorkerMessages.ProgressChanged += backgroundWorkerMessages_ProgressChanged;
            // 
            // WaitingForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(303, 104);
            Controls.Add(labelLoading);
            Controls.Add(progressBar1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Name = "WaitingForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "WaitingForm";
            Load += WaitingForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelLoading;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorkerMessages;
    }
}