namespace ATA_GUI
{
    internal class Device
    {
        public Device()
        {
            Name = "";
            Serial = "";
        }

        public string Serial { get; set; }
        public string Name { get; set; }
    }
}
