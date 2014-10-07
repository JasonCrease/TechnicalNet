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
using TechnicalNet.Functors;
using System.Runtime.InteropServices;
using TechnicalNet.Predictor;
using TechnicalNet.RealData;
using TechnicalNet.Scheme;

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

            AddFunctors();
            AddStrategies();
            UpdateImage();
        }

        private void AddStrategies()
        {
            List<AbstractScheme> schemes = new List<AbstractScheme>();
            schemes.Add(new BuyTenBestScheme());
            schemes.Add(new LongOneOrShortOneScheme());

            List<AbstractPredictor> strategies = new List<AbstractPredictor>();
            strategies.Add(new ThingsDontChangePredictor());
            strategies.Add(new OraclePredictor());
            strategies.Add(new LinearSlopePredictor());
            strategies.Add(new LogSlopePredictor());
            strategies.Add(new EverythingWillDoublePredictor());
            strategies.Add(new EverythingWillHalfPredictor());

            foreach (AbstractScheme scheme in schemes)
            {
                StackPanel schemePanel = new StackPanel();
                schemePanel.Orientation = Orientation.Vertical;
                PredictorPanel.Children.Add(schemePanel);

                Label schemeNameLabel = new Label();
                schemeNameLabel.Content = scheme.Name;
                schemeNameLabel.Width = 120;
                schemeNameLabel.FontSize = 14;
                schemePanel.Children.Add(schemeNameLabel);

                foreach (AbstractPredictor Predictor in strategies)
                {
                    StackPanel panel = new StackPanel();
                    panel.Orientation = Orientation.Horizontal;
                    schemePanel.Children.Add(panel);

                    Label nameLabel = new Label();
                    nameLabel.Content = Predictor.Name;
                    nameLabel.Width = 120;

                    Label valLabel = new Label();
                    valLabel.Content = scheme.GetSchemeProfit(m_Data, Predictor, 150, 50).ToString("$0.00");
                    valLabel.Tag = Predictor;

                    panel.Children.Add(nameLabel);
                    panel.Children.Add(valLabel);
                }
            }
        }

        List<Label> m_FunctorLabels;

        private void AddFunctors()
        {
            m_FunctorLabels = new List<Label>();

            List<IFunctor> Functors = new List<IFunctor>();
            Functors.Add(new EmaFunctor());
            Functors.Add(new TenDaysTangentFunctor());
            Functors.Add(new HundredDaysTangentFunctor());
            Functors.Add(new EmaFunctor());

            foreach (IFunctor p in Functors)
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
                m_FunctorLabels.Add(valLabel);

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

            foreach (Label label in m_FunctorLabels)
            {
                IFunctor pred = (IFunctor)label.Tag;
                pred.Analyse(data);
                label.Content = pred.Val.ToString("0.000");
            }

            Graph g = new Graph(data, metrics);

            AbstractPredictor Predictor = new ThingsDontChangePredictor();
            double val = Predictor.PredictValue(data, 150, 50);
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
