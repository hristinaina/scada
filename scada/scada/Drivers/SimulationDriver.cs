namespace scada.Drivers
{
    public class SimulationDriver : IDriver
    {
        public static double GetValue(string address)
        {
            string[] sine = { "s1", "s2", "s3" };
            string[] cosine = { "c1", "c2", "c3" };
            string[] ramp = { "r1", "r2", "r3" };

            if (sine.Contains(address)) return Sine();
            else if (cosine.Contains(address)) return Cosine();
            else if (ramp.Contains(address)) return Ramp();
            else return -1000;
        }

        private static double Sine()
        {
            return 100 * Math.Sin((double)DateTime.Now.Second / 60 * Math.PI);
        }

        private static double Cosine()
        {
            return 100 * Math.Cos((double)DateTime.Now.Second / 60 * Math.PI);
        }

        private static double Ramp()
        {
            return 100 * DateTime.Now.Second / 60;
        }
    }
}
