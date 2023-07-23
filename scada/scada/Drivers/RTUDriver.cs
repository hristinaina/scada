namespace scada.Drivers
{
    public class RTUDriver : IDriver
    {
        private static IDictionary<string, double> AddressValues = new Dictionary<string, double>();

        public static double GetValue(string address)
        {
            return AddressValues[address];
        }

        public static void SetValue(string address, double value) 
        {
            AddressValues[address] = value;
        }
    }
}
