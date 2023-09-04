namespace scada.Drivers
{
    public class RTUDriver : IDriver
    {
        private static IDictionary<string, double> addressValues = new Dictionary<string, double>();
        private static readonly object Locker = new object();

        public double GetValue(string address)
        {
            return addressValues[address];
        }

        public static void SetValue(string address, double value) 
        {
            lock (Locker)
            {
                addressValues[address] = value;
                /*Console.WriteLine("START");
                foreach (var kvp in addressValues)
                {
                    Console.Write(kvp.Key);
                    Console.Write("|");
                    Console.Write(kvp.Value);
                    Console.WriteLine();
                }
                Console.WriteLine("END");*/
            }
        }

        public static List<String> GetAddresses()
        {
            return addressValues.Keys.ToList();
        }
    }
}
