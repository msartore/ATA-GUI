namespace ATA_GUI.Classes
{
    public class AppData
    {
        public AppData(string name, string package)
        {
            this.Name = name;
            this.Package = package;
        }

        public string Name { get; set; }
        public string Package { get; set; }
    }
}
