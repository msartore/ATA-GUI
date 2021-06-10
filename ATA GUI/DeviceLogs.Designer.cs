
namespace ATA_GUI
{
    partial class DeviceLogs
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
            this.buttonGetEvent = new System.Windows.Forms.Button();
            this.buttonLogcat = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.backgroundWorkerLog = new System.ComponentModel.BackgroundWorker();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonClearLog = new System.Windows.Forms.Button();
            this.buttonCopyText = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonGetEvent
            // 
            this.buttonGetEvent.BackColor = System.Drawing.Color.White;
            this.buttonGetEvent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGetEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGetEvent.ForeColor = System.Drawing.Color.Black;
            this.buttonGetEvent.Location = new System.Drawing.Point(557, 461);
            this.buttonGetEvent.Name = "buttonGetEvent";
            this.buttonGetEvent.Size = new System.Drawing.Size(82, 23);
            this.buttonGetEvent.TabIndex = 33;
            this.buttonGetEvent.Text = "GetEvent";
            this.buttonGetEvent.UseVisualStyleBackColor = false;
            this.buttonGetEvent.Click += new System.EventHandler(this.buttonGetEvent_Click);
            // 
            // buttonLogcat
            // 
            this.buttonLogcat.BackColor = System.Drawing.Color.White;
            this.buttonLogcat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLogcat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLogcat.ForeColor = System.Drawing.Color.Black;
            this.buttonLogcat.Location = new System.Drawing.Point(469, 461);
            this.buttonLogcat.Name = "buttonLogcat";
            this.buttonLogcat.Size = new System.Drawing.Size(82, 23);
            this.buttonLogcat.TabIndex = 35;
            this.buttonLogcat.Text = "Logcat";
            this.buttonLogcat.UseVisualStyleBackColor = false;
            this.buttonLogcat.Click += new System.EventHandler(this.buttonLogcat_Click);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.BackColor = System.Drawing.Color.Black;
            this.richTextBoxLog.ForeColor = System.Drawing.Color.White;
            this.richTextBoxLog.Location = new System.Drawing.Point(12, 12);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            this.richTextBoxLog.Size = new System.Drawing.Size(629, 443);
            this.richTextBoxLog.TabIndex = 36;
            this.richTextBoxLog.Text = "";
            // 
            // backgroundWorkerLog
            // 
            this.backgroundWorkerLog.WorkerReportsProgress = true;
            this.backgroundWorkerLog.WorkerSupportsCancellation = true;
            this.backgroundWorkerLog.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerLog_DoWorkAsync);
            this.backgroundWorkerLog.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerLog_ProgressChanged);
            // 
            // buttonStop
            // 
            this.buttonStop.BackColor = System.Drawing.Color.White;
            this.buttonStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStop.ForeColor = System.Drawing.Color.Black;
            this.buttonStop.Location = new System.Drawing.Point(363, 461);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(82, 23);
            this.buttonStop.TabIndex = 37;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonClearLog
            // 
            this.buttonClearLog.BackColor = System.Drawing.Color.White;
            this.buttonClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClearLog.ForeColor = System.Drawing.Color.Black;
            this.buttonClearLog.Location = new System.Drawing.Point(12, 461);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new System.Drawing.Size(82, 23);
            this.buttonClearLog.TabIndex = 38;
            this.buttonClearLog.Text = "Clear Log";
            this.buttonClearLog.UseVisualStyleBackColor = false;
            this.buttonClearLog.Click += new System.EventHandler(this.buttonClearLog_Click);
            // 
            // buttonCopyText
            // 
            this.buttonCopyText.BackColor = System.Drawing.Color.White;
            this.buttonCopyText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCopyText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCopyText.ForeColor = System.Drawing.Color.Black;
            this.buttonCopyText.Location = new System.Drawing.Point(101, 461);
            this.buttonCopyText.Name = "buttonCopyText";
            this.buttonCopyText.Size = new System.Drawing.Size(82, 23);
            this.buttonCopyText.TabIndex = 39;
            this.buttonCopyText.Text = "Copy log";
            this.buttonCopyText.UseVisualStyleBackColor = false;
            this.buttonCopyText.Click += new System.EventHandler(this.buttonCopyText_Click);
            // 
            // DeviceLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(653, 491);
            this.Controls.Add(this.buttonCopyText);
            this.Controls.Add(this.buttonClearLog);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.buttonLogcat);
            this.Controls.Add(this.buttonGetEvent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeviceLogs";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Device Logs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DeviceLogs_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonGetEvent;
        private System.Windows.Forms.Button buttonLogcat;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.ComponentModel.BackgroundWorker backgroundWorkerLog;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonClearLog;
        private System.Windows.Forms.Button buttonCopyText;
    }
}