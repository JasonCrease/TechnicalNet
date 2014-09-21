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
            m_Data = new TechnicalNet.HistoricalData();
            m_ShareNum = 0;
            UpdateImage();
        }

        public void UpdateImage()
        {
            Graph g = new Graph(m_Data.AllStocks[m_ShareNum], new List<IMetric>());
            img.Source = ToBitmapSource(g.Bitmap);
            LabelShareName.Content = m_Data.AllStocks[m_ShareNum].Name;
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
