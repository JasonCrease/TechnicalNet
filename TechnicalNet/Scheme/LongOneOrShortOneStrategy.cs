using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalNet.RealData;
using TechnicalNet.Predictor;

namespace TechnicalNet.Scheme
{
    public class LongOneOrShortOneScheme : AbstractScheme
    {
        public override string Name { get { return "Long1Short1"; } }

        public override double GetSchemeProfit(StockHistorySet testData, AbstractPredictor Predictor, int today, int daysToPredict)
        {
            double profit = 0D;

            // If prediction is higher, buy 1 unit. Otherwise, short 1 unit
            foreach (StockHistory stockHistory in testData.AllStockHistories)
            {
                double startV = stockHistory.Closes[today];
                double predictedEndV = Predictor.PredictValue(stockHistory, today, daysToPredict);
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
