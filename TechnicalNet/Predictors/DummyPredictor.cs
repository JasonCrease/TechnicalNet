using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TechnicalNet.Predictors
{
    public class DummyPredictor : IPredictor
    {
        public string Name { get { return "Dummy"; } }
        public double Val { get; private set; }

        public void Analyse(StockHistory stock)
        {
            throw new NotImplementedException();
        }
    }
}
