using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TechnicalNet.Functors
{
    /// <summary>
    /// Exponential-moving average crossover.
    /// </summary>
    public class EmaFunctor : IFunctor
    {
        public string Name { get { return "EMA crossover"; } }
        public double Val { get; set; }
        private double Rate1 = 0.8D;
        private double Rate2 = 0.98D;

        public void Analyse(TechnicalNet.RealData.StockHistory stock, int today)
        {
            double[] data1 = new double[stock.Count];
            data1[0] = stock.Closes[0];
            double[] data2 = new double[stock.Count];
            data2[0] = stock.Closes[0];

            for (int i = 1; i < stock.Count; i++)
                data1[i] = ((1 - Rate1) * stock.Closes[i]) + (Rate1 * data1[i - 1]);
            for (int i = 1; i < stock.Count; i++)
                data2[i] = ((1 - Rate2) * stock.Closes[i]) + (Rate2 * data2[i - 1]);

            Val = 0D;

            for (int i = 1; i < today; i++)
            {
                if ((data1[i - 1] < data2[i - 1]) && (data1[i] > data2[i]))
                    Val = -1D;
                if ((data1[i - 1] > data2[i - 1]) && (data1[i] < data2[i]))
                    Val = 1D;
            }
        }
    }
}
