using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.Strategy
{
    // Just return today's value
    public class ThingsDontChangeStrategy : IStrategy
    {
        public double PredictValue(TechnicalNet.RealData.StockHistory stockHistory, int today, int daysInFuture)
        {
            return stockHistory.Closes[today];
        }
    }
}
