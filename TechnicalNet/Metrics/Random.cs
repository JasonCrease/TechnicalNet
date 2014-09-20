using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.Metrics
{
    public class Random : IMetric
    {
        public double Analyse(HistoricalData data)
        {
            return (new System.Random()).NextDouble() - 0.5D;
        }
    }
}
