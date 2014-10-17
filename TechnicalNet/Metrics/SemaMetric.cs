using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TechnicalNet.RealData;

namespace TechnicalNet.Metrics
{
    public class SemaMetric : XemaMetric
    {
        public SemaMetric(StockHistory data) : base(data)
        {
            m_Color = Color.SeaGreen;
        }
        public override double Rate { get { return 0.98D; } }
    }
    public class FemaMetric : XemaMetric
    {
        public FemaMetric(StockHistory data) : base(data)
        {
            m_Color = Color.RoyalBlue;
        }
        public override double Rate { get { return 0.88D; } }
    }   

    public abstract class XemaMetric : IMetric
    {
        public double Val { get; set; }
        public abstract double Rate { get; }
        public double[] Data;
        public Color m_Color;

        public XemaMetric(StockHistory stock)
        {
            Analyse(stock);
        }

        public void Analyse(StockHistory stock)
        {
            Data = new double[stock.Count];
            Data[0] = stock.Closes[0];

            for (int i = 1; i < stock.Count; i++)
                Data[i] = ((1 - Rate) * stock.Closes[i]) + (Rate * Data[i-1]);
        }

        public void Render(Graph g)
        {
            g.DrawJoinedPoints(m_Color, Data);
        }
    }
}
