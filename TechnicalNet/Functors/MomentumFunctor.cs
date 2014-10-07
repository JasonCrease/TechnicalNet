using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.Functors
{
    public class TenDaysTangentFunctor : TangentFunctor
    {
        public override string Name { get { return "10 day tangent"; } }
        protected override int N { get { return 10; } }
    }
    public class HundredDaysTangentFunctor : TangentFunctor
    {
        public override string Name { get { return "100 day tangent"; } }
        protected override int N { get { return 100; } }
    }

    // This just draw a straight line and gets the tangent.
    public abstract class TangentFunctor : IFunctor
    {
        public abstract string Name { get; }
        protected abstract int N { get; }
        public double Val
        {
            get;
            private set;
        }

        public void Analyse(TechnicalNet.RealData.StockHistory data)
        {
            int lastDay = 150;
            double finalClose = data.Closes[lastDay];
            double fromAgoClose = data.Closes[lastDay - N];
            double ratio = (((finalClose - fromAgoClose) * 100) / fromAgoClose) / N;

            Val = Math.Tanh(ratio);
        }
    }
}
