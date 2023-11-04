
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
            buttonKillProcess = new System.Windows.Forms.Button();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripButtonReloadTasks = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            textBoxTaskName = new System.Windows.Forms.ToolStripTextBox();
            toolStripButtonKillProcess = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            appsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripLabelTotalTasks = new System.Windows.Forms.ToolStripLabel();
            dataGridViewTasks = new System.Windows.Forms.DataGridView();
            user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            pid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ppid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            vsz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            rss = new System.Windows.Forms.DataGridViewTextBoxColumn();
            wchan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            addr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            s = new System.Windows.Forms.DataGridViewTextBoxColumn();
            name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTasks).BeginInit();
            SuspendLayout();
            // 
            // buttonKillProcess
            // 
            buttonKillProcess.Location = new System.Drawing.Point(0, 0);
            buttonKillProcess.Name = "buttonKillProcess";
            buttonKillProcess.Size = new System.Drawing.Size(75, 23);
            buttonKillProcess.TabIndex = 51;
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = System.Drawing.Color.White;
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonReloadTasks, toolStripSeparator3, textBoxTaskName, toolStripButtonKillProcess, toolStripSeparator6, toolStripDropDownButton1, toolStripSeparator1, toolStripLabelTotalTasks });
            toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            toolStrip1.Size = new System.Drawing.Size(986, 23);
            toolStrip1.TabIndex = 47;
            toolStrip1.Text = "toolStripAPKMenu";
            // 
            // toolStripButtonReloadTasks
            // 
            toolStripButtonReloadTasks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonReloadTasks.Image = Properties.Resources.icons8_refresh_48;
            toolStripButtonReloadTasks.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonReloadTasks.Name = "toolStripButtonReloadTasks";
            toolStripButtonReloadTasks.Size = new System.Drawing.Size(23, 20);
            toolStripButtonReloadTasks.Text = "Reload Apps";
            toolStripButtonReloadTasks.Click += toolStripButtonReloadTasks_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
            // 
            // textBoxTaskName
            // 
            textBoxTaskName.BackColor = System.Drawing.Color.White;
            textBoxTaskName.Name = "textBoxTaskName";
            textBoxTaskName.Size = new System.Drawing.Size(174, 23);
            textBoxTaskName.TextChanged += textBoxTaskName_TextChanged;
            // 
            // toolStripButtonKillProcess
            // 
            toolStripButtonKillProcess.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonKillProcess.Enabled = false;
            toolStripButtonKillProcess.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonKillProcess.Image");
            toolStripButtonKillProcess.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonKillProcess.Name = "toolStripButtonKillProcess";
            toolStripButtonKillProcess.Size = new System.Drawing.Size(23, 20);
            toolStripButtonKillProcess.Text = "toolStripButton1";
            toolStripButtonKillProcess.ToolTipText = "Kill process";
            toolStripButtonKillProcess.Click += toolStripButtonKillProcess_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { allToolStripMenuItem, appsToolStripMenuItem });
            toolStripDropDownButton1.Image = Properties.Resources.icons8_filter_48;
            toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new System.Drawing.Size(29, 20);
            toolStripDropDownButton1.Text = "Filter";
            // 
            // allToolStripMenuItem
            // 
            allToolStripMenuItem.Name = "allToolStripMenuItem";
            allToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            allToolStripMenuItem.Text = "All";
            allToolStripMenuItem.Click += allToolStripMenuItem_Click;
            // 
            // appsToolStripMenuItem
            // 
            appsToolStripMenuItem.Name = "appsToolStripMenuItem";
            appsToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            appsToolStripMenuItem.Text = "Apps";
            appsToolStripMenuItem.Click += appsToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripLabelTotalTasks
            // 
            toolStripLabelTotalTasks.Name = "toolStripLabelTotalTasks";
            toolStripLabelTotalTasks.Size = new System.Drawing.Size(44, 15);
            toolStripLabelTotalTasks.Text = "Total: 0";
            // 
            // dataGridViewTasks
            // 
            dataGridViewTasks.AllowUserToAddRows = false;
            dataGridViewTasks.AllowUserToDeleteRows = false;
            dataGridViewTasks.AllowUserToOrderColumns = true;
            dataGridViewTasks.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dataGridViewTasks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewTasks.BackgroundColor = System.Drawing.Color.White;
            dataGridViewTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { user, pid, ppid, vsz, rss, wchan, addr, s, name });
            dataGridViewTasks.Location = new System.Drawing.Point(12, 24);
            dataGridViewTasks.Name = "dataGridViewTasks";
            dataGridViewTasks.ReadOnly = true;
            dataGridViewTasks.RowTemplate.Height = 25;
            dataGridViewTasks.Size = new System.Drawing.Size(962, 326);
            dataGridViewTasks.TabIndex = 50;
            // 
            // user
            // 
            user.HeaderText = "USER";
            user.Name = "user";
            user.ReadOnly = true;
            // 
            // pid
            // 
            pid.HeaderText = "PID";
            pid.Name = "pid";
            pid.ReadOnly = true;
            // 
            // ppid
            // 
            ppid.HeaderText = "PPID";
            ppid.Name = "ppid";
            ppid.ReadOnly = true;
            // 
            // vsz
            // 
            vsz.HeaderText = "VSZ";
            vsz.Name = "vsz";
            vsz.ReadOnly = true;
            // 
            // rss
            // 
            rss.HeaderText = "RSS";
            rss.Name = "rss";
            rss.ReadOnly = true;
            // 
            // wchan
            // 
            wchan.HeaderText = "WCHAN";
            wchan.Name = "wchan";
            wchan.ReadOnly = true;
            // 
            // addr
            // 
            addr.HeaderText = "ADDR";
            addr.Name = "addr";
            addr.ReadOnly = true;
            // 
            // s
            // 
            s.HeaderText = "S";
            s.Name = "s";
            s.ReadOnly = true;
            // 
            // name
            // 
            name.HeaderText = "NAME";
            name.Name = "name";
            name.ReadOnly = true;
            // 
            // TaskManager
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(986, 362);
            Controls.Add(dataGridViewTasks);
            Controls.Add(toolStrip1);
            Controls.Add(buttonKillProcess);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimizeBox = false;
            Name = "TaskManager";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "TaskManager";
            Load += TaskManager_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTasks).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button buttonKillProcess;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonReloadTasks;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripTextBox textBoxTaskName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelTotalTasks;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem appsToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridViewTasks;
        private System.Windows.Forms.DataGridViewTextBoxColumn user;
        private System.Windows.Forms.DataGridViewTextBoxColumn pid;
        private System.Windows.Forms.DataGridViewTextBoxColumn ppid;
        private System.Windows.Forms.DataGridViewTextBoxColumn vsz;
        private System.Windows.Forms.DataGridViewTextBoxColumn rss;
        private System.Windows.Forms.DataGridViewTextBoxColumn wchan;
        private System.Windows.Forms.DataGridViewTextBoxColumn addr;
        private System.Windows.Forms.DataGridViewTextBoxColumn s;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonKillProcess;
    }
}