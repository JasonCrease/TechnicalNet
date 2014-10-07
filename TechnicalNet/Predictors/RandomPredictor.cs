using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.Predictor
{
    public class RandomPredictor : AbstractPredictor
    {
        static Random random = new Random();

        public override double PredictValue(RealData.StockHistory stockHistory, int today, int daysInFuture)
        {
            double change = random.NextDouble();
            return stockHistory.Closes[today] * Math.Exp(change);
        }

        public override string Name
        {
            get { return "Random Predictor"; }
        }
    }
}
