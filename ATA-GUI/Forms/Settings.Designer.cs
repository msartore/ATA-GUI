
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
            this.labelLatestRelease = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCredits = new System.Windows.Forms.Button();
            this.buttonCheckLastVersion = new System.Windows.Forms.Button();
            this.linkLabelChangelog = new System.Windows.Forms.LinkLabel();
            this.labelLog = new System.Windows.Forms.Label();
            this.labelCurrentRelease = new System.Windows.Forms.Label();
            this.checkBoxInitPopUp = new System.Windows.Forms.CheckBox();
            this.buttonRemoveLocalSDK = new System.Windows.Forms.Button();
            this.buttonDeleteIPHistory = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelLatestRelease
            // 
            this.labelLatestRelease.AutoSize = true;
            this.labelLatestRelease.Location = new System.Drawing.Point(236, 49);
            this.labelLatestRelease.Name = "labelLatestRelease";
            this.labelLatestRelease.Size = new System.Drawing.Size(57, 13);
            this.labelLatestRelease.TabIndex = 38;
            this.labelLatestRelease.Text = "UNKNOW";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(80, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 36;
            this.label2.Text = "Latest Release:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "Current Release:";
            // 
            // buttonCredits
            // 
            this.buttonCredits.BackColor = System.Drawing.Color.White;
            this.buttonCredits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCredits.ForeColor = System.Drawing.Color.Black;
            this.buttonCredits.Location = new System.Drawing.Point(104, 248);
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
            this.buttonCheckLastVersion.Location = new System.Drawing.Point(104, 93);
            this.buttonCheckLastVersion.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCheckLastVersion.Name = "buttonCheckLastVersion";
            this.buttonCheckLastVersion.Size = new System.Drawing.Size(160, 23);
            this.buttonCheckLastVersion.TabIndex = 34;
            this.buttonCheckLastVersion.Text = "Check Last Version";
            this.buttonCheckLastVersion.UseVisualStyleBackColor = false;
            this.buttonCheckLastVersion.Click += new System.EventHandler(this.buttonCheckLastVersion_ClickAsync);
            // 
            // linkLabelChangelog
            // 
            this.linkLabelChangelog.AutoSize = true;
            this.linkLabelChangelog.Location = new System.Drawing.Point(236, 62);
            this.linkLabelChangelog.Name = "linkLabelChangelog";
            this.linkLabelChangelog.Size = new System.Drawing.Size(58, 13);
            this.linkLabelChangelog.TabIndex = 40;
            this.linkLabelChangelog.TabStop = true;
            this.linkLabelChangelog.Text = "Changelog";
            this.linkLabelChangelog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelChangelog_LinkClicked);
            // 
            // labelLog
            // 
            this.labelLog.AutoSize = true;
            this.labelLog.Location = new System.Drawing.Point(110, 130);
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(0, 13);
            this.labelLog.TabIndex = 39;
            this.labelLog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCurrentRelease
            // 
            this.labelCurrentRelease.AutoSize = true;
            this.labelCurrentRelease.Location = new System.Drawing.Point(236, 28);
            this.labelCurrentRelease.Name = "labelCurrentRelease";
            this.labelCurrentRelease.Size = new System.Drawing.Size(57, 13);
            this.labelCurrentRelease.TabIndex = 37;
            this.labelCurrentRelease.Text = "UNKNOW";
            // 
            // checkBoxInitPopUp
            // 
            this.checkBoxInitPopUp.AutoSize = true;
            this.checkBoxInitPopUp.Location = new System.Drawing.Point(104, 226);
            this.checkBoxInitPopUp.Name = "checkBoxInitPopUp";
            this.checkBoxInitPopUp.Size = new System.Drawing.Size(145, 17);
            this.checkBoxInitPopUp.TabIndex = 41;
            this.checkBoxInitPopUp.Text = "Disable feedback pop up";
            this.checkBoxInitPopUp.UseVisualStyleBackColor = true;
            this.checkBoxInitPopUp.CheckedChanged += new System.EventHandler(this.checkBoxInitPopUp_CheckedChanged);
            // 
            // buttonRemoveLocalSDK
            // 
            this.buttonRemoveLocalSDK.BackColor = System.Drawing.Color.White;
            this.buttonRemoveLocalSDK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRemoveLocalSDK.ForeColor = System.Drawing.Color.Black;
            this.buttonRemoveLocalSDK.Location = new System.Drawing.Point(104, 167);
            this.buttonRemoveLocalSDK.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRemoveLocalSDK.Name = "buttonRemoveLocalSDK";
            this.buttonRemoveLocalSDK.Size = new System.Drawing.Size(160, 23);
            this.buttonRemoveLocalSDK.TabIndex = 42;
            this.buttonRemoveLocalSDK.Text = "Remove local SDK";
            this.buttonRemoveLocalSDK.UseVisualStyleBackColor = false;
            this.buttonRemoveLocalSDK.Click += new System.EventHandler(this.buttonRemoveLocalSDK_Click);
            // 
            // buttonDeleteIPHistory
            // 
            this.buttonDeleteIPHistory.BackColor = System.Drawing.Color.White;
            this.buttonDeleteIPHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDeleteIPHistory.ForeColor = System.Drawing.Color.Black;
            this.buttonDeleteIPHistory.Location = new System.Drawing.Point(104, 197);
            this.buttonDeleteIPHistory.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDeleteIPHistory.Name = "buttonDeleteIPHistory";
            this.buttonDeleteIPHistory.Size = new System.Drawing.Size(160, 23);
            this.buttonDeleteIPHistory.TabIndex = 43;
            this.buttonDeleteIPHistory.Text = "Delete IP History";
            this.buttonDeleteIPHistory.UseVisualStyleBackColor = false;
            this.buttonDeleteIPHistory.Click += new System.EventHandler(this.buttonDeleteIPHistory_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(363, 301);
            this.Controls.Add(this.buttonDeleteIPHistory);
            this.Controls.Add(this.buttonRemoveLocalSDK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxInitPopUp);
            this.Controls.Add(this.labelLatestRelease);
            this.Controls.Add(this.labelCurrentRelease);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelLog);
            this.Controls.Add(this.buttonCredits);
            this.Controls.Add(this.linkLabelChangelog);
            this.Controls.Add(this.buttonCheckLastVersion);
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

        private System.Windows.Forms.Label labelLatestRelease;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCredits;
        private System.Windows.Forms.Button buttonCheckLastVersion;
        private System.Windows.Forms.LinkLabel linkLabelChangelog;
        private System.Windows.Forms.Label labelLog;
        private System.Windows.Forms.Label labelCurrentRelease;
        private System.Windows.Forms.CheckBox checkBoxInitPopUp;
        private System.Windows.Forms.Button buttonRemoveLocalSDK;
        private System.Windows.Forms.Button buttonDeleteIPHistory;
    }
}