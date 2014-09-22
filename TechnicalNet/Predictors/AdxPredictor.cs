using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TechnicalNet.Predictors
{
    // http://en.wikipedia.org/wiki/Average_directional_movement_index
    public class AdxPredictor : IPredictor
    {
        public string Name { get { return "ADX"; } }
        public double Val { get; private set; }

        public void Analyse(StockHistory stock)
        {
            throw new NotImplementedException();
        }
    }
}
