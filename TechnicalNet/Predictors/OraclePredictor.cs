using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.Predictor
{
    // A cheaty oracle Predictor. Just return the value in 
    public class OraclePredictor : AbstractPredictor
    {
        public override string Name
        {
            get { return "Oracle"; }
        }

        public override double PredictValue(TechnicalNet.RealData.StockHistory stockHistory, int today, int daysInFuture)
        {
            return stockHistory.Closes[today + daysInFuture];
        }
    }
}
