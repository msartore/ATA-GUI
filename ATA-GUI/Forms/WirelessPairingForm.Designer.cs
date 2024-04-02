namespace ATA_GUI.Forms
{
    partial class WirelessPairingForm
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
            buttonPair = new System.Windows.Forms.Button();
            textBoxCode = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // buttonPair
            // 
            buttonPair.Location = new System.Drawing.Point(206, 123);
            buttonPair.Name = "buttonPair";
            buttonPair.Size = new System.Drawing.Size(75, 23);
            buttonPair.TabIndex = 1;
            buttonPair.Text = "Pair";
            buttonPair.UseVisualStyleBackColor = true;
            buttonPair.Click += buttonPair_Click;
            // 
            // textBoxCode
            // 
            textBoxCode.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            textBoxCode.Location = new System.Drawing.Point(81, 61);
            textBoxCode.Name = "textBoxCode";
            textBoxCode.Size = new System.Drawing.Size(131, 33);
            textBoxCode.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(81, 43);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(66, 15);
            label1.TabIndex = 4;
            label1.Text = "Enter code:";
            // 
            // WirelessPairingForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(291, 157);
            Controls.Add(label1);
            Controls.Add(textBoxCode);
            Controls.Add(buttonPair);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WirelessPairingForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Wireless Pairing";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button buttonPair;
        private System.Windows.Forms.TextBox textBoxCode;
        private System.Windows.Forms.Label label1;
    }
}