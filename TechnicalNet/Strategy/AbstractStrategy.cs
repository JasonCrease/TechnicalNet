using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalNet.RealData;

namespace TechnicalNet.Strategy
{
    public abstract class AbstractStrategy
    {
        public abstract double PredictValue(TechnicalNet.RealData.StockHistory stockHistory, int today, int daysInFuture);

        public abstract string Name { get; }

        /// <summary>
        /// How good the strategy is. Specifically, if you invested $1 in it, how much would you get back.
        /// </summary>
        /// <returns>The wins/loses on 1.00 dollars</returns>
        public double GetStrategyProfit(StockHistorySet testData, int today, int daysToPredict)
        {
            return BuyTenBest(testData, today, daysToPredict);
            //return BuyIfUpOtherwiseShort(testData, today, daysToPredict);
        }

        private double BuyTenBest(StockHistorySet testData, int today, int daysToPredict)
        {
            var stocksInOrder = testData.AllStockHistories.OrderByDescending(x => this.PredictValue(x, today, daysToPredict) / x.Closes[today]).ToArray();
            var tenBest = stocksInOrder.Take(10).ToArray();

            double profit = 0D;

            foreach (StockHistory stockHistory in tenBest)
            {
                double startV = stockHistory.Closes[today];
                double predictedEndV = this.PredictValue(stockHistory, today, daysToPredict);
                double actualEndV = stockHistory.Closes[today + daysToPredict];

                profit += (actualEndV - startV);
            }

            return profit / 10;
        }

        private double BuyIfUpOtherwiseShort(StockHistorySet testData, int today, int daysToPredict)
        {
            double profit = 0D;

            // If prediction is higher, buy 1 unit. Otherwise, short 1 unit
            foreach (StockHistory stockHistory in testData.AllStockHistories)
            {
                double startV = stockHistory.Closes[today];
                double predictedEndV = this.PredictValue(stockHistory, today, daysToPredict);
                double actualEndV = stockHistory.Closes[today + daysToPredict];

                if (predictedEndV > startV)
                    profit += (actualEndV - startV);
                else
                    profit += (startV - actualEndV);
            }

            return profit / 475;
        }
    }
}
