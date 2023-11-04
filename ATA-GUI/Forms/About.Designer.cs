
namespace ATA_GUI
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            groupBox1 = new System.Windows.Forms.GroupBox();
            labelVersion = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            linkLabelIcons = new System.Windows.Forms.LinkLabel();
            linkLabelScrcpy = new System.Windows.Forms.LinkLabel();
            linkLabelNewtonsoft = new System.Windows.Forms.LinkLabel();
            linkLabelSDK = new System.Windows.Forms.LinkLabel();
            linkLabelDNZ = new System.Windows.Forms.LinkLabel();
            groupBox3 = new System.Windows.Forms.GroupBox();
            linkLabelWebsite = new System.Windows.Forms.LinkLabel();
            linkLabelGithub = new System.Windows.Forms.LinkLabel();
            label4 = new System.Windows.Forms.Label();
            linkLabelRepo = new System.Windows.Forms.LinkLabel();
            label3 = new System.Windows.Forms.Label();
            buttonClose = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(labelVersion);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new System.Drawing.Point(14, 16);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            groupBox1.Size = new System.Drawing.Size(421, 68);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Program Version";
            // 
            // labelVersion
            // 
            labelVersion.AutoSize = true;
            labelVersion.Location = new System.Drawing.Point(14, 42);
            labelVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelVersion.Name = "labelVersion";
            labelVersion.Size = new System.Drawing.Size(127, 17);
            labelVersion.TabIndex = 1;
            labelVersion.Text = "Version: UNKNOWN";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(14, 21);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(229, 16);
            label1.TabIndex = 0;
            label1.Text = "ATA-GUI (Advance Tool for Android™)";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(linkLabelIcons);
            groupBox2.Controls.Add(linkLabelScrcpy);
            groupBox2.Controls.Add(linkLabelNewtonsoft);
            groupBox2.Controls.Add(linkLabelSDK);
            groupBox2.Controls.Add(linkLabelDNZ);
            groupBox2.Location = new System.Drawing.Point(14, 221);
            groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            groupBox2.Size = new System.Drawing.Size(421, 161);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Assest Used";
            // 
            // linkLabelIcons
            // 
            linkLabelIcons.AutoSize = true;
            linkLabelIcons.Location = new System.Drawing.Point(14, 122);
            linkLabelIcons.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabelIcons.Name = "linkLabelIcons";
            linkLabelIcons.Size = new System.Drawing.Size(45, 17);
            linkLabelIcons.TabIndex = 4;
            linkLabelIcons.TabStop = true;
            linkLabelIcons.Text = "Icons8";
            linkLabelIcons.LinkClicked += linkLabelIcons_LinkClicked;
            // 
            // linkLabelScrcpy
            // 
            linkLabelScrcpy.AutoSize = true;
            linkLabelScrcpy.Location = new System.Drawing.Point(14, 97);
            linkLabelScrcpy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabelScrcpy.Name = "linkLabelScrcpy";
            linkLabelScrcpy.Size = new System.Drawing.Size(46, 17);
            linkLabelScrcpy.TabIndex = 3;
            linkLabelScrcpy.TabStop = true;
            linkLabelScrcpy.Text = "Scrcpy";
            linkLabelScrcpy.LinkClicked += linkLabelScrcpy_LinkClicked;
            // 
            // linkLabelNewtonsoft
            // 
            linkLabelNewtonsoft.AutoSize = true;
            linkLabelNewtonsoft.Location = new System.Drawing.Point(14, 71);
            linkLabelNewtonsoft.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabelNewtonsoft.Name = "linkLabelNewtonsoft";
            linkLabelNewtonsoft.Size = new System.Drawing.Size(102, 17);
            linkLabelNewtonsoft.TabIndex = 2;
            linkLabelNewtonsoft.TabStop = true;
            linkLabelNewtonsoft.Text = "Newtonsoft.json";
            linkLabelNewtonsoft.LinkClicked += linkLabelNewtonsoft_LinkClicked;
            // 
            // linkLabelSDK
            // 
            linkLabelSDK.AutoSize = true;
            linkLabelSDK.Location = new System.Drawing.Point(14, 46);
            linkLabelSDK.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabelSDK.Name = "linkLabelSDK";
            linkLabelSDK.Size = new System.Drawing.Size(114, 17);
            linkLabelSDK.TabIndex = 1;
            linkLabelSDK.TabStop = true;
            linkLabelSDK.Text = "SDK Platform Tool";
            linkLabelSDK.LinkClicked += linkLabelSDK_LinkClicked;
            // 
            // linkLabelDNZ
            // 
            linkLabelDNZ.AutoSize = true;
            linkLabelDNZ.Location = new System.Drawing.Point(14, 21);
            linkLabelDNZ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabelDNZ.Name = "linkLabelDNZ";
            linkLabelDNZ.Size = new System.Drawing.Size(68, 17);
            linkLabelDNZ.TabIndex = 0;
            linkLabelDNZ.TabStop = true;
            linkLabelDNZ.Text = "DotNetZip";
            linkLabelDNZ.LinkClicked += linkLabelDNZ_LinkClicked;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(linkLabelWebsite);
            groupBox3.Controls.Add(linkLabelGithub);
            groupBox3.Controls.Add(label4);
            groupBox3.Controls.Add(linkLabelRepo);
            groupBox3.Controls.Add(label3);
            groupBox3.Location = new System.Drawing.Point(14, 92);
            groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            groupBox3.Size = new System.Drawing.Size(421, 122);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Developer Info";
            // 
            // linkLabelWebsite
            // 
            linkLabelWebsite.AutoSize = true;
            linkLabelWebsite.Location = new System.Drawing.Point(14, 88);
            linkLabelWebsite.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabelWebsite.Name = "linkLabelWebsite";
            linkLabelWebsite.Size = new System.Drawing.Size(54, 17);
            linkLabelWebsite.TabIndex = 4;
            linkLabelWebsite.TabStop = true;
            linkLabelWebsite.Text = "Website";
            linkLabelWebsite.LinkClicked += linkLabelWebsite_LinkClicked;
            // 
            // linkLabelGithub
            // 
            linkLabelGithub.AutoSize = true;
            linkLabelGithub.Location = new System.Drawing.Point(14, 64);
            linkLabelGithub.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabelGithub.Name = "linkLabelGithub";
            linkLabelGithub.Size = new System.Drawing.Size(46, 17);
            linkLabelGithub.TabIndex = 3;
            linkLabelGithub.TabStop = true;
            linkLabelGithub.Text = "Github";
            linkLabelGithub.LinkClicked += linkLabelGithub_LinkClicked;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(264, 21);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(146, 17);
            label4.TabIndex = 2;
            label4.Text = "Copyright © 2021-2023";
            // 
            // linkLabelRepo
            // 
            linkLabelRepo.AutoSize = true;
            linkLabelRepo.Location = new System.Drawing.Point(14, 41);
            linkLabelRepo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabelRepo.Name = "linkLabelRepo";
            linkLabelRepo.Size = new System.Drawing.Size(71, 17);
            linkLabelRepo.TabIndex = 1;
            linkLabelRepo.TabStop = true;
            linkLabelRepo.Text = "Repository";
            linkLabelRepo.LinkClicked += linkLabelRepo_LinkClicked;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(14, 21);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(131, 17);
            label3.TabIndex = 0;
            label3.Text = "Massimiliano Sartore";
            // 
            // buttonClose
            // 
            buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            buttonClose.Location = new System.Drawing.Point(348, 390);
            buttonClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new System.Drawing.Size(88, 30);
            buttonClose.TabIndex = 3;
            buttonClose.Text = "Close";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += buttonClose_Click;
            // 
            // About
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ControlLightLight;
            ClientSize = new System.Drawing.Size(449, 432);
            Controls.Add(buttonClose);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "About";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "About";
            Load += About_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.LinkLabel linkLabelSDK;
        private System.Windows.Forms.LinkLabel linkLabelDNZ;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabelRepo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel linkLabelNewtonsoft;
        private System.Windows.Forms.LinkLabel linkLabelScrcpy;
        private System.Windows.Forms.LinkLabel linkLabelIcons;
        private System.Windows.Forms.LinkLabel linkLabelWebsite;
        private System.Windows.Forms.LinkLabel linkLabelGithub;
    }
}