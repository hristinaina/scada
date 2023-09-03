namespace scada.Drivers
{
    public interface IDriver
    {
        public abstract double GetValue(string address);
    }
}
