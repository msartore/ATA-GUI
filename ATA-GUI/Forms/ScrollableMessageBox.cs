using System.Diagnostics;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class ScrollableMessageBox : Form
    {
        public ScrollableMessageBox()
        {
            InitializeComponent();
        }

        public static void show(string text, string title)
        {
            ScrollableMessageBox scrollableMessageBox = new()
            {
                Text = title
            };
            scrollableMessageBox.richTextBox.Text = text;
            scrollableMessageBox.Show();
        }

        private void richTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            _ = Process.Start(e.LinkText);
        }
    }
}
