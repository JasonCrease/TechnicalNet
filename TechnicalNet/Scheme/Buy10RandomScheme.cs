using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalNet.RealData;
using TechnicalNet.Predictor;

namespace TechnicalNet.Scheme
{
    public class Buy10RandomScheme : AbstractScheme
    {
        public override string Name { get { return "Buy10Random"; } }

        Random random = new Random();

        public override double GetSchemeProfit(StockHistorySet testData, AbstractPredictor Predictor, int today, int daysToPredict)
        {
            var stocksInOrder = testData.AllStockHistories.OrderByDescending(x => random.NextDouble()).ToArray();
            var tenRandom = stocksInOrder.Take(10).ToArray();

            double profit = 0D;

            foreach (StockHistory stockHistory in tenRandom)
            {
                double startV = stockHistory.Closes[today];
                double actualEndV = stockHistory.Closes[today + daysToPredict];

                profit += (actualEndV - startV);
            }

            return profit / 10;
        }
    }
}

