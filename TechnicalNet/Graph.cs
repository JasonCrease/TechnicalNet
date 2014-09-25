using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TechnicalNet.Metrics;

namespace TechnicalNet
{
    public class Graph
    {
        double yMin, yMax;
        double xMin, xMax;
        double yRange;
        public Bitmap Bitmap { get; set; }
        Graphics m_Graphics;
        TechnicalNet.StockHistory m_StockHistory;
        List<TechnicalNet.Metrics.IMetric> m_Metrics;

        int MarginHorizontal = 50;
        int MarginVertical = 20;

        int BitmapHeight = 600;
        int BitmapWidth = 900;

        int GraphHeight = -1;
        int GraphWidth = -1;

        double HorizontalStretch = -1;

        public Graph(TechnicalNet.StockHistory stockHistory, List<TechnicalNet.Metrics.IMetric> metrics)
        {
            m_StockHistory = stockHistory;
            m_Metrics = metrics;
            Bitmap = new Bitmap(BitmapWidth, BitmapHeight);
            m_Graphics = Graphics.FromImage(Bitmap);

            yMin = stockHistory.Opens.Min();
            yMax = stockHistory.Opens.Max();
            xMin = stockHistory.StartDate.Ticks;
            xMax = stockHistory.EndDate.Ticks;
            yRange = yMax - yMin;

            GraphHeight = BitmapHeight - (MarginVertical * 2);
            GraphWidth = BitmapWidth - (MarginHorizontal * 2);

            HorizontalStretch = ((double)GraphWidth / 250D);

            DrawLeftAxis();
            DrawBottomAxis();
            DrawValues();
            DrawMetrics();
            DrawCutoff();
        }

        private void DrawMetrics()
        {
            foreach(IMetric metric in m_Metrics)
            {
                metric.Render(this);
            }
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
                y = (((yMax - val) / yRange) * GraphHeight) + MarginVertical;
                x = (i * HorizontalStretch) + MarginHorizontal;

                //m_Graphics.DrawEllipse(penBlue, (int)x, (int)y, 2, 2);

                if (i > 0)
                {
                    m_Graphics.DrawLine(penLightBlue, (int)oldx, (int)oldy, (int)x, (int)y);
                }
            }
        }

        private void DrawLeftAxis()
        {
            Pen p = new Pen(Color.Black);
            m_Graphics.DrawLine(p, MarginHorizontal, MarginVertical, MarginHorizontal, GraphHeight + MarginVertical);
            m_Graphics.DrawString(yMax.ToString(), new Font(FontFamily.GenericSerif, 9), Brushes.BlueViolet, 5, MarginVertical);
            m_Graphics.DrawString(yMin.ToString(), new Font(FontFamily.GenericSerif, 9), Brushes.BlueViolet, 5, GraphHeight + MarginVertical);
        }

        private void DrawCutoff()
        {
            Pen p = new Pen(Color.Gray);
            m_Graphics.DrawLine(p, MarginHorizontal + (int)(HorizontalStretch * 150), MarginVertical, MarginHorizontal + (int)(HorizontalStretch * 150), GraphHeight - MarginVertical);
        }

        private void DrawBottomAxis()
        {
            Pen p = new Pen(Color.Black);
            m_Graphics.DrawLine(p, MarginHorizontal, GraphHeight + MarginVertical, GraphWidth + MarginHorizontal, GraphHeight + MarginVertical);
            m_Graphics.DrawString(m_StockHistory.StartDate.ToShortDateString(),
                new Font(FontFamily.GenericSansSerif, 9), Brushes.BlueViolet, MarginHorizontal, GraphHeight + MarginVertical);
            m_Graphics.DrawString(m_StockHistory.EndDate.ToShortDateString(),
                new Font(FontFamily.GenericSansSerif, 9), Brushes.BlueViolet, GraphWidth + MarginHorizontal - 20, GraphHeight + MarginVertical);
        }

        internal void DrawPoint(Color color, double t, double val)
        {
            Pen p = new Pen(color);
            double y = (((yMax - val) / yRange) * GraphWidth) + MarginVertical;
            double x = (t * HorizontalStretch) + MarginHorizontal;
            m_Graphics.DrawRectangle(p, (int)x, (int)(y), 1, 1);
        }

        internal void DrawJoinedPoints(Color color, double[] ys)
        {
            Pen pen = new Pen(color);

            double y = 0d;
            double x = 0d;

            for (int i = 0; i < ys.Length; i++)
            {
                double val = ys[i];
                double oldy = y;
                double oldx = x;
                y = (((yMax - val) / yRange) * GraphHeight) + MarginVertical;
                x = (i * HorizontalStretch) + MarginHorizontal;

                if (i > 0 && y > 1 && oldy > 1)
                {
                    m_Graphics.DrawLine(pen, (int)oldx, (int)oldy, (int)x, (int)y);
                }
            }
        }
    }
}