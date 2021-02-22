using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            ScrollableMessageBox scrollableMessageBox = new ScrollableMessageBox();
            scrollableMessageBox.Text = title;
            scrollableMessageBox.richTextBox.Text = text;
            scrollableMessageBox.Show();
        }
    }
}
