using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.Strategy
{
    // A cheaty oracle strategy. Just return the value in 
    public class OracleStrategy : AbstractStrategy
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
