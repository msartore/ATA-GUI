using ATA_GUI.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class TaskManager : Form
    {
        private readonly List<string> arrayApks;

        public TaskManager(List<string> arrayApks)
        {
            this.arrayApks = arrayApks;

            InitializeComponent();
        }

        private void TaskManager_Load(object sender, EventArgs e)
        {
            loadTasks();
        }

        private void toolStripButtonReloadTasks_Click(object sender, EventArgs e)
        {
            loadTasks();
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonKillProcess.Enabled = false;
            noFilterDataGridView();
        }

        private void appsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonKillProcess.Enabled = true;

            textBoxTaskName.Text = "";

            foreach (DataGridViewRow row in dataGridViewTasks.Rows)
            {
                row.Visible = arrayApks.Any((it) =>
                {
                    return row.Cells[8].Value != null && ((string)row.Cells[8].Value).Contains(it.Trim());
                });
            }
        }

        private void loadTasks()
        {
            textBoxTaskName.Text = "";
            dataGridViewTasks.Rows.Clear();
            string[] tasks = ConsoleProcess.AdbProcess(MainForm.commandAssemblerF("shell ps")).Split('\n');

            for (int i = 1; i < tasks.Length; i++)
            {
                _ = dataGridViewTasks.Rows.Add(tasks[i].Split(' ').Where(it => it != string.Empty).ToArray());
            }

            toolStripLabelTotalTasks.Text = "Total: " + dataGridViewTasks.Rows.Count.ToString();
        }

        private void filterDataGridView(int coloumn, string filter)
        {
            foreach (DataGridViewRow row in dataGridViewTasks.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    row.Visible = cell.Value != null && cell.Value.ToString().Contains(filter);
                }
            }
        }

        private void textBoxTaskName_TextChanged(object sender, EventArgs e)
        {
            if (textBoxTaskName.Text == string.Empty)
            {
                noFilterDataGridView();
            }
            else
            {
                filterDataGridView(8, textBoxTaskName.Text);
            }
        }

        private void noFilterDataGridView()
        {
            textBoxTaskName.Text = "";
            foreach (DataGridViewRow row in dataGridViewTasks.Rows)
            {
                row.Visible = true;
            }
        }

        private void toolStripButtonKillProcess_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewTasks.SelectedRows)
            {
                _ = ConsoleProcess.AdbFastbootCommandR(MainForm.commandAssemblerF(" shell am force-stop " + row.Cells[8].Value).Replace("\r", ""), 0);
            }

            loadTasks();
        }
    }
}
