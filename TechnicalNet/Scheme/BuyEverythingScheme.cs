using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalNet.RealData;
using TechnicalNet.Predictor;

namespace TechnicalNet.Scheme
{
    public class BuyEverythingScheme : AbstractScheme
    {
        public override string Name { get { return "BuyEverything"; } }

        Random random = new Random();

        public override double GetSchemeProfit(StockHistorySet testData, AbstractPredictor Predictor, int today, int daysToPredict)
        {
            double profit = 0D;

            foreach (StockHistory stockHistory in testData.AllStockHistories)
            {
                double startV = stockHistory.Closes[today];
                double actualEndV = stockHistory.Closes[today + daysToPredict];

                profit += (actualEndV - startV);
            }

            return profit / testData.AllStockHistories.Count;
        }
    }
}

