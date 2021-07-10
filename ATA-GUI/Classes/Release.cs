using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATA_GUI
{
    class Release
    {
        private int number;
        private bool pre;

        public Release()
        {
            number = -1;
            pre = false;
        }

        public int Number { get => number; set => number = value; }
        public bool Pre { get => pre; set => pre = value; }
    }
}
