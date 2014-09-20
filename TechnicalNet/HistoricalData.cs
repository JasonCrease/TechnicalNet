using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TechnicalNet
{
    public class HistoricalData
    {
        public List<StockHistory> AllStocks { get; set; }

        public HistoricalData()
        {
            using (StreamReader sr = new StreamReader(".\\..\\..\\..\\data\\sp500hst.txt"))
            {
                string csv = sr.ReadToEnd();
                AllStocks = StockHistory.BuildCollection(csv);
            }
        }
    }

    public class StockHistory
    {
        public double[] Opens;
        public double[] Closes;
        public double[] Highs;
        public double[] Lows;
        public double[] Volumes;
        public DateTime StartDate;
        public DateTime EndDate;
        public int Count;
        public string Name { get; private set; }

        public StockHistory()
        {
        }

        public StockHistory(string csv)
        {
            ParseCsv(csv);
        }

        private void ParseCsv(string csv)
        {

        }

        public static List<StockHistory> BuildCollection(string multipleStocksCsv)
        {
            List<StockHistory> list = new List<StockHistory>();
            string[] lines = multipleStocksCsv.Split('\n');
            string currentShare = "XXXXXXX";
            StockHistory history = null;
            int t = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] bits = lines[i].Split(',');

                if (bits[1] != currentShare)
                {
                    if (history != null)
                    {
                        history.Count = t;
                    }
                    t = 0;

                    history = new StockHistory();
                    history.StartDate = ParseDate(bits[0]);
                    history.Name = bits[1];
                    currentShare = bits[1];
                    history.Closes = new double[250];
                    history.Highs = new double[250];
                    history.Lows = new double[250];
                    history.Opens = new double[250];
                    history.Volumes = new double[250];
                    list.Add(history);
                }
                
                history.EndDate = ParseDate(bits[0]);
                history.Opens[t] = double.Parse(bits[2]);
                history.Highs[t] = double.Parse(bits[3]);
                history.Lows[t] = double.Parse(bits[4]);
                history.Closes[t] = double.Parse(bits[5]);
                history.Volumes[t] = double.Parse(bits[6]);

                t++;
            }

            // Final share
            history.Count = t;

            return list;
        }

        private static DateTime ParseDate(string str)
        {
            return new DateTime(
                int.Parse(str.Substring(0, 4)), 
                int.Parse(str.Substring(4, 2)), 
                int.Parse(str.Substring(6, 2)));
        }
    }
}
