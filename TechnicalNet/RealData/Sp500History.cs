using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TechnicalNet.RealData
{
    public class Sp500History : StockHistorySet
    {
        public Sp500History()
        {
            using (StreamReader sr = new StreamReader(".\\..\\..\\..\\data\\sp500hst.txt"))
            {
                string csv = sr.ReadToEnd();
                AllStocks = StockHistory.BuildCollection(csv);
            }
        }
    }
}
