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

    public class Datum
    {
        public double Open, Close, High, Low, Volume;
        public DateTime Date;

        public Datum(DateTime date, double open, double close, double high, double low, double volume)
        {
            this.Open = open;
            this.Close = close;
            this.High = high;
            this.Low = low;
            this.Volume = volume;
            this.Date = date;

        }
    }

    public class StockHistory
    {
        public double[] Opens;
        public double[] Closes;
        public double[] Highs;
        public double[] Lows;
        public double[] Volumes;
        public List<Datum> Datums;
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
                        history.Name = currentShare;
                        history.Count = t;
                        history.StartDate = ParseDate(bits[0]);
                        history.Closes = history.Datums.Select(x => x.Close).ToArray();
                        history.Highs = history.Datums.Select(x => x.High).ToArray();
                        history.Lows = history.Datums.Select(x => x.Low).ToArray();
                        history.Opens = history.Datums.Select(x => x.Open).ToArray();
                        history.Volumes = history.Datums.Select(x => x.Volume).ToArray();
                    }

                    t = 0;
                    history = new StockHistory();
                    history.Datums = new List<Datum>();
                    currentShare = bits[1];
                    list.Add(history);
                }

                history.EndDate = ParseDate(bits[0]);
                history.Datums.Add(new Datum(ParseDate(bits[0]), double.Parse(bits[2]), double.Parse(bits[5]),
                    double.Parse(bits[3]), double.Parse(bits[4]), double.Parse(bits[6])));
                t++;
            }

            // TODO: Load final share

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
