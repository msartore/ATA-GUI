
using System;

namespace ATA_GUI
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            labelLatestRelease = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            buttonCredits = new System.Windows.Forms.Button();
            buttonCheckLastVersion = new System.Windows.Forms.Button();
            linkLabelChangelog = new System.Windows.Forms.LinkLabel();
            labelLog = new System.Windows.Forms.Label();
            labelCurrentRelease = new System.Windows.Forms.Label();
            checkBoxInitPopUp = new System.Windows.Forms.CheckBox();
            buttonUpdateLocalSDK = new System.Windows.Forms.Button();
            buttonDeleteIPHistory = new System.Windows.Forms.Button();
            buttonATABridgeDownload = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // labelLatestRelease
            // 
            labelLatestRelease.AutoSize = true;
            labelLatestRelease.Location = new System.Drawing.Point(275, 57);
            labelLatestRelease.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelLatestRelease.Name = "labelLatestRelease";
            labelLatestRelease.Size = new System.Drawing.Size(60, 15);
            labelLatestRelease.TabIndex = 38;
            labelLatestRelease.Text = "UNKNOW";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(93, 57);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(83, 15);
            label2.TabIndex = 36;
            label2.Text = "Latest Release:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(93, 32);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(92, 15);
            label1.TabIndex = 35;
            label1.Text = "Current Release:";
            // 
            // buttonCredits
            // 
            buttonCredits.BackColor = System.Drawing.Color.White;
            buttonCredits.FlatStyle = System.Windows.Forms.FlatStyle.System;
            buttonCredits.ForeColor = System.Drawing.Color.Black;
            buttonCredits.Location = new System.Drawing.Point(121, 298);
            buttonCredits.Margin = new System.Windows.Forms.Padding(2);
            buttonCredits.Name = "buttonCredits";
            buttonCredits.Size = new System.Drawing.Size(187, 27);
            buttonCredits.TabIndex = 33;
            buttonCredits.Text = "About";
            buttonCredits.UseVisualStyleBackColor = false;
            buttonCredits.Click += ButtonCredits_Click;
            // 
            // buttonCheckLastVersion
            // 
            buttonCheckLastVersion.BackColor = System.Drawing.Color.White;
            buttonCheckLastVersion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            buttonCheckLastVersion.ForeColor = System.Drawing.Color.Black;
            buttonCheckLastVersion.Location = new System.Drawing.Point(121, 205);
            buttonCheckLastVersion.Margin = new System.Windows.Forms.Padding(2);
            buttonCheckLastVersion.Name = "buttonCheckLastVersion";
            buttonCheckLastVersion.Size = new System.Drawing.Size(187, 27);
            buttonCheckLastVersion.TabIndex = 34;
            buttonCheckLastVersion.Text = "Check Last Version";
            buttonCheckLastVersion.UseVisualStyleBackColor = false;
            buttonCheckLastVersion.Click += ButtonCheckLastVersion_ClickAsync;
            // 
            // linkLabelChangelog
            // 
            linkLabelChangelog.AutoSize = true;
            linkLabelChangelog.Location = new System.Drawing.Point(275, 72);
            linkLabelChangelog.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabelChangelog.Name = "linkLabelChangelog";
            linkLabelChangelog.Size = new System.Drawing.Size(65, 15);
            linkLabelChangelog.TabIndex = 40;
            linkLabelChangelog.TabStop = true;
            linkLabelChangelog.Text = "Changelog";
            linkLabelChangelog.LinkClicked += LinkLabelChangelog_LinkClicked;
            // 
            // labelLog
            // 
            labelLog.AutoSize = true;
            labelLog.Location = new System.Drawing.Point(93, 98);
            labelLog.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelLog.Name = "labelLog";
            labelLog.Size = new System.Drawing.Size(0, 15);
            labelLog.TabIndex = 39;
            labelLog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCurrentRelease
            // 
            labelCurrentRelease.AutoSize = true;
            labelCurrentRelease.Location = new System.Drawing.Point(275, 32);
            labelCurrentRelease.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelCurrentRelease.Name = "labelCurrentRelease";
            labelCurrentRelease.Size = new System.Drawing.Size(60, 15);
            labelCurrentRelease.TabIndex = 37;
            labelCurrentRelease.Text = "UNKNOW";
            // 
            // checkBoxInitPopUp
            // 
            checkBoxInitPopUp.AutoSize = true;
            checkBoxInitPopUp.Location = new System.Drawing.Point(121, 149);
            checkBoxInitPopUp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxInitPopUp.Name = "checkBoxInitPopUp";
            checkBoxInitPopUp.Size = new System.Drawing.Size(156, 19);
            checkBoxInitPopUp.TabIndex = 41;
            checkBoxInitPopUp.Text = "Disable feedback pop up";
            checkBoxInitPopUp.UseVisualStyleBackColor = true;
            checkBoxInitPopUp.CheckedChanged += CheckBoxInitPopUp_CheckedChanged;
            // 
            // buttonUpdateLocalSDK
            // 
            buttonUpdateLocalSDK.BackColor = System.Drawing.Color.White;
            buttonUpdateLocalSDK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            buttonUpdateLocalSDK.ForeColor = System.Drawing.Color.Black;
            buttonUpdateLocalSDK.Location = new System.Drawing.Point(121, 236);
            buttonUpdateLocalSDK.Margin = new System.Windows.Forms.Padding(2);
            buttonUpdateLocalSDK.Name = "buttonUpdateLocalSDK";
            buttonUpdateLocalSDK.Size = new System.Drawing.Size(187, 27);
            buttonUpdateLocalSDK.TabIndex = 42;
            buttonUpdateLocalSDK.Text = "Update local SDK Platform Tools";
            buttonUpdateLocalSDK.UseVisualStyleBackColor = false;
            buttonUpdateLocalSDK.Click += ButtonUpdateLocalSDK_Click;
            // 
            // buttonDeleteIPHistory
            // 
            buttonDeleteIPHistory.BackColor = System.Drawing.Color.White;
            buttonDeleteIPHistory.FlatStyle = System.Windows.Forms.FlatStyle.System;
            buttonDeleteIPHistory.ForeColor = System.Drawing.Color.Black;
            buttonDeleteIPHistory.Location = new System.Drawing.Point(121, 267);
            buttonDeleteIPHistory.Margin = new System.Windows.Forms.Padding(2);
            buttonDeleteIPHistory.Name = "buttonDeleteIPHistory";
            buttonDeleteIPHistory.Size = new System.Drawing.Size(187, 27);
            buttonDeleteIPHistory.TabIndex = 43;
            buttonDeleteIPHistory.Text = "Delete IP History";
            buttonDeleteIPHistory.UseVisualStyleBackColor = false;
            buttonDeleteIPHistory.Click += ButtonDeleteIPHistory_Click;
            // 
            // buttonATABridgeDownload
            // 
            buttonATABridgeDownload.Enabled = false;
            buttonATABridgeDownload.Location = new System.Drawing.Point(121, 174);
            buttonATABridgeDownload.Name = "buttonATABridgeDownload";
            buttonATABridgeDownload.Size = new System.Drawing.Size(187, 27);
            buttonATABridgeDownload.TabIndex = 44;
            buttonATABridgeDownload.Text = "ATABridge Coming Soon!";
            buttonATABridgeDownload.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(424, 347);
            Controls.Add(buttonATABridgeDownload);
            Controls.Add(buttonDeleteIPHistory);
            Controls.Add(buttonUpdateLocalSDK);
            Controls.Add(label1);
            Controls.Add(checkBoxInitPopUp);
            Controls.Add(labelLatestRelease);
            Controls.Add(labelCurrentRelease);
            Controls.Add(label2);
            Controls.Add(labelLog);
            Controls.Add(buttonCredits);
            Controls.Add(linkLabelChangelog);
            Controls.Add(buttonCheckLastVersion);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Settings";
            Load += Settings_Load;
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.Button buttonUpdateLocalSDK;
        private System.Windows.Forms.Button buttonDeleteIPHistory;
        private System.Windows.Forms.Button buttonATABridgeDownload;
    }
}