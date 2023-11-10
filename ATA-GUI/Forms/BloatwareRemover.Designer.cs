
namespace ATA_GUI
{
    partial class BloatwareRemover
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BloatwareRemover));
            buttonAction = new System.Windows.Forms.Button();
            checkedListBoxBloatwareList = new System.Windows.Forms.CheckedListBox();
            label1 = new System.Windows.Forms.Label();
            checkBoxSelectAll = new System.Windows.Forms.CheckBox();
            comboBoxActionMode = new System.Windows.Forms.ComboBox();
            label2 = new System.Windows.Forms.Label();
            comboBoxScanLevel = new System.Windows.Forms.ComboBox();
            labelBC = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // buttonAction
            // 
            buttonAction.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            buttonAction.Location = new System.Drawing.Point(268, 235);
            buttonAction.Name = "buttonAction";
            buttonAction.Size = new System.Drawing.Size(140, 73);
            buttonAction.TabIndex = 0;
            buttonAction.Text = "Run";
            buttonAction.UseVisualStyleBackColor = true;
            buttonAction.Click += buttonAction_Click;
            // 
            // checkedListBoxBloatwareList
            // 
            checkedListBoxBloatwareList.FormattingEnabled = true;
            checkedListBoxBloatwareList.Location = new System.Drawing.Point(47, 38);
            checkedListBoxBloatwareList.Name = "checkedListBoxBloatwareList";
            checkedListBoxBloatwareList.Size = new System.Drawing.Size(361, 184);
            checkedListBoxBloatwareList.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(47, 223);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(78, 15);
            label1.TabIndex = 5;
            label1.Text = "Level of scan:";
            // 
            // checkBoxSelectAll
            // 
            checkBoxSelectAll.AutoSize = true;
            checkBoxSelectAll.Location = new System.Drawing.Point(47, 12);
            checkBoxSelectAll.Name = "checkBoxSelectAll";
            checkBoxSelectAll.Size = new System.Drawing.Size(72, 19);
            checkBoxSelectAll.TabIndex = 6;
            checkBoxSelectAll.Text = "Select all";
            checkBoxSelectAll.UseVisualStyleBackColor = true;
            checkBoxSelectAll.CheckedChanged += checkBoxSelectAll_CheckedChanged;
            // 
            // comboBoxActionMode
            // 
            comboBoxActionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxActionMode.FormattingEnabled = true;
            comboBoxActionMode.Items.AddRange(new object[] { "disable", "disable or remove" });
            comboBoxActionMode.Location = new System.Drawing.Point(47, 285);
            comboBoxActionMode.Name = "comboBoxActionMode";
            comboBoxActionMode.Size = new System.Drawing.Size(140, 23);
            comboBoxActionMode.TabIndex = 7;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(47, 267);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(79, 15);
            label2.TabIndex = 8;
            label2.Text = "Action mode:";
            // 
            // comboBoxScanLevel
            // 
            comboBoxScanLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxScanLevel.FormattingEnabled = true;
            comboBoxScanLevel.Items.AddRange(new object[] { "basic", "medium", "advance" });
            comboBoxScanLevel.Location = new System.Drawing.Point(47, 241);
            comboBoxScanLevel.Name = "comboBoxScanLevel";
            comboBoxScanLevel.Size = new System.Drawing.Size(140, 23);
            comboBoxScanLevel.TabIndex = 9;
            comboBoxScanLevel.SelectedIndexChanged += comboBoxScanLevel_SelectedIndexChanged;
            // 
            // labelBC
            // 
            labelBC.AutoSize = true;
            labelBC.Location = new System.Drawing.Point(285, 13);
            labelBC.Name = "labelBC";
            labelBC.Size = new System.Drawing.Size(69, 15);
            labelBC.TabIndex = 10;
            labelBC.Text = "UNKNOWN";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label3.Location = new System.Drawing.Point(22, 320);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(417, 11);
            label3.TabIndex = 11;
            label3.Text = "* If the option \"disable or remove\" is chosen, system apps will be disabled, and non-system apps will be uninstalled.";
            // 
            // BloatwareRemover
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(460, 343);
            Controls.Add(label3);
            Controls.Add(labelBC);
            Controls.Add(comboBoxScanLevel);
            Controls.Add(label2);
            Controls.Add(comboBoxActionMode);
            Controls.Add(checkBoxSelectAll);
            Controls.Add(label1);
            Controls.Add(checkedListBoxBloatwareList);
            Controls.Add(buttonAction);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BloatwareRemover";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Bloatware Remover";
            Shown += BloatwareRemover_Shown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button buttonAction;
        private System.Windows.Forms.CheckedListBox checkedListBoxBloatwareList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxSelectAll;
        private System.Windows.Forms.ComboBox comboBoxActionMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxScanLevel;
        private System.Windows.Forms.Label labelBC;
        private System.Windows.Forms.Label label3;
    }
}