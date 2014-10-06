using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using TechnicalNet;
using TechnicalNet.Metrics;
using TechnicalNet.Predictors;
using System.Runtime.InteropServices;
using TechnicalNet.Strategy;
using TechnicalNet.RealData;

namespace TechNet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StockHistorySet m_Data;
        int m_ShareNum;

        public MainWindow()
        {
            InitializeComponent();

            m_Data = new Sp500History();
            m_ShareNum = 200;

            AddPredictors();
            AddStrategies();
            UpdateImage();
        }

        List<Label> m_StrategyLabels;

        private void AddStrategies()
        {
            m_StrategyLabels = new List<Label>();

            List<AbstractStrategy> strategies = new List<AbstractStrategy>();
            strategies.Add(new ThingsDontChangeStrategy());
            strategies.Add(new OracleStrategy());
            strategies.Add(new LinearSlopeStrategy());
            strategies.Add(new LogSlopeStrategy());
            strategies.Add(new EverythingWillDoubleStrategy());
            strategies.Add(new EverythingWillHalfStrategy());

            foreach (AbstractStrategy s in strategies)
            {
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;
                StrategyPanel.Children.Add(panel);

                Label nameLabel = new Label();
                nameLabel.Content = s.Name;
                nameLabel.Width = 120;

                Label valLabel = new Label();
                valLabel.Content = s.GetStrategyProfit(m_Data, 150, 50).ToString("$0.00");
                valLabel.Tag = s;
                m_StrategyLabels.Add(valLabel);

                panel.Children.Add(nameLabel);
                panel.Children.Add(valLabel);
            }
        }

        List<Label> m_PredictorLabels;

        private void AddPredictors()
        {
            m_PredictorLabels = new List<Label>();

            List<IPredictor> predictors = new List<IPredictor>();
            predictors.Add(new EmaPredictor());
            predictors.Add(new TenDaysTangentPredictor());
            predictors.Add(new HundredDaysTangentPredictor());
            predictors.Add(new EmaPredictor());

            foreach (IPredictor p in predictors)
            {
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;
                MetricsPanel.Children.Add(panel);

                Label nameLabel = new Label();
                nameLabel.Content = p.Name;
                nameLabel.Width = 120;

                Label valLabel = new Label();
                valLabel.Content = p.Val;
                valLabel.Tag = p;
                m_PredictorLabels.Add(valLabel);

                panel.Children.Add(nameLabel);
                panel.Children.Add(valLabel);
            }
        }

        public void UpdateImage()
        {
            StockHistory data = m_Data.AllStockHistories[m_ShareNum];

            var metrics = new List<IMetric>();

            //SemaMetric semaMetric = new SemaMetric(data);
            //metrics.Add(semaMetric);
            //FemaMetric femaMetric = new FemaMetric(data);
            //metrics.Add(femaMetric);
            BollingerBandsMetric bollingerBandsMetric = new BollingerBandsMetric(data);
            metrics.Add(bollingerBandsMetric);

            foreach (Label label in m_PredictorLabels)
            {
                IPredictor pred = (IPredictor)label.Tag;
                pred.Analyse(data);
                label.Content = pred.Val.ToString("0.000");
            }

            Graph g = new Graph(data, metrics);

            AbstractStrategy strategy = new ThingsDontChangeStrategy();
            double val = strategy.PredictValue(data, 150, 50);
            g.DrawPredictionPoint(val, 150 + 50);

            img.Source = ToBitmapSource(g.Bitmap);
            LabelShareName.Content = data.Name;
            LabelProfit.Content = data.Profit.ToString("0.00%");
        }

        private void ButtonNextShare_Click(object sender, RoutedEventArgs e)
        {
            if (m_ShareNum < m_Data.AllStockHistories.Count - 1)
            {
                m_ShareNum++;
                UpdateImage();
            }
        }

        private void ButtonPrevShare_Click(object sender, RoutedEventArgs e)
        {
            if (m_ShareNum > 0)
            {
                m_ShareNum--;
                UpdateImage();
            }
        }

        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        public static BitmapSource ToBitmapSource(Bitmap bitmap)
        {
            IntPtr ptr = bitmap.GetHbitmap();

            BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                ptr,
                IntPtr.Zero,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(ptr);
            return bs;
        }

        private void ButtonFindProfits_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
