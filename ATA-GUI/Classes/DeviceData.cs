namespace ATA_GUI.Classes
{
    internal class DeviceData
    {
        public DeviceData(string name, string id, DeviceMode mode, DeviceConnection connection)
        {
            Name = name;
            ID = id;
            Mode = mode;
            Connection = connection;
        }

        public string Name { get; }

        public DeviceMode Mode { get; }

        public DeviceConnection Connection { get; }

        public string ID { get; }
    }

    internal enum DeviceMode
    {
        RECOVERY,
        SYSTEM
    }

    internal enum DeviceConnection
    {
        CABLE,
        WIRELESS
    }
}
