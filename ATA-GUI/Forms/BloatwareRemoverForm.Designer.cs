
namespace ATA_GUI
{
    partial class BloatwareRemoverForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BloatwareRemoverForm));
            buttonAction = new System.Windows.Forms.Button();
            dataGridViewBloatwareList = new System.Windows.Forms.DataGridView();
            label1 = new System.Windows.Forms.Label();
            checkBoxSelectAll = new System.Windows.Forms.CheckBox();
            comboBoxActionMode = new System.Windows.Forms.ComboBox();
            label2 = new System.Windows.Forms.Label();
            comboBoxScanLevel = new System.Windows.Forms.ComboBox();
            labelBC = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            packageColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            descriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridViewBloatwareList).BeginInit();
            SuspendLayout();
            //
            // buttonAction
            //
            buttonAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            buttonAction.Font = new System.Drawing.Font("Segoe UI", 18F);
            buttonAction.Location = new System.Drawing.Point(620, 500);
            buttonAction.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonAction.Name = "buttonAction";
            buttonAction.Size = new System.Drawing.Size(140, 60);
            buttonAction.TabIndex = 0;
            buttonAction.Text = "Run";
            buttonAction.UseVisualStyleBackColor = true;
            buttonAction.Click += buttonAction_Click;
            //
            // dataGridViewBloatwareList
            //
            dataGridViewBloatwareList.AllowUserToAddRows = false;
            dataGridViewBloatwareList.AllowUserToDeleteRows = false;
            dataGridViewBloatwareList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewBloatwareList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewBloatwareList.BackgroundColor = System.Drawing.Color.White;
            dataGridViewBloatwareList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewBloatwareList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewBloatwareList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewBloatwareList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewBloatwareList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            packageColumn,
            descriptionColumn});
            dataGridViewBloatwareList.GridColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewBloatwareList.Location = new System.Drawing.Point(20, 35);
            dataGridViewBloatwareList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dataGridViewBloatwareList.Name = "dataGridViewBloatwareList";
            dataGridViewBloatwareList.ReadOnly = true;
            dataGridViewBloatwareList.RowHeadersVisible = false;
            dataGridViewBloatwareList.RowHeadersWidth = 62;
            dataGridViewBloatwareList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            dataGridViewBloatwareList.Size = new System.Drawing.Size(760, 400);
            dataGridViewBloatwareList.TabIndex = 1;
            dataGridViewBloatwareList.VirtualMode = false;
            dataGridViewBloatwareList.SelectionChanged += dataGridViewBloatwareList_SelectionChanged;
            //
            // packageColumn
            //
            packageColumn.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            packageColumn.HeaderText = "Package";
            packageColumn.Name = "packageColumn";
            packageColumn.ReadOnly = true;
            packageColumn.DividerWidth = 1;
            //
            // descriptionColumn
            //
            descriptionColumn.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            descriptionColumn.HeaderText = "Description";
            descriptionColumn.Name = "descriptionColumn";
            descriptionColumn.ReadOnly = true;
            descriptionColumn.DividerWidth = 1;
            //
            // label1
            //
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(20, 460);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(85, 20);
            label1.TabIndex = 5;
            label1.Text = "Level of scan:";
            //
            // checkBoxSelectAll
            //
            checkBoxSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            checkBoxSelectAll.AutoSize = true;
            checkBoxSelectAll.Location = new System.Drawing.Point(20, 10);
            checkBoxSelectAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxSelectAll.Name = "checkBoxSelectAll";
            checkBoxSelectAll.Size = new System.Drawing.Size(200, 24);
            checkBoxSelectAll.TabIndex = 6;
            checkBoxSelectAll.Text = "Select all (Not suggested)";
            checkBoxSelectAll.UseVisualStyleBackColor = true;
            checkBoxSelectAll.CheckedChanged += checkBoxSelectAll_CheckedChanged;
            //
            // comboBoxActionMode
            //
            comboBoxActionMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            comboBoxActionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxActionMode.FormattingEnabled = true;
            comboBoxActionMode.Items.AddRange(new object[] { "disable or remove", "remove" });
            comboBoxActionMode.Location = new System.Drawing.Point(20, 530);
            comboBoxActionMode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxActionMode.Name = "comboBoxActionMode";
            comboBoxActionMode.Size = new System.Drawing.Size(200, 28);
            comboBoxActionMode.TabIndex = 7;
            //
            // label2
            //
            label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(20, 505);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(96, 20);
            label2.TabIndex = 8;
            label2.Text = "Action mode:";
            //
            // comboBoxScanLevel
            //
            comboBoxScanLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            comboBoxScanLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxScanLevel.FormattingEnabled = true;
            comboBoxScanLevel.Items.AddRange(new object[] { "basic", "medium", "advance", "expert" });
            comboBoxScanLevel.Location = new System.Drawing.Point(20, 480);
            comboBoxScanLevel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxScanLevel.Name = "comboBoxScanLevel";
            comboBoxScanLevel.Size = new System.Drawing.Size(200, 28);
            comboBoxScanLevel.TabIndex = 9;
            comboBoxScanLevel.SelectedIndexChanged += comboBoxScanLevel_SelectedIndexChanged;
            //
            // labelBC
            //
            labelBC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            labelBC.AutoSize = true;
            labelBC.Location = new System.Drawing.Point(600, 10);
            labelBC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelBC.Name = "labelBC";
            labelBC.Size = new System.Drawing.Size(180, 20);
            labelBC.TabIndex = 10;
            labelBC.Text = "Bloatware found: 0";
            //
            // label3
            //
            label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Segoe UI", 8F);
            label3.ForeColor = System.Drawing.Color.DarkRed;
            label3.Location = new System.Drawing.Point(20, 570);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(550, 21);
            label3.TabIndex = 11;
            label3.Text = "* If the option \"disable or remove\" is chosen, system apps will be disabled, and non-system apps will be uninstalled.";
            //
            // BloatwareRemoverForm
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(800, 600);
            Controls.Add(label3);
            Controls.Add(labelBC);
            Controls.Add(comboBoxScanLevel);
            Controls.Add(label2);
            Controls.Add(comboBoxActionMode);
            Controls.Add(checkBoxSelectAll);
            Controls.Add(label1);
            Controls.Add(dataGridViewBloatwareList);
            Controls.Add(buttonAction);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = true;
            MinimizeBox = true;
            MinimumSize = new System.Drawing.Size(600, 400);
            Name = "BloatwareRemoverForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Bloatware Remover";
            Shown += BloatwareRemover_Shown;
            ((System.ComponentModel.ISupportInitialize)dataGridViewBloatwareList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button buttonAction;
        private System.Windows.Forms.DataGridView dataGridViewBloatwareList;
        private System.Windows.Forms.DataGridViewTextBoxColumn packageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionColumn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxSelectAll;
        private System.Windows.Forms.ComboBox comboBoxActionMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxScanLevel;
        private System.Windows.Forms.Label labelBC;
        private System.Windows.Forms.Label label3;
    }
}