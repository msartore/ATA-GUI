using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATA_GUI
{
    class Device
    {
        private string name;
        private string serial;

        public Device()
        {
            Name = "";
            Serial = "";
        }

        public string Serial { get => serial; set => serial = value; }
        public string Name { get => name; set => name = value; }
    }
}
