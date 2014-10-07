using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalNet.RealData;
using TechnicalNet.Predictor;

namespace TechnicalNet.Scheme
{
    public abstract class AbstractScheme
    {
        public abstract string Name { get; }

        /// <summary>
        /// How good the Predictor is with this purchasing scheme. Specifically, if you invested $1 in it, how much would you get back.
        /// </summary>
        /// <returns>The profit of this scheme</returns>
        public abstract double GetSchemeProfit(StockHistorySet testData, AbstractPredictor Predictor, int today, int daysToPredict);
    }
}
