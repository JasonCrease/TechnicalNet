using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.Strategy
{
    public interface IStrategy
    {
        double PredictValue(StockHistory stockHistory, int today, int daysInFuture);
    }
}
