using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalNet.RealData;

namespace TechnicalNet.Strategy
{
    class Evaluator
    {
        /// <summary>
        /// How good the strategy is. Specifically, if you invested $1 in it, how much would you get back.
        /// </summary>
        /// <returns>The wins/loses on 1.00 dollars</returns>
        public static double GetOutcome(StockHistorySet testData, int today, int daysToPredict, IStrategy strategy)
        {
            foreach(var x in testData.AllStocks)
            {

            }

            return 1;
        }
    }
}
