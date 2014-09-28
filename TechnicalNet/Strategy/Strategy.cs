using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalNet.RealData;

namespace TechnicalNet.Strategy
{
    public abstract class Strategy
    {
        public abstract double PredictValue(TechnicalNet.RealData.StockHistory stockHistory, int today, int daysInFuture);     
        
        /// <summary>
        /// How good the strategy is. Specifically, if you invested $1 in it, how much would you get back.
        /// </summary>
        /// <returns>The wins/loses on 1.00 dollars</returns>
        public double GetStrategyProfit(StockHistorySet testData, int today, int daysToPredict)
        {
            double profit = 0D;

            foreach (var stock in testData.AllStocks)
            {
                double predictedValue = this.PredictValue(stock, 150, 50);
                double actualValue = stock.Closes[today + daysToPredict];
                profit += (actualValue - predictedValue);
            }

            return profit;
        }
    }
}
