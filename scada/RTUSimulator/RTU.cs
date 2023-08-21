using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTUSimulator
{
    public class RTU
    {
        public string Address { get; set; }
        public int Value { get; set; }
        public int LowLimit { get; set; }
        public int HighLimit { get; set; }

        public RTU() { }

        public RTU(string address, int lowLimit, int highLimit)
        {
            Address = address;
            LowLimit = lowLimit;
            HighLimit = highLimit;
        }
    }
}
