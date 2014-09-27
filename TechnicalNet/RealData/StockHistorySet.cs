using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.RealData
{
    public abstract class StockHistorySet
    {
        public List<StockHistory> AllStocks { get; set; }
    }
}
