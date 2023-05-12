
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
            this.buttonClearLog = new System.Windows.Forms.Button();
            this.buttonCopyText = new System.Windows.Forms.Button();
            this.buttonKeepScrolling = new System.Windows.Forms.Button();
            this.buttonLogcatClear = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonGetEvent
            // 
            this.buttonGetEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetEvent.BackColor = System.Drawing.Color.White;
            this.buttonGetEvent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGetEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGetEvent.ForeColor = System.Drawing.Color.Black;
            this.buttonGetEvent.Location = new System.Drawing.Point(301, 414);
            this.buttonGetEvent.Name = "buttonGetEvent";
            this.buttonGetEvent.Size = new System.Drawing.Size(82, 23);
            this.buttonGetEvent.TabIndex = 33;
            this.buttonGetEvent.Text = "GetEvent";
            this.buttonGetEvent.UseVisualStyleBackColor = false;
            this.buttonGetEvent.Visible = false;
            this.buttonGetEvent.Click += new System.EventHandler(this.buttonGetEvent_Click);
            // 
            // buttonLogcat
            // 
            this.buttonLogcat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLogcat.BackColor = System.Drawing.Color.White;
            this.buttonLogcat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLogcat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLogcat.ForeColor = System.Drawing.Color.Black;
            this.buttonLogcat.Location = new System.Drawing.Point(240, 15);
            this.buttonLogcat.Name = "buttonLogcat";
            this.buttonLogcat.Size = new System.Drawing.Size(82, 23);
            this.buttonLogcat.TabIndex = 35;
            this.buttonLogcat.Text = "Start";
            this.buttonLogcat.UseVisualStyleBackColor = false;
            this.buttonLogcat.Click += new System.EventHandler(this.buttonLogcat_Click);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxLog.BackColor = System.Drawing.Color.Black;
            this.richTextBoxLog.ForeColor = System.Drawing.Color.White;
            this.richTextBoxLog.Location = new System.Drawing.Point(12, 12);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            this.richTextBoxLog.Size = new System.Drawing.Size(766, 381);
            this.richTextBoxLog.TabIndex = 36;
            this.richTextBoxLog.Text = "";
            this.richTextBoxLog.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxLog_LinkClicked);
            // 
            // backgroundWorkerLog
            // 
            this.backgroundWorkerLog.WorkerReportsProgress = true;
            this.backgroundWorkerLog.WorkerSupportsCancellation = true;
            this.backgroundWorkerLog.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerLog_DoWorkAsync);
            // 
            // buttonClearLog
            // 
            this.buttonClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClearLog.BackColor = System.Drawing.Color.White;
            this.buttonClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClearLog.ForeColor = System.Drawing.Color.Black;
            this.buttonClearLog.Location = new System.Drawing.Point(12, 414);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new System.Drawing.Size(82, 23);
            this.buttonClearLog.TabIndex = 38;
            this.buttonClearLog.Text = "Clear Log";
            this.buttonClearLog.UseVisualStyleBackColor = false;
            this.buttonClearLog.Click += new System.EventHandler(this.buttonClearLog_Click);
            // 
            // buttonCopyText
            // 
            this.buttonCopyText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCopyText.BackColor = System.Drawing.Color.White;
            this.buttonCopyText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCopyText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCopyText.ForeColor = System.Drawing.Color.Black;
            this.buttonCopyText.Location = new System.Drawing.Point(101, 414);
            this.buttonCopyText.Name = "buttonCopyText";
            this.buttonCopyText.Size = new System.Drawing.Size(82, 23);
            this.buttonCopyText.TabIndex = 39;
            this.buttonCopyText.Text = "Copy log";
            this.buttonCopyText.UseVisualStyleBackColor = false;
            this.buttonCopyText.Click += new System.EventHandler(this.buttonCopyText_Click);
            // 
            // buttonKeepScrolling
            // 
            this.buttonKeepScrolling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonKeepScrolling.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonKeepScrolling.Location = new System.Drawing.Point(189, 414);
            this.buttonKeepScrolling.Name = "buttonKeepScrolling";
            this.buttonKeepScrolling.Size = new System.Drawing.Size(106, 23);
            this.buttonKeepScrolling.TabIndex = 40;
            this.buttonKeepScrolling.Text = "Stop scrolling";
            this.buttonKeepScrolling.UseVisualStyleBackColor = true;
            this.buttonKeepScrolling.Click += new System.EventHandler(this.buttonKeepScrolling_Click);
            // 
            // buttonLogcatClear
            // 
            this.buttonLogcatClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLogcatClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLogcatClear.Location = new System.Drawing.Point(159, 15);
            this.buttonLogcatClear.Name = "buttonLogcatClear";
            this.buttonLogcatClear.Size = new System.Drawing.Size(75, 23);
            this.buttonLogcatClear.TabIndex = 41;
            this.buttonLogcatClear.Text = "Clear";
            this.buttonLogcatClear.UseVisualStyleBackColor = true;
            this.buttonLogcatClear.Click += new System.EventHandler(this.buttonLogcatClear_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxFilter);
            this.groupBox1.Controls.Add(this.buttonLogcatClear);
            this.groupBox1.Controls.Add(this.buttonLogcat);
            this.groupBox1.Location = new System.Drawing.Point(450, 399);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 44);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Logcat";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 44;
            this.label1.Text = "Filter:";
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilter.Location = new System.Drawing.Point(53, 17);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(100, 20);
            this.textBoxFilter.TabIndex = 43;
            // 
            // DeviceLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(790, 448);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonKeepScrolling);
            this.Controls.Add(this.buttonCopyText);
            this.Controls.Add(this.buttonClearLog);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.buttonGetEvent);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 400);
            this.Name = "DeviceLogs";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Device Logs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DeviceLogs_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonGetEvent;
        private System.Windows.Forms.Button buttonLogcat;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.ComponentModel.BackgroundWorker backgroundWorkerLog;
        private System.Windows.Forms.Button buttonClearLog;
        private System.Windows.Forms.Button buttonCopyText;
        private System.Windows.Forms.Button buttonKeepScrolling;
        private System.Windows.Forms.Button buttonLogcatClear;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Label label1;
    }
}