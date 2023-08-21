using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTUSimulator
{
    public class RTU
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public double LowLimit { get; set; }
        public double HighLimit { get; set; }

    }
}
