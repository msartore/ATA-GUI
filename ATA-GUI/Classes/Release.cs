namespace ATA_GUI
{
    internal class Release
    {
        public int Number { get; set; }
        public bool Pre { get; set; }
        public string Name { get; set; }

        public Release()
        {
            Number = -1;
            Pre = false;
        }
    }
}
