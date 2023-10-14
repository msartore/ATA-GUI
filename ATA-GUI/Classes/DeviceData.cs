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

        public bool sameMode(Tab tab)
        {
            return (Mode == DeviceMode.RECOVERY && tab == Tab.RECOVERY) || (Mode == DeviceMode.FASTBOOT && tab == Tab.FASTBOOT) || (Mode == DeviceMode.SYSTEM && tab == Tab.SYSTEM) || (Mode == DeviceMode.SIDELOAD && tab == Tab.RECOVERY);
        }
    }

    internal enum DeviceMode
    {
        RECOVERY,
        SYSTEM,
        UNAUTHORIZED,
        SIDELOAD,
        UNKNOWN,
        FASTBOOT
    }

    internal enum DeviceConnection
    {
        CABLE,
        WIRELESS
    }
}
