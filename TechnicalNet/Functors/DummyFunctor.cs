using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TechnicalNet.Functors
{
    public class DummyFunctor : IFunctor
    {
        public string Name { get { return "Dummy"; } }
        public double Val { get; private set; }

        public void Analyse(TechnicalNet.RealData.StockHistory stock, int today)
        {
            throw new NotImplementedException();
        }
    }
}
