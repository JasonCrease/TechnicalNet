using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalNet.RealData;
using TechnicalNet.Predictor;

namespace TechnicalNet.Scheme
{
    public class BuyTenBestScheme : AbstractScheme
    {
        public override string Name { get { return "Buy10Best"; } }

        public override double GetSchemeProfit(StockHistorySet testData, AbstractPredictor Predictor, int today, int daysToPredict)
        {
            var stocksInOrder = testData.AllStockHistories.OrderByDescending(x => Predictor.PredictValue(x, today, daysToPredict) / x.Closes[today]).ToArray();
            var tenBest = stocksInOrder.Take(10).ToArray();

            double profit = 0D;

            foreach (StockHistory stockHistory in tenBest)
            {
                double startV = stockHistory.Closes[today];
                double predictedEndV = Predictor.PredictValue(stockHistory, today, daysToPredict);
                double actualEndV = stockHistory.Closes[today + daysToPredict];

                profit += (actualEndV - startV);
            }

            return profit / 10;
        }
    }
}
