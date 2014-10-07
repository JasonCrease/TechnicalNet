using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.Predictor
{
    // Just return today's value
    public class ThingsDontChangePredictor : AbstractPredictor
    {
        public override string Name
        {
            get { return "Things don't change"; }
        }

        public override double PredictValue(TechnicalNet.RealData.StockHistory stockHistory, int today, int daysInFuture)
        {
            return stockHistory.Closes[today];
        }
    }
}
