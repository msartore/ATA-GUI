
namespace ATA_GUI
{
    partial class BloatwareDetecter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BloatwareDetecter));
            this.trackBarBloatwareInt = new System.Windows.Forms.TrackBar();
            this.labelWaring = new System.Windows.Forms.Label();
            this.labelAppCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkedListBoxBloatwareList = new System.Windows.Forms.CheckedListBox();
            this.buttonDisable = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBloatwareInt)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarBloatwareInt
            // 
            this.trackBarBloatwareInt.LargeChange = 1;
            this.trackBarBloatwareInt.Location = new System.Drawing.Point(45, 101);
            this.trackBarBloatwareInt.Maximum = 2;
            this.trackBarBloatwareInt.Name = "trackBarBloatwareInt";
            this.trackBarBloatwareInt.Size = new System.Drawing.Size(302, 45);
            this.trackBarBloatwareInt.TabIndex = 0;
            this.trackBarBloatwareInt.Scroll += new System.EventHandler(this.trackBarBloatwareInt_Scroll);
            // 
            // labelWaring
            // 
            this.labelWaring.AutoSize = true;
            this.labelWaring.BackColor = System.Drawing.Color.Green;
            this.labelWaring.ForeColor = System.Drawing.Color.White;
            this.labelWaring.Location = new System.Drawing.Point(135, 36);
            this.labelWaring.Name = "labelWaring";
            this.labelWaring.Size = new System.Drawing.Size(127, 13);
            this.labelWaring.TabIndex = 1;
            this.labelWaring.Text = "Normal bloatware remove";
            this.labelWaring.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAppCount
            // 
            this.labelAppCount.AutoSize = true;
            this.labelAppCount.Location = new System.Drawing.Point(162, 57);
            this.labelAppCount.Name = "labelAppCount";
            this.labelAppCount.Size = new System.Drawing.Size(71, 13);
            this.labelAppCount.TabIndex = 2;
            this.labelAppCount.Text = "App Found: 0";
            this.labelAppCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Basic";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(180, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Medium";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(308, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Advance";
            // 
            // checkedListBoxBloatwareList
            // 
            this.checkedListBoxBloatwareList.BackColor = System.Drawing.Color.Black;
            this.checkedListBoxBloatwareList.ForeColor = System.Drawing.Color.White;
            this.checkedListBoxBloatwareList.FormattingEnabled = true;
            this.checkedListBoxBloatwareList.Location = new System.Drawing.Point(45, 173);
            this.checkedListBoxBloatwareList.Name = "checkedListBoxBloatwareList";
            this.checkedListBoxBloatwareList.Size = new System.Drawing.Size(302, 79);
            this.checkedListBoxBloatwareList.TabIndex = 6;
            // 
            // buttonDisable
            // 
            this.buttonDisable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDisable.Location = new System.Drawing.Point(165, 262);
            this.buttonDisable.Name = "buttonDisable";
            this.buttonDisable.Size = new System.Drawing.Size(75, 23);
            this.buttonDisable.TabIndex = 7;
            this.buttonDisable.Text = "Disable";
            this.buttonDisable.UseVisualStyleBackColor = true;
            this.buttonDisable.Click += new System.EventHandler(this.buttonDisable_Click);
            // 
            // BloatwareDetecter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(394, 297);
            this.Controls.Add(this.buttonDisable);
            this.Controls.Add(this.checkedListBoxBloatwareList);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelAppCount);
            this.Controls.Add(this.labelWaring);
            this.Controls.Add(this.trackBarBloatwareInt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BloatwareDetecter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bloatware Detecter";
            this.Shown += new System.EventHandler(this.BloatwareDetecter_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBloatwareInt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBarBloatwareInt;
        private System.Windows.Forms.Label labelWaring;
        private System.Windows.Forms.Label labelAppCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckedListBox checkedListBoxBloatwareList;
        private System.Windows.Forms.Button buttonDisable;
    }
}