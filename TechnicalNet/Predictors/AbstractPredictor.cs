using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalNet.RealData;

namespace TechnicalNet.Predictor
{
    public abstract class AbstractPredictor
    {
        public abstract double PredictValue(TechnicalNet.RealData.StockHistory stockHistory, int today, int daysInFuture);

        public abstract string Name { get; }
    }
}
