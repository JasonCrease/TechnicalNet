using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechNetConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TechnicalNet.RealData.StockHistorySet technicalNet = new TechnicalNet.RealData.Sp500History();
        }
    }
}
