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

namespace TechNet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TechnicalNet.HistoricalData m_Data;
        int m_ShareNum;

        public MainWindow()
        {
            InitializeComponent();
            AddPredictors();
            m_Data = new TechnicalNet.HistoricalData();
            m_ShareNum = 0;
            UpdateImage();
        }

        List<IPredictor> m_Predictors;
        List<Label> m_PredictorLabels;

        private void AddPredictors()
        {
            m_PredictorLabels = new List<Label>();

            m_Predictors = new List<IPredictor>();
            m_Predictors.Add(new EmaPredictor());
            m_Predictors.Add(new TenDaysMomentumPredictor());
            m_Predictors.Add(new HundredDaysMomentumPredictor());

            foreach (IPredictor p in m_Predictors)
            {
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;
                Label nameLabel = new Label();
                nameLabel.Content = p.Name;
                nameLabel.Width = 120;
                Label valLabel = new Label();
                valLabel.Content = p.Val;
                valLabel.Tag = p;
                panel.Children.Add(nameLabel);
                panel.Children.Add(valLabel);
                MetricsPanel.Children.Add(panel);

                m_PredictorLabels.Add(valLabel);
            }
        }

        public void UpdateImage()
        {
            StockHistory data = m_Data.AllStocks[m_ShareNum];

            var metrics = new List<IMetric>();

            SemaMetric semaMetric = new SemaMetric(data);
            metrics.Add(semaMetric);
            FemaMetric femaMetric = new FemaMetric(data);
            metrics.Add(femaMetric);

            //BollingerBandsMetric bollingerBandsMetric = new BollingerBandsMetric(data);
            //metrics.Add(bollingerBandsMetric);

            foreach (Label label in m_PredictorLabels)
            {
                IPredictor pred = (IPredictor)label.Tag;
                pred.Analyse(data);
                label.Content = pred.Val.ToString("0.000");
            }

            Graph g = new Graph(data, metrics);
            img.Source = ToBitmapSource(g.Bitmap);
            LabelShareName.Content = data.Name;

            //LabelAction.Content = emaPredictor.Val;
        }

        private void ButtonNextShare_Click(object sender, RoutedEventArgs e)
        {
            m_ShareNum++;
            UpdateImage();
        }

        private void ButtonPrevShare_Click(object sender, RoutedEventArgs e)
        {
            m_ShareNum--;
            UpdateImage();
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
    }
}
