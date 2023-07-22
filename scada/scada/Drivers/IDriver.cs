namespace scada.Drivers
{
    public interface IDriver
    {
        public abstract static double GetValue(string address);
    }
}
