namespace ATA_GUI.Forms
{
    partial class ScrcpyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScrcpyForm));
            buttonRun = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            comboBoxVideoCodec = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            comboBoxAudioCodec = new System.Windows.Forms.ComboBox();
            checkBoxPOOC = new System.Windows.Forms.CheckBox();
            checkBoxST = new System.Windows.Forms.CheckBox();
            checkBoxTSOff = new System.Windows.Forms.CheckBox();
            checkBoxPOOS = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // buttonRun
            // 
            buttonRun.Image = Properties.Resources.circled_play_48;
            buttonRun.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            buttonRun.Location = new System.Drawing.Point(292, 18);
            buttonRun.Name = "buttonRun";
            buttonRun.Size = new System.Drawing.Size(74, 85);
            buttonRun.TabIndex = 1;
            buttonRun.Text = "Start";
            buttonRun.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            buttonRun.UseVisualStyleBackColor = true;
            buttonRun.Click += buttonRun_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 18);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(74, 15);
            label2.TabIndex = 4;
            label2.Text = "Video Codec";
            // 
            // comboBoxVideoCodec
            // 
            comboBoxVideoCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxVideoCodec.FlatStyle = System.Windows.Forms.FlatStyle.System;
            comboBoxVideoCodec.FormattingEnabled = true;
            comboBoxVideoCodec.Location = new System.Drawing.Point(12, 36);
            comboBoxVideoCodec.Name = "comboBoxVideoCodec";
            comboBoxVideoCodec.Size = new System.Drawing.Size(222, 23);
            comboBoxVideoCodec.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(12, 62);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(76, 15);
            label3.TabIndex = 6;
            label3.Text = "Audio Codec";
            // 
            // comboBoxAudioCodec
            // 
            comboBoxAudioCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxAudioCodec.FormattingEnabled = true;
            comboBoxAudioCodec.Location = new System.Drawing.Point(13, 80);
            comboBoxAudioCodec.Name = "comboBoxAudioCodec";
            comboBoxAudioCodec.Size = new System.Drawing.Size(221, 23);
            comboBoxAudioCodec.TabIndex = 7;
            // 
            // checkBoxPOOC
            // 
            checkBoxPOOC.AutoSize = true;
            checkBoxPOOC.Location = new System.Drawing.Point(138, 153);
            checkBoxPOOC.Name = "checkBoxPOOC";
            checkBoxPOOC.Size = new System.Drawing.Size(124, 19);
            checkBoxPOOC.TabIndex = 11;
            checkBoxPOOC.Text = "Power off on close";
            checkBoxPOOC.UseVisualStyleBackColor = true;
            // 
            // checkBoxST
            // 
            checkBoxST.AutoSize = true;
            checkBoxST.Location = new System.Drawing.Point(138, 128);
            checkBoxST.Name = "checkBoxST";
            checkBoxST.Size = new System.Drawing.Size(100, 19);
            checkBoxST.TabIndex = 12;
            checkBoxST.Text = "Show touches";
            checkBoxST.UseVisualStyleBackColor = true;
            // 
            // checkBoxTSOff
            // 
            checkBoxTSOff.AutoSize = true;
            checkBoxTSOff.Location = new System.Drawing.Point(13, 128);
            checkBoxTSOff.Name = "checkBoxTSOff";
            checkBoxTSOff.Size = new System.Drawing.Size(105, 19);
            checkBoxTSOff.TabIndex = 13;
            checkBoxTSOff.Text = "Turn screen off";
            checkBoxTSOff.UseVisualStyleBackColor = true;
            // 
            // checkBoxPOOS
            // 
            checkBoxPOOS.AutoSize = true;
            checkBoxPOOS.Checked = true;
            checkBoxPOOS.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxPOOS.Location = new System.Drawing.Point(13, 153);
            checkBoxPOOS.Name = "checkBoxPOOS";
            checkBoxPOOS.Size = new System.Drawing.Size(119, 19);
            checkBoxPOOS.TabIndex = 14;
            checkBoxPOOS.Text = "Power on on start";
            checkBoxPOOS.UseVisualStyleBackColor = true;
            // 
            // ScrcpyForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(378, 182);
            Controls.Add(checkBoxPOOS);
            Controls.Add(checkBoxTSOff);
            Controls.Add(checkBoxST);
            Controls.Add(checkBoxPOOC);
            Controls.Add(comboBoxAudioCodec);
            Controls.Add(label3);
            Controls.Add(comboBoxVideoCodec);
            Controls.Add(label2);
            Controls.Add(buttonRun);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "ScrcpyForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Scrcpy";
            Shown += ScrcpyForm_Shown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxVideoCodec;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxAudioCodec;
        private System.Windows.Forms.CheckBox checkBoxPOOC;
        private System.Windows.Forms.CheckBox checkBoxST;
        private System.Windows.Forms.CheckBox checkBoxTSOff;
        private System.Windows.Forms.CheckBox checkBoxPOOS;
    }
}