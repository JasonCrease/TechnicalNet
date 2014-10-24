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
            AddSchemes();
            UpdateImage();
        }

        List<Label> m_ValLabels = new List<Label>();

        private void AddSchemes()
        {
            List<AbstractScheme> schemes = new List<AbstractScheme>();
            schemes.Add(new BuyTenBestScheme());
            schemes.Add(new LongGoodShortBadScheme());
            schemes.Add(new Buy10RandomScheme());
            schemes.Add(new BuyEverythingScheme());

            List<AbstractPredictor> predictors = new List<AbstractPredictor>();
            predictors.Add(new ThingsDontChangePredictor());
            predictors.Add(new OraclePredictor());
            predictors.Add(new LinearSlopePredictor());
            predictors.Add(new LogSlopePredictor());
            predictors.Add(new EverythingWillDoublePredictor());
            predictors.Add(new EverythingWillHalfPredictor());
            predictors.Add(new RandomPredictor());


            // Build columns

            ColumnDefinition predictorNamesColumn = new ColumnDefinition();
            predictorNamesColumn.Width = new GridLength(170);
            OutcomeGrid.ColumnDefinitions.Add(predictorNamesColumn);
            for (int i = 0; i < schemes.Count; i++)
            {
                ColumnDefinition colDefinition = new ColumnDefinition();
                colDefinition.Width = new GridLength(120);
                OutcomeGrid.ColumnDefinitions.Add(colDefinition);
            }


            // Build rows

            RowDefinition schemeNamesRow = new RowDefinition();
            OutcomeGrid.RowDefinitions.Add(schemeNamesRow);
            for (int i = 0; i < predictors.Count; i++)
                OutcomeGrid.RowDefinitions.Add(new RowDefinition());


            // Add scheme names to top row

            int colNum = 0;
            foreach (AbstractScheme scheme in schemes)
            {
                colNum++;

                Label schemeNameLabel = new Label();
                schemeNameLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                schemeNameLabel.Content = scheme.Name;

                OutcomeGrid.Children.Add(schemeNameLabel);
                Grid.SetRow(schemeNameLabel, 0);
                Grid.SetColumn(schemeNameLabel, colNum);
            }


            // Add strategy names to first column

            int rowNum = 0;
            foreach (AbstractPredictor predictor in predictors)
            {
                rowNum++;

                Label predictorNameLabel = new Label();
                predictorNameLabel.Content = predictor.Name;
                predictorNameLabel.Width = 120;

                OutcomeGrid.Children.Add(predictorNameLabel);
                Grid.SetRow(predictorNameLabel, rowNum);
                Grid.SetColumn(predictorNameLabel, 0);
            }


            //Add value labels

            colNum = 1;
            foreach (AbstractScheme scheme in schemes)
            {
                rowNum = 1;
                foreach (AbstractPredictor predictor in predictors)
                {
                    Label valLabel = new Label();
                    valLabel.Tag = new Func<double>(() => scheme.GetSchemeProfit(m_Data, predictor, 100, 50));
                    valLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    m_ValLabels.Add(valLabel);
                    OutcomeGrid.Children.Add(valLabel);
                    Grid.SetRow(valLabel, rowNum);
                    Grid.SetColumn(valLabel, colNum);
                    rowNum++;
                }
                colNum++;
            }



            ButtonFindProfits_Click(this, null);
        }

        List<Label> m_FunctorLabels;

        private void AddFunctors()
        {
            m_FunctorLabels = new List<Label>();

            List<IFunctor> functors = new List<IFunctor>();
            functors.Add(new EmaFunctor());
            functors.Add(new TenDaysTangentFunctor());
            functors.Add(new HundredDaysTangentFunctor());
            functors.Add(new EmaFunctor());

            foreach (IFunctor p in functors)
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
            LabelProfit.Content = data.Profit(240, 150).ToString("0.00%");
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
            foreach(Label l in m_ValLabels)
            {
                Func<double> func = (Func<double>)(l.Tag);
                double val = func();
                l.Content = val.ToString("$0.00");
            }
        }
    }
}
