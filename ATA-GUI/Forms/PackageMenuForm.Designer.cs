
namespace ATA_GUI
{
    partial class PackageMenuForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageMenuForm));
            richTextBoxAPKList = new System.Windows.Forms.RichTextBox();
            label1 = new System.Windows.Forms.Label();
            buttonEnable = new System.Windows.Forms.Button();
            buttonCancel = new System.Windows.Forms.Button();
            buttonDisable = new System.Windows.Forms.Button();
            buttonClearData = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // richTextBoxAPKList
            // 
            richTextBoxAPKList.BackColor = System.Drawing.Color.Black;
            richTextBoxAPKList.ForeColor = System.Drawing.Color.White;
            richTextBoxAPKList.Location = new System.Drawing.Point(75, 24);
            richTextBoxAPKList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            richTextBoxAPKList.Name = "richTextBoxAPKList";
            richTextBoxAPKList.Size = new System.Drawing.Size(448, 87);
            richTextBoxAPKList.TabIndex = 0;
            richTextBoxAPKList.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(14, 24);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(50, 15);
            label1.TabIndex = 1;
            label1.Text = "APK list:";
            // 
            // buttonEnable
            // 
            buttonEnable.FlatStyle = System.Windows.Forms.FlatStyle.System;
            buttonEnable.Location = new System.Drawing.Point(330, 133);
            buttonEnable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonEnable.Name = "buttonEnable";
            buttonEnable.Size = new System.Drawing.Size(88, 27);
            buttonEnable.TabIndex = 2;
            buttonEnable.Text = "Enable";
            buttonEnable.UseVisualStyleBackColor = true;
            buttonEnable.Click += ButtonEnable_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            buttonCancel.Location = new System.Drawing.Point(436, 133);
            buttonCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(88, 27);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // buttonDisable
            // 
            buttonDisable.FlatStyle = System.Windows.Forms.FlatStyle.System;
            buttonDisable.Location = new System.Drawing.Point(236, 133);
            buttonDisable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonDisable.Name = "buttonDisable";
            buttonDisable.Size = new System.Drawing.Size(88, 27);
            buttonDisable.TabIndex = 4;
            buttonDisable.Text = "Disable";
            buttonDisable.UseVisualStyleBackColor = true;
            buttonDisable.Click += ButtonDisable_Click;
            // 
            // buttonClearData
            // 
            buttonClearData.FlatStyle = System.Windows.Forms.FlatStyle.System;
            buttonClearData.Location = new System.Drawing.Point(141, 133);
            buttonClearData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonClearData.Name = "buttonClearData";
            buttonClearData.Size = new System.Drawing.Size(88, 27);
            buttonClearData.TabIndex = 5;
            buttonClearData.Text = "Clear Data";
            buttonClearData.UseVisualStyleBackColor = true;
            buttonClearData.Click += ButtonClearData_Click;
            // 
            // PackageMenuForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(548, 167);
            Controls.Add(buttonClearData);
            Controls.Add(buttonDisable);
            Controls.Add(buttonCancel);
            Controls.Add(buttonEnable);
            Controls.Add(label1);
            Controls.Add(richTextBoxAPKList);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PackageMenuForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Package Menu";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxAPKList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonEnable;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonDisable;
        private System.Windows.Forms.Button buttonClearData;
    }
}