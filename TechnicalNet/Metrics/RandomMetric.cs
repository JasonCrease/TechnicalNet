using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TechnicalNet.Metrics
{
    public class RandomMetric : IMetric
    {
        public double Val { get; set; }

        public void Analyse(StockHistory data)
        {
            Val = (new System.Random()).NextDouble() - 0.5D;
        }

        public void Render(Graph g)
        {
            g.DrawPoint(Color.Red, 200, 300);
            throw new NotImplementedException();
        }
    }
}
