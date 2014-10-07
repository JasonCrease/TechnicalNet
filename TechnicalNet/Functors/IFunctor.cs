using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.Functors
{
    public interface IFunctor
    {
        double Val { get; }
        void Analyse(TechnicalNet.RealData.StockHistory data);
        string Name { get; }
    }
}
