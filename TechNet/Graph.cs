using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TechNet
{
    class Graph
    {
        double yMin, yMax;
        double xMin, xMax;
        double yRange;
        public Bitmap Bitmap { get; set; }
        Graphics m_Graphics;
        TechnicalNet.StockHistory m_StockHistory;
        List<TechnicalNet.Metrics.IMetric> m_Metrics;
        int GraphStartX = 50;
        int GraphStartY = 20;

        public Graph(TechnicalNet.StockHistory stockHistory, List<TechnicalNet.Metrics.IMetric> metrics)
        {
            m_StockHistory = stockHistory;
            m_Metrics = metrics;
            Bitmap = new Bitmap(600, 500);
            m_Graphics = Graphics.FromImage(Bitmap);

            yMin = stockHistory.Opens.Min();
            yMax = stockHistory.Opens.Max();
            xMin = stockHistory.StartDate.Ticks;
            xMax = stockHistory.EndDate.Ticks;
            yRange = yMax - yMin;

            DrawLeftAxis();
            DrawBottomAxis();
            DrawValues();
            DrawMetrics();
        }

        private void DrawMetrics()
        {
            //throw new NotImplementedException();
        }

        private void DrawValues()
        {
            Pen penBlue = new Pen(Color.Blue);
            Pen penLightBlue = new Pen(Color.LightBlue);
            double y = 0d;
            double x = 0d;
            
            for (int i = 0; i < m_StockHistory.Count; i++)
            {
                double val = m_StockHistory.Opens[i];
                double oldy = y;
                double oldx = x;
                y = ((val - yMin) / yRange) * 460d;
                x = (i * 2) + GraphStartX;

                m_Graphics.DrawEllipse(penBlue, (int)x, (int)y, 2, 2);

                if (i > 0)
                {
                    m_Graphics.DrawLine(penLightBlue, (int)oldx, (int)oldy, (int)x, (int)y);
                }
            }
        }

        private void DrawLeftAxis()
        {
            Pen p = new Pen(Color.Black);
            m_Graphics.DrawLine(p, GraphStartX, GraphStartY, GraphStartX, 480);
            m_Graphics.DrawString(yMin.ToString(), new Font(FontFamily.GenericSerif, 6), Brushes.BlueViolet, 5, 460);
            m_Graphics.DrawString(yMax.ToString(), new Font(FontFamily.GenericSerif, 6), Brushes.BlueViolet, 5, 10);
        }

        private void DrawBottomAxis()
        {
            Pen p = new Pen(Color.Black);
            m_Graphics.DrawLine(p, GraphStartX, 480, 500 + GraphStartX, 480);
            m_Graphics.DrawString(m_StockHistory.StartDate.ToShortDateString(),
                new Font(FontFamily.GenericSansSerif, 6), Brushes.BlueViolet, GraphStartX - 30, 480);
            m_Graphics.DrawString(m_StockHistory.EndDate.ToShortDateString(),
                new Font(FontFamily.GenericSansSerif, 6), Brushes.BlueViolet, 500 + GraphStartX - 30, 480);
        }

    }
}
