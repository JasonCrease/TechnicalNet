using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TechnicalNet.Metrics
{
    public interface IMetric
    {
        double Val { get; set; }
        void Analyse(TechnicalNet.RealData.StockHistory data);
        void Render(Graph g);
    }
}
