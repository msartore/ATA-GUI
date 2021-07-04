using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class TaskManager : Form
    {
        private int mode = 0;

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

            List<string> tasks = MainForm.adbFastbootCommandR(new string[] { " -s " + MainForm.currentDeviceSelected + " shell ps" }, 0).Split('\n').ToList();
            richTextBoxTasks.Text = tasks[0];
            foreach (string task in tasks)
            {
                if(task.Contains(textBoxTaskName.Text))
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
            MainForm.adbFastbootCommandR(new string[] { " -s " + MainForm.currentDeviceSelected + " shell am force-stop " + textBoxPackage.Text }, 0);
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
            List<string> tasks = MainForm.adbFastbootCommandR(new string[] { " -s " + MainForm.currentDeviceSelected + " shell ps" }, 0).Split('\n').ToList();

            richTextBoxTasks.Clear();
            switch (mode)
            {
                case 0:
                    foreach (string task in tasks)
                    {
                        richTextBoxTasks.Text += task;
                    }
                    toolStripLabelTotalTasks.Text = "Active Tasks: " + tasks.Count.ToString();
                    buttonKillProcess.Enabled = false;  
                    break;
                case 1:
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
                    buttonKillProcess.Enabled = true;
                    break;
            }
        }
    }
}
