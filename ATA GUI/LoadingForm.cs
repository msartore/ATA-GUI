using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;


namespace ATA_GUI
{
    public partial class LoadingForm : Form
    {
        private List<string> arrayApk;
        private string command;

        public LoadingForm(List<string> arrayApkTemp, string commandTemp)
        {
            InitializeComponent();
            arrayApk = arrayApkTemp;
            command = commandTemp;
        }
        private void LoadingForm_Shown(Object sender, EventArgs e)
        {
            if (!backgroundWorkerUninstaller.IsBusy)
            {
                backgroundWorkerUninstaller.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("errore 43");
            }
        }


        private void backgroundWorkerUninstaller_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value = progressBar1.Maximum;
            progressBar1.Refresh();
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void backgroundWorkerUninstaller_DoWork(object sender, DoWorkEventArgs e)
        {
            Invoke((Action)delegate
            {
                progressBar1.Maximum = arrayApk.Count;
                foreach (string apk in arrayApk)
                {
                    labelApk.Text = apk;
                    MainForm.systemCommand(command + apk);
                    progressBar1.Value += 1;
                    progressBar1.Refresh();
                    Thread.Sleep(500);
                }
            });
        }
    }
}
