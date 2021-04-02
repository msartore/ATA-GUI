
namespace ATA_GUI
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.buttonCredits = new System.Windows.Forms.Button();
            this.buttonCheckLastVersion = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelCurrentRelease = new System.Windows.Forms.Label();
            this.labelLatestRelease = new System.Windows.Forms.Label();
            this.labelLog = new System.Windows.Forms.Label();
            this.linkLabelCLogLatest = new System.Windows.Forms.LinkLabel();
            this.linkLabelCLogCurrent = new System.Windows.Forms.LinkLabel();
            this.linkLabelChangelog = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // buttonCredits
            // 
            this.buttonCredits.BackColor = System.Drawing.Color.White;
            this.buttonCredits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCredits.ForeColor = System.Drawing.Color.Black;
            this.buttonCredits.Location = new System.Drawing.Point(101, 271);
            this.buttonCredits.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCredits.Name = "buttonCredits";
            this.buttonCredits.Size = new System.Drawing.Size(160, 23);
            this.buttonCredits.TabIndex = 33;
            this.buttonCredits.Text = "About";
            this.buttonCredits.UseVisualStyleBackColor = false;
            this.buttonCredits.Click += new System.EventHandler(this.buttonCredits_Click);
            // 
            // buttonCheckLastVersion
            // 
            this.buttonCheckLastVersion.BackColor = System.Drawing.Color.White;
            this.buttonCheckLastVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCheckLastVersion.ForeColor = System.Drawing.Color.Black;
            this.buttonCheckLastVersion.Location = new System.Drawing.Point(101, 170);
            this.buttonCheckLastVersion.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCheckLastVersion.Name = "buttonCheckLastVersion";
            this.buttonCheckLastVersion.Size = new System.Drawing.Size(160, 23);
            this.buttonCheckLastVersion.TabIndex = 34;
            this.buttonCheckLastVersion.Text = "Check Last Version";
            this.buttonCheckLastVersion.UseVisualStyleBackColor = false;
            this.buttonCheckLastVersion.Click += new System.EventHandler(this.buttonCheckLastVersion_ClickAsync);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "Current Release:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 36;
            this.label2.Text = "Latest Release:";
            // 
            // labelCurrentRelease
            // 
            this.labelCurrentRelease.AutoSize = true;
            this.labelCurrentRelease.Location = new System.Drawing.Point(192, 92);
            this.labelCurrentRelease.Name = "labelCurrentRelease";
            this.labelCurrentRelease.Size = new System.Drawing.Size(57, 13);
            this.labelCurrentRelease.TabIndex = 37;
            this.labelCurrentRelease.Text = "UNKNOW";
            // 
            // labelLatestRelease
            // 
            this.labelLatestRelease.AutoSize = true;
            this.labelLatestRelease.Location = new System.Drawing.Point(192, 126);
            this.labelLatestRelease.Name = "labelLatestRelease";
            this.labelLatestRelease.Size = new System.Drawing.Size(57, 13);
            this.labelLatestRelease.TabIndex = 38;
            this.labelLatestRelease.Text = "UNKNOW";
            // 
            // labelLog
            // 
            this.labelLog.AutoSize = true;
            this.labelLog.Location = new System.Drawing.Point(104, 209);
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(0, 13);
            this.labelLog.TabIndex = 39;
            // 
            // linkLabelCLogLatest
            // 
            this.linkLabelCLogLatest.Location = new System.Drawing.Point(0, 0);
            this.linkLabelCLogLatest.Name = "linkLabelCLogLatest";
            this.linkLabelCLogLatest.Size = new System.Drawing.Size(100, 23);
            this.linkLabelCLogLatest.TabIndex = 1;
            // 
            // linkLabelCLogCurrent
            // 
            this.linkLabelCLogCurrent.Location = new System.Drawing.Point(0, 0);
            this.linkLabelCLogCurrent.Name = "linkLabelCLogCurrent";
            this.linkLabelCLogCurrent.Size = new System.Drawing.Size(100, 23);
            this.linkLabelCLogCurrent.TabIndex = 0;
            // 
            // linkLabelChangelog
            // 
            this.linkLabelChangelog.AutoSize = true;
            this.linkLabelChangelog.Location = new System.Drawing.Point(192, 139);
            this.linkLabelChangelog.Name = "linkLabelChangelog";
            this.linkLabelChangelog.Size = new System.Drawing.Size(58, 13);
            this.linkLabelChangelog.TabIndex = 40;
            this.linkLabelChangelog.TabStop = true;
            this.linkLabelChangelog.Text = "Changelog";
            this.linkLabelChangelog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelChangelog_LinkClicked);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(363, 363);
            this.Controls.Add(this.linkLabelChangelog);
            this.Controls.Add(this.linkLabelCLogCurrent);
            this.Controls.Add(this.linkLabelCLogLatest);
            this.Controls.Add(this.labelLog);
            this.Controls.Add(this.labelLatestRelease);
            this.Controls.Add(this.labelCurrentRelease);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCheckLastVersion);
            this.Controls.Add(this.buttonCredits);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCredits;
        private System.Windows.Forms.Button buttonCheckLastVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelCurrentRelease;
        private System.Windows.Forms.Label labelLatestRelease;
        private System.Windows.Forms.Label labelLog;
        private System.Windows.Forms.LinkLabel linkLabelCLogLatest;
        private System.Windows.Forms.LinkLabel linkLabelCLogCurrent;
        private System.Windows.Forms.LinkLabel linkLabelChangelog;
    }
}