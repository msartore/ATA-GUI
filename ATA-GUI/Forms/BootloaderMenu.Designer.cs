
namespace ATA_GUI
{
    partial class BootloaderMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BootloaderMenu));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonLB2014 = new System.Windows.Forms.Button();
            this.buttonUB2014 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonLB2015 = new System.Windows.Forms.Button();
            this.buttonUB2015 = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonVivoLock = new System.Windows.Forms.Button();
            this.buttonVivoUnlock = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonDID = new System.Windows.Forms.Button();
            this.buttonDI = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonLB2014);
            this.groupBox1.Controls.Add(this.buttonUB2014);
            this.groupBox1.Location = new System.Drawing.Point(12, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 83);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "2014 and earlier";
            // 
            // buttonLB2014
            // 
            this.buttonLB2014.BackColor = System.Drawing.Color.White;
            this.buttonLB2014.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLB2014.ForeColor = System.Drawing.Color.Black;
            this.buttonLB2014.Location = new System.Drawing.Point(43, 45);
            this.buttonLB2014.Margin = new System.Windows.Forms.Padding(2);
            this.buttonLB2014.Name = "buttonLB2014";
            this.buttonLB2014.Size = new System.Drawing.Size(118, 23);
            this.buttonLB2014.TabIndex = 2;
            this.buttonLB2014.Text = "Lock Bootloader";
            this.buttonLB2014.UseVisualStyleBackColor = false;
            this.buttonLB2014.Click += new System.EventHandler(this.buttonLB2014_Click);
            // 
            // buttonUB2014
            // 
            this.buttonUB2014.BackColor = System.Drawing.Color.White;
            this.buttonUB2014.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUB2014.ForeColor = System.Drawing.Color.Black;
            this.buttonUB2014.Location = new System.Drawing.Point(43, 18);
            this.buttonUB2014.Margin = new System.Windows.Forms.Padding(2);
            this.buttonUB2014.Name = "buttonUB2014";
            this.buttonUB2014.Size = new System.Drawing.Size(118, 23);
            this.buttonUB2014.TabIndex = 0;
            this.buttonUB2014.Text = "Unlock Bootloader";
            this.buttonUB2014.UseVisualStyleBackColor = false;
            this.buttonUB2014.Click += new System.EventHandler(this.buttonUB2014_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonLB2015);
            this.groupBox2.Controls.Add(this.buttonUB2015);
            this.groupBox2.Location = new System.Drawing.Point(218, 29);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 83);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2015 and later";
            // 
            // buttonLB2015
            // 
            this.buttonLB2015.BackColor = System.Drawing.Color.White;
            this.buttonLB2015.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLB2015.ForeColor = System.Drawing.Color.Black;
            this.buttonLB2015.Location = new System.Drawing.Point(42, 45);
            this.buttonLB2015.Margin = new System.Windows.Forms.Padding(2);
            this.buttonLB2015.Name = "buttonLB2015";
            this.buttonLB2015.Size = new System.Drawing.Size(118, 23);
            this.buttonLB2015.TabIndex = 4;
            this.buttonLB2015.Text = "Lock Bootloader";
            this.buttonLB2015.UseVisualStyleBackColor = false;
            this.buttonLB2015.Click += new System.EventHandler(this.buttonLB2015_Click);
            // 
            // buttonUB2015
            // 
            this.buttonUB2015.BackColor = System.Drawing.Color.White;
            this.buttonUB2015.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUB2015.ForeColor = System.Drawing.Color.Black;
            this.buttonUB2015.Location = new System.Drawing.Point(42, 18);
            this.buttonUB2015.Margin = new System.Windows.Forms.Padding(2);
            this.buttonUB2015.Name = "buttonUB2015";
            this.buttonUB2015.Size = new System.Drawing.Size(118, 23);
            this.buttonUB2015.TabIndex = 3;
            this.buttonUB2015.Text = "Unlock Bootloader";
            this.buttonUB2015.UseVisualStyleBackColor = false;
            this.buttonUB2015.Click += new System.EventHandler(this.buttonUB2015_Click);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.BackColor = System.Drawing.Color.Black;
            this.richTextBoxLog.ForeColor = System.Drawing.Color.White;
            this.richTextBoxLog.Location = new System.Drawing.Point(12, 234);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(406, 204);
            this.richTextBoxLog.TabIndex = 56;
            this.richTextBoxLog.TabStop = false;
            this.richTextBoxLog.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 215);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Log:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Crimson;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "(not all device supported)";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonVivoLock);
            this.groupBox3.Controls.Add(this.buttonVivoUnlock);
            this.groupBox3.Location = new System.Drawing.Point(12, 118);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 80);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Vivo Devices";
            // 
            // buttonVivoLock
            // 
            this.buttonVivoLock.BackColor = System.Drawing.Color.White;
            this.buttonVivoLock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonVivoLock.ForeColor = System.Drawing.Color.Black;
            this.buttonVivoLock.Location = new System.Drawing.Point(43, 45);
            this.buttonVivoLock.Margin = new System.Windows.Forms.Padding(2);
            this.buttonVivoLock.Name = "buttonVivoLock";
            this.buttonVivoLock.Size = new System.Drawing.Size(118, 23);
            this.buttonVivoLock.TabIndex = 6;
            this.buttonVivoLock.Text = "Lock Bootloader";
            this.buttonVivoLock.UseVisualStyleBackColor = false;
            this.buttonVivoLock.Click += new System.EventHandler(this.buttonVivoLock_Click);
            // 
            // buttonVivoUnlock
            // 
            this.buttonVivoUnlock.BackColor = System.Drawing.Color.White;
            this.buttonVivoUnlock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonVivoUnlock.ForeColor = System.Drawing.Color.Black;
            this.buttonVivoUnlock.Location = new System.Drawing.Point(43, 18);
            this.buttonVivoUnlock.Margin = new System.Windows.Forms.Padding(2);
            this.buttonVivoUnlock.Name = "buttonVivoUnlock";
            this.buttonVivoUnlock.Size = new System.Drawing.Size(118, 23);
            this.buttonVivoUnlock.TabIndex = 5;
            this.buttonVivoUnlock.Text = "Unlock Bootloader";
            this.buttonVivoUnlock.UseVisualStyleBackColor = false;
            this.buttonVivoUnlock.Click += new System.EventHandler(this.buttonVivoUnlock_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonDID);
            this.groupBox4.Controls.Add(this.buttonDI);
            this.groupBox4.Location = new System.Drawing.Point(218, 118);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 80);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Tool Box";
            // 
            // buttonDID
            // 
            this.buttonDID.BackColor = System.Drawing.Color.White;
            this.buttonDID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDID.ForeColor = System.Drawing.Color.Black;
            this.buttonDID.Location = new System.Drawing.Point(42, 45);
            this.buttonDID.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDID.Name = "buttonDID";
            this.buttonDID.Size = new System.Drawing.Size(118, 23);
            this.buttonDID.TabIndex = 8;
            this.buttonDID.Text = "Device ID";
            this.buttonDID.UseVisualStyleBackColor = false;
            this.buttonDID.Click += new System.EventHandler(this.buttonDID_Click);
            // 
            // buttonDI
            // 
            this.buttonDI.BackColor = System.Drawing.Color.White;
            this.buttonDI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDI.ForeColor = System.Drawing.Color.Black;
            this.buttonDI.Location = new System.Drawing.Point(42, 18);
            this.buttonDI.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDI.Name = "buttonDI";
            this.buttonDI.Size = new System.Drawing.Size(118, 23);
            this.buttonDI.TabIndex = 7;
            this.buttonDI.Text = "Device Info";
            this.buttonDI.UseVisualStyleBackColor = false;
            this.buttonDI.Click += new System.EventHandler(this.buttonDI_Click);
            // 
            // BootloaderMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(430, 450);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "BootloaderMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bootloader Menu";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonLB2014;
        private System.Windows.Forms.Button buttonUB2014;
        private System.Windows.Forms.Button buttonLB2015;
        private System.Windows.Forms.Button buttonUB2015;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonVivoLock;
        private System.Windows.Forms.Button buttonVivoUnlock;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button buttonDI;
        private System.Windows.Forms.Button buttonDID;
    }
}