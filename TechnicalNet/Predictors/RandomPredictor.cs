using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.Predictor
{
    /// <summary>
    /// Gives you a random predicted price. The price is sensible - having mean x and variance x where x is the current price
    /// </summary>
    public class RandomPredictor : AbstractPredictor
    {
        static Random random = new Random();

        public override double PredictValue(RealData.StockHistory stockHistory, int today, int daysInFuture)
        {
            double change = -Math.Log(random.NextDouble()) * Math.E;
            return stockHistory.Closes[today] * Math.Exp(change);
        }

        public override string Name
        {
            get { return "Random Predictor"; }
        }
    }
}
