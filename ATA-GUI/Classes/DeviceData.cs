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

        public DeviceMode Mode { get; set; }

        public DeviceConnection Connection { get; }

        public string ID { get; }

        public char getConnectionSymbol()
        {
            return Connection.ToString()[0];
        }
    }

    internal enum DeviceMode
    {
        RECOVERY,
        SYSTEM,
        UNAUTHORIZED,
        UNKNOWN,
        FASTBOOT
    }

    internal enum DeviceConnection
    {
        CABLE,
        WIRELESS
    }
}
