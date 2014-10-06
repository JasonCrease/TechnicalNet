using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.Strategy
{
    public class LinearSlopeStrategy : AbstractStrategy
    {
        public override string Name
        {
            get { return "Linear slope"; }
        }

        public override double PredictValue(TechnicalNet.RealData.StockHistory stockHistory, int today, int daysInFuture)
        {
            int N = Math.Min(40, today - 1);

            double todayClose = stockHistory.Closes[today];
            double fromAgoClose = stockHistory.Closes[today - N];
            double slope = (todayClose - fromAgoClose) / (double)N;

            return todayClose + (slope * (double)daysInFuture);
        }
    }
}
