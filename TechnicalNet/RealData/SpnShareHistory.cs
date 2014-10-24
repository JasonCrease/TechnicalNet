using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TechnicalNet.RealData
{
    public class SpnShareHistory : StockHistorySet
    {
        public SpnShareHistory(int n)
        {
            using (StreamReader sr = new StreamReader(".\\..\\..\\..\\data\\sp500hst.txt"))
            {
                string csv = sr.ReadToEnd();
                AllStockHistories = StockHistory.BuildCollection(csv).GetRange(0, n);
            }
        }
    }
}
