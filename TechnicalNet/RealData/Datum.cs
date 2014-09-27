using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.RealData
{
    public class Datum
    {
        public double Open, Close, High, Low, Volume;
        public DateTime Date;

        public Datum(DateTime date, double open, double close, double high, double low, double volume)
        {
            this.Open = open;
            this.Close = close;
            this.High = high;
            this.Low = low;
            this.Volume = volume;
            this.Date = date;
        }
    }
}
