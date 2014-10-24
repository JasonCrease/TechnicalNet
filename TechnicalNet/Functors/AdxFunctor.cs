using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TechnicalNet.Functors
{
    // http://en.wikipedia.org/wiki/Average_directional_movement_index
    public class AdxFunctor : IFunctor
    {
        public string Name { get { return "ADX"; } }
        public double Val { get; private set; }

        public void Analyse(TechnicalNet.RealData.StockHistory stock, int today)
        {
            double dmplus = 0;
            double dmminus = 0;
            double alpha = 0.1;

            for (int i = today; i > 50; i--)
            {
                double upmove = stock.Highs[i] - stock.Highs[i - 1];
                double downmove = stock.Lows[i - 1] - stock.Lows[i];
                if (upmove > downmove && upmove > 0)
                    dmplus = alpha * upmove + (1 - alpha) * dmplus;
                else dmplus = 0;
                if (downmove > upmove && downmove > 0)
                    dmminus = alpha * downmove + (1 - alpha) * dmminus;
            }

            double diplus = (100 * dmplus) / stock.ATR[today];
            double diminus = (100 * dmplus) / stock.ATR[today];
        }
    }
}
