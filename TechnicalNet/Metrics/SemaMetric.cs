using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TechnicalNet.Metrics
{
    public class SemaMetric : IMetric
    {
        public double Val { get; set; }
        public const Double Rate= 5f;
        public double[] Data;

        public void Analyse(HistoricalData data)
        {
        }

        public void Render(Graph g)
        {

        }
    }
}
