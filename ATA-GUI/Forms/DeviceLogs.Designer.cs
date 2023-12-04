
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
            buttonGetEvent = new System.Windows.Forms.Button();
            buttonLogcat = new System.Windows.Forms.Button();
            richTextBoxLog = new System.Windows.Forms.RichTextBox();
            backgroundWorkerLog = new System.ComponentModel.BackgroundWorker();
            buttonClearLog = new System.Windows.Forms.Button();
            buttonCopyText = new System.Windows.Forms.Button();
            buttonKeepScrolling = new System.Windows.Forms.Button();
            buttonLogcatClear = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            textBoxFilter = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            labelCCount = new System.Windows.Forms.Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // buttonGetEvent
            // 
            buttonGetEvent.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonGetEvent.BackColor = System.Drawing.Color.White;
            buttonGetEvent.FlatStyle = System.Windows.Forms.FlatStyle.System;
            buttonGetEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            buttonGetEvent.ForeColor = System.Drawing.Color.Black;
            buttonGetEvent.Location = new System.Drawing.Point(351, 478);
            buttonGetEvent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonGetEvent.Name = "buttonGetEvent";
            buttonGetEvent.Size = new System.Drawing.Size(96, 27);
            buttonGetEvent.TabIndex = 33;
            buttonGetEvent.Text = "GetEvent";
            buttonGetEvent.UseVisualStyleBackColor = false;
            buttonGetEvent.Visible = false;
            buttonGetEvent.Click += buttonGetEvent_Click;
            // 
            // buttonLogcat
            // 
            buttonLogcat.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonLogcat.BackColor = System.Drawing.Color.White;
            buttonLogcat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            buttonLogcat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            buttonLogcat.ForeColor = System.Drawing.Color.Black;
            buttonLogcat.Location = new System.Drawing.Point(280, 17);
            buttonLogcat.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonLogcat.Name = "buttonLogcat";
            buttonLogcat.Size = new System.Drawing.Size(96, 27);
            buttonLogcat.TabIndex = 35;
            buttonLogcat.Text = "Start";
            buttonLogcat.UseVisualStyleBackColor = false;
            buttonLogcat.Click += buttonLogcat_Click;
            // 
            // richTextBoxLog
            // 
            richTextBoxLog.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            richTextBoxLog.BackColor = System.Drawing.Color.Black;
            richTextBoxLog.ForeColor = System.Drawing.Color.White;
            richTextBoxLog.Location = new System.Drawing.Point(14, 14);
            richTextBoxLog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            richTextBoxLog.Name = "richTextBoxLog";
            richTextBoxLog.ReadOnly = true;
            richTextBoxLog.Size = new System.Drawing.Size(893, 439);
            richTextBoxLog.TabIndex = 36;
            richTextBoxLog.Text = "";
            richTextBoxLog.LinkClicked += richTextBoxLog_LinkClicked;
            // 
            // backgroundWorkerLog
            // 
            backgroundWorkerLog.WorkerReportsProgress = true;
            backgroundWorkerLog.WorkerSupportsCancellation = true;
            backgroundWorkerLog.DoWork += backgroundWorkerLog_DoWorkAsync;
            // 
            // buttonClearLog
            // 
            buttonClearLog.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            buttonClearLog.BackColor = System.Drawing.Color.White;
            buttonClearLog.FlatStyle = System.Windows.Forms.FlatStyle.System;
            buttonClearLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            buttonClearLog.ForeColor = System.Drawing.Color.Black;
            buttonClearLog.Location = new System.Drawing.Point(14, 478);
            buttonClearLog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonClearLog.Name = "buttonClearLog";
            buttonClearLog.Size = new System.Drawing.Size(96, 27);
            buttonClearLog.TabIndex = 38;
            buttonClearLog.Text = "Clear Log";
            buttonClearLog.UseVisualStyleBackColor = false;
            buttonClearLog.Click += buttonClearLog_Click;
            // 
            // buttonCopyText
            // 
            buttonCopyText.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            buttonCopyText.BackColor = System.Drawing.Color.White;
            buttonCopyText.FlatStyle = System.Windows.Forms.FlatStyle.System;
            buttonCopyText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            buttonCopyText.ForeColor = System.Drawing.Color.Black;
            buttonCopyText.Location = new System.Drawing.Point(118, 478);
            buttonCopyText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonCopyText.Name = "buttonCopyText";
            buttonCopyText.Size = new System.Drawing.Size(96, 27);
            buttonCopyText.TabIndex = 39;
            buttonCopyText.Text = "Copy log";
            buttonCopyText.UseVisualStyleBackColor = false;
            buttonCopyText.Click += buttonCopyText_Click;
            // 
            // buttonKeepScrolling
            // 
            buttonKeepScrolling.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            buttonKeepScrolling.FlatStyle = System.Windows.Forms.FlatStyle.System;
            buttonKeepScrolling.Location = new System.Drawing.Point(220, 478);
            buttonKeepScrolling.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonKeepScrolling.Name = "buttonKeepScrolling";
            buttonKeepScrolling.Size = new System.Drawing.Size(124, 27);
            buttonKeepScrolling.TabIndex = 40;
            buttonKeepScrolling.Text = "Stop scrolling";
            buttonKeepScrolling.UseVisualStyleBackColor = true;
            buttonKeepScrolling.Click += buttonKeepScrolling_Click;
            // 
            // buttonLogcatClear
            // 
            buttonLogcatClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonLogcatClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            buttonLogcatClear.Location = new System.Drawing.Point(186, 17);
            buttonLogcatClear.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonLogcatClear.Name = "buttonLogcatClear";
            buttonLogcatClear.Size = new System.Drawing.Size(88, 27);
            buttonLogcatClear.TabIndex = 41;
            buttonLogcatClear.Text = "Clear";
            buttonLogcatClear.UseVisualStyleBackColor = true;
            buttonLogcatClear.Click += buttonLogcatClear_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(textBoxFilter);
            groupBox1.Controls.Add(buttonLogcatClear);
            groupBox1.Controls.Add(buttonLogcat);
            groupBox1.Location = new System.Drawing.Point(525, 460);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(383, 51);
            groupBox1.TabIndex = 42;
            groupBox1.TabStop = false;
            groupBox1.Text = "Logcat";
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(18, 23);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(36, 15);
            label1.TabIndex = 44;
            label1.Text = "Filter:";
            // 
            // textBoxFilter
            // 
            textBoxFilter.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            textBoxFilter.Location = new System.Drawing.Point(62, 20);
            textBoxFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxFilter.Name = "textBoxFilter";
            textBoxFilter.Size = new System.Drawing.Size(116, 23);
            textBoxFilter.TabIndex = 43;
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(14, 457);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(95, 15);
            label2.TabIndex = 43;
            label2.Text = "Character count:";
            // 
            // labelCCount
            // 
            labelCCount.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            labelCCount.AutoSize = true;
            labelCCount.Location = new System.Drawing.Point(114, 457);
            labelCCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelCCount.Name = "labelCCount";
            labelCCount.Size = new System.Drawing.Size(13, 15);
            labelCCount.TabIndex = 44;
            labelCCount.Text = "0";
            // 
            // DeviceLogs
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(922, 517);
            Controls.Add(labelCCount);
            Controls.Add(label2);
            Controls.Add(groupBox1);
            Controls.Add(buttonKeepScrolling);
            Controls.Add(buttonCopyText);
            Controls.Add(buttonClearLog);
            Controls.Add(richTextBoxLog);
            Controls.Add(buttonGetEvent);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(814, 456);
            Name = "DeviceLogs";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Device Logs";
            FormClosing += DeviceLogs_FormClosing;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelCCount;
    }
}