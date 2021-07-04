
namespace ATA_GUI
{
    partial class TaskManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskManager));
            this.richTextBoxTasks = new System.Windows.Forms.RichTextBox();
            this.textBoxPackage = new System.Windows.Forms.TextBox();
            this.buttonKillProcess = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonReloadTasks = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.textBoxTaskName = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButtonSearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelTotalTasks = new System.Windows.Forms.ToolStripLabel();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxTasks
            // 
            this.richTextBoxTasks.Location = new System.Drawing.Point(0, 21);
            this.richTextBoxTasks.Name = "richTextBoxTasks";
            this.richTextBoxTasks.ReadOnly = true;
            this.richTextBoxTasks.Size = new System.Drawing.Size(570, 287);
            this.richTextBoxTasks.TabIndex = 44;
            this.richTextBoxTasks.Text = "";
            // 
            // textBoxPackage
            // 
            this.textBoxPackage.Location = new System.Drawing.Point(99, 314);
            this.textBoxPackage.Name = "textBoxPackage";
            this.textBoxPackage.Size = new System.Drawing.Size(100, 20);
            this.textBoxPackage.TabIndex = 45;
            // 
            // buttonKillProcess
            // 
            this.buttonKillProcess.BackColor = System.Drawing.Color.White;
            this.buttonKillProcess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonKillProcess.ForeColor = System.Drawing.Color.Black;
            this.buttonKillProcess.Location = new System.Drawing.Point(208, 313);
            this.buttonKillProcess.Name = "buttonKillProcess";
            this.buttonKillProcess.Size = new System.Drawing.Size(73, 23);
            this.buttonKillProcess.TabIndex = 46;
            this.buttonKillProcess.Text = "Kill Process";
            this.buttonKillProcess.UseVisualStyleBackColor = false;
            this.buttonKillProcess.Click += new System.EventHandler(this.buttonKillProcess_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonReloadTasks,
            this.toolStripSeparator2,
            this.toolStripDropDownButton1,
            this.toolStripSeparator6,
            this.textBoxTaskName,
            this.toolStripButtonSearch,
            this.toolStripSeparator1,
            this.toolStripLabelTotalTasks});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(568, 23);
            this.toolStrip1.TabIndex = 47;
            this.toolStrip1.Text = "toolStripAPKMenu";
            // 
            // toolStripButtonReloadTasks
            // 
            this.toolStripButtonReloadTasks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReloadTasks.Image = global::ATA_GUI.Properties.Resources.icons8_refresh_48;
            this.toolStripButtonReloadTasks.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReloadTasks.Name = "toolStripButtonReloadTasks";
            this.toolStripButtonReloadTasks.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonReloadTasks.Text = "Reload Apps";
            this.toolStripButtonReloadTasks.Click += new System.EventHandler(this.toolStripButtonReloadTasks_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 23);
            // 
            // textBoxTaskName
            // 
            this.textBoxTaskName.BackColor = System.Drawing.Color.White;
            this.textBoxTaskName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxTaskName.Name = "textBoxTaskName";
            this.textBoxTaskName.Size = new System.Drawing.Size(150, 23);
            this.textBoxTaskName.Text = "Search";
            // 
            // toolStripButtonSearch
            // 
            this.toolStripButtonSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSearch.Image = global::ATA_GUI.Properties.Resources.icons8_search_48;
            this.toolStripButtonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSearch.Name = "toolStripButtonSearch";
            this.toolStripButtonSearch.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonSearch.Text = "toolStripButton1";
            this.toolStripButtonSearch.Click += new System.EventHandler(this.toolStripButtonSearch_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripLabelTotalTasks
            // 
            this.toolStripLabelTotalTasks.Name = "toolStripLabelTotalTasks";
            this.toolStripLabelTotalTasks.Size = new System.Drawing.Size(44, 15);
            this.toolStripLabelTotalTasks.Text = "Total: 0";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allToolStripMenuItem,
            this.appsToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::ATA_GUI.Properties.Resources.icons8_filter_48;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 20);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButtonFilter";
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.allToolStripMenuItem.Text = "All";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
            // 
            // appsToolStripMenuItem
            // 
            this.appsToolStripMenuItem.Name = "appsToolStripMenuItem";
            this.appsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.appsToolStripMenuItem.Text = "Apps";
            this.appsToolStripMenuItem.Click += new System.EventHandler(this.appsToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 316);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 48;
            this.label1.Text = "Package name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(136, 339);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 13);
            this.label2.TabIndex = 49;
            this.label2.Text = "Pay attention on what you kill";
            // 
            // TaskManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(568, 358);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.buttonKillProcess);
            this.Controls.Add(this.textBoxPackage);
            this.Controls.Add(this.richTextBoxTasks);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TaskManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TaskManager";
            this.Load += new System.EventHandler(this.TaskManager_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox richTextBoxTasks;
        private System.Windows.Forms.TextBox textBoxPackage;
        private System.Windows.Forms.Button buttonKillProcess;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonReloadTasks;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripTextBox textBoxTaskName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelTotalTasks;
        private System.Windows.Forms.ToolStripButton toolStripButtonSearch;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem appsToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}