using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class TaskManager : Form
    {
        private int mode;

        public TaskManager()
        {
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

        private void toolStripButtonSearch_Click(object sender, EventArgs e)
        {
            bool found = false;
            int taskCounter = 0;

            List<string> tasks = MainForm.adbFastbootCommandR(new string[] { " -s " + MainForm.CurrentDeviceSelected + " shell ps" }, 0).Split('\n').ToList();
            richTextBoxTasks.Text = tasks[0];
            foreach (string task in tasks)
            {
                if (task.Contains(textBoxTaskName.Text))
                {
                    found = true;
                    taskCounter++;
                    richTextBoxTasks.Text += "\n" + task;
                }
            }
            if (!found)
            {
                MainForm.MessageShowBox(textBoxTaskName.Text + " not found!", 0);
                toolStripLabelTotalTasks.Text = "Active Tasks: 0";
            }
            else
            {
                toolStripLabelTotalTasks.Text = "Active Tasks: " + taskCounter.ToString();
            }
        }

        private void buttonKillProcess_Click(object sender, EventArgs e)
        {
            _ = MainForm.adbFastbootCommandR(new string[] { " -s " + MainForm.CurrentDeviceSelected + " shell am force-stop " + textBoxPackage.Text }, 0);
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = 0;
            loadTasks();
        }

        private void appsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = 1;
            loadTasks();
        }

        private void loadTasks()
        {
            string[] tasks = MainForm.adbFastbootCommandR(new string[] { " -s " + MainForm.CurrentDeviceSelected + " shell ps" }, 0).Split('\n');

            richTextBoxTasks.Clear();

            if (mode == 0)
            {
                toolStripLabelTotalTasks.Text = "Active Tasks: " + tasks.Length.ToString();
                richTextBoxTasks.Text = string.Join("\n", tasks);
            }
            else
            {
                int taskCounter = 0;
                bool found = false;

                richTextBoxTasks.Text = tasks[0];
                foreach (string app in MainForm.arrayApks)
                {
                    foreach (string task in tasks)
                    {
                        if (task.Contains(app))
                        {
                            found = true;
                            taskCounter++;
                            richTextBoxTasks.Text += task;
                        }
                    }
                }

                if (!found)
                {
                    MainForm.MessageShowBox(textBoxTaskName.Text + " not found!", 0);
                    toolStripLabelTotalTasks.Text = "Active Tasks: 0";
                }
                else
                {
                    toolStripLabelTotalTasks.Text = "Active Tasks: " + taskCounter.ToString();
                }
            }
        }
    }
}
