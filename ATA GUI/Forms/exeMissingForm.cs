using System;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class ExeMissingForm : Form
    {

        private readonly string title;
        private readonly string message;

        public ExeMissingForm(string message, string title)
        {
            InitializeComponent();

            this.title = title;
            this.message = message;
        }

        private void adbMissingForm_Load(object sender, EventArgs e)
        {
            label1.Text = message;
            this.Text = title;
        }

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            this.Close();
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            this.Close();
        }
    }
}
