
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
            this.richTextBoxAPKList = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonEnable = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDisable = new System.Windows.Forms.Button();
            this.buttonClearData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBoxAPKList
            // 
            this.richTextBoxAPKList.BackColor = System.Drawing.Color.Black;
            this.richTextBoxAPKList.ForeColor = System.Drawing.Color.White;
            this.richTextBoxAPKList.Location = new System.Drawing.Point(64, 21);
            this.richTextBoxAPKList.Name = "richTextBoxAPKList";
            this.richTextBoxAPKList.Size = new System.Drawing.Size(385, 76);
            this.richTextBoxAPKList.TabIndex = 0;
            this.richTextBoxAPKList.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "APK list:";
            // 
            // buttonEnable
            // 
            this.buttonEnable.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonEnable.Location = new System.Drawing.Point(283, 115);
            this.buttonEnable.Name = "buttonEnable";
            this.buttonEnable.Size = new System.Drawing.Size(75, 23);
            this.buttonEnable.TabIndex = 2;
            this.buttonEnable.Text = "Enable";
            this.buttonEnable.UseVisualStyleBackColor = true;
            this.buttonEnable.Click += new System.EventHandler(this.ButtonEnable_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCancel.Location = new System.Drawing.Point(374, 115);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // buttonDisable
            // 
            this.buttonDisable.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonDisable.Location = new System.Drawing.Point(202, 115);
            this.buttonDisable.Name = "buttonDisable";
            this.buttonDisable.Size = new System.Drawing.Size(75, 23);
            this.buttonDisable.TabIndex = 4;
            this.buttonDisable.Text = "Disable";
            this.buttonDisable.UseVisualStyleBackColor = true;
            this.buttonDisable.Click += new System.EventHandler(this.ButtonDisable_Click);
            // 
            // buttonClearData
            // 
            this.buttonClearData.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonClearData.Location = new System.Drawing.Point(121, 115);
            this.buttonClearData.Name = "buttonClearData";
            this.buttonClearData.Size = new System.Drawing.Size(75, 23);
            this.buttonClearData.TabIndex = 5;
            this.buttonClearData.Text = "Clear Data";
            this.buttonClearData.UseVisualStyleBackColor = true;
            this.buttonClearData.Click += new System.EventHandler(this.ButtonClearData_Click);
            // 
            // PackageMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(470, 145);
            this.Controls.Add(this.buttonClearData);
            this.Controls.Add(this.buttonDisable);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonEnable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBoxAPKList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "PackageMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Package Menu";
            this.ResumeLayout(false);
            this.PerformLayout();

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