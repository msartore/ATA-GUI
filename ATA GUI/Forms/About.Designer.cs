
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
            this.label2 = new System.Windows.Forms.Label();
            this.labelLicense = new System.Windows.Forms.Label();
            this.pictureRepo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureRepo)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(39, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(400, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Copyright (C) 2021 Massimiliano Sartore";
            // 
            // labelLicense
            // 
            this.labelLicense.AutoSize = true;
            this.labelLicense.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLicense.Location = new System.Drawing.Point(311, 110);
            this.labelLicense.Name = "labelLicense";
            this.labelLicense.Size = new System.Drawing.Size(298, 37);
            this.labelLicense.TabIndex = 4;
            this.labelLicense.Text = "Apache License 2.0";
            this.labelLicense.Click += new System.EventHandler(this.labelLicense_Click);
            // 
            // pictureRepo
            // 
            this.pictureRepo.Image = ((System.Drawing.Image)(resources.GetObject("pictureRepo.Image")));
            this.pictureRepo.Location = new System.Drawing.Point(44, 98);
            this.pictureRepo.Name = "pictureRepo";
            this.pictureRepo.Size = new System.Drawing.Size(212, 60);
            this.pictureRepo.TabIndex = 3;
            this.pictureRepo.TabStop = false;
            this.pictureRepo.Click += new System.EventHandler(this.pictureRepo_Click);
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(647, 182);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelLicense);
            this.Controls.Add(this.pictureRepo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.pictureRepo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelLicense;
        private System.Windows.Forms.PictureBox pictureRepo;
    }
}