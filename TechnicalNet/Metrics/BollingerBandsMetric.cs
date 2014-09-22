using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TechnicalNet.Metrics
{
    public class BollingerBandsMetric : IMetric
    {
        public double Val { get; set; }
        public int N = 10;  // this is half the period of the mean/variance
        public int K = 4;   // number of SDs wide
        public double[] LowerBand;
        public double[] UpperBand;
        public double[] MaBand;
        public double[] Volatility;

        public BollingerBandsMetric(StockHistory stock)
        {
            Analyse(stock);
        }

        public void Analyse(StockHistory stock)
        {
            LowerBand = new double[stock.Count];
            UpperBand = new double[stock.Count];
            Volatility = new double[stock.Count];
            MaBand = new double[stock.Count];

            MaBand[0] = stock.Closes[0];
            for (int i = N; i < stock.Count - N; i++)
                for (int j = -N; j < N; j++)
                {
                    MaBand[i] += stock.Closes[i + j];
                }
            for (int i = N; i < stock.Count - N; i++)
                MaBand[i] /= (double)(N * 2);

            for (int i = N * 2; i < stock.Count - (N * 2); i++)
                for (int j = -N; j < N; j++)
                {
                    Volatility[i] += (stock.Closes[i + j] - MaBand[i + j]) * (stock.Closes[i + j] - MaBand[i + j]);
                }

            for (int i = N; i < stock.Count - N; i++)
                Volatility[i] = Math.Sqrt(Volatility[i]);
            for (int i = N; i < stock.Count - N; i++)
                Volatility[i] /= (double)(N * 2);

            for (int i = 0; i < stock.Count; i++)
                LowerBand[i] = MaBand[i] - (Volatility[i] * K);
            for (int i = 0; i < stock.Count; i++)
                UpperBand[i] = MaBand[i] + (Volatility[i] * K);
        }

        public void Render(Graph g)
        {
            g.DrawJoinedPoints(Color.RoyalBlue, LowerBand);
            g.DrawJoinedPoints(Color.Salmon, MaBand);
            g.DrawJoinedPoints(Color.RosyBrown, UpperBand);
        }
    }
}
