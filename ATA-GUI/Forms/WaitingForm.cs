using System.Threading;
using System.Windows.Forms;

namespace ATA_GUI.Forms
{
    public partial class WaitingForm : Form
    {
        public WaitingForm()
        {
            InitializeComponent();
        }



        private void backgroundWorkerMessages_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int counter = 0;

            while (!backgroundWorkerMessages.CancellationPending)
            {
                backgroundWorkerMessages.ReportProgress(counter++);
                Thread.Sleep(1000);
            }
        }

        private void backgroundWorkerMessages_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            int r = e.ProgressPercentage % 3;

            labelLoading.Text = r == 0 ? "Extracting data..." : r == 1 ? "Restoring settings..." : "Checking for updates...";
        }

        private void WaitingForm_Load(object sender, System.EventArgs e)
        {
            backgroundWorkerMessages.RunWorkerAsync();
        }
    }
}
