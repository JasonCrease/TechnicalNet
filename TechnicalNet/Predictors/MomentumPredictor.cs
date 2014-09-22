using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.Predictors
{
    public class TenDaysMomentumPredictor : MomentumPredictor
    {
        public override string Name { get { return "10 day mom"; } }
        protected override int N { get { return 10; } }
    }
    public class HundredDaysMomentumPredictor : MomentumPredictor
    {
        public override string Name { get { return "100 day mom"; } }
        protected override int N { get { return 100; } }
    }

    public abstract class MomentumPredictor : IPredictor
    {
        public abstract string Name { get; }
        protected abstract int N { get; }
        public double Val
        {
            get;
            private set;
        }

        public void Analyse(StockHistory data)
        {
            int lastDay = 150;
            double finalClose = data.Closes[lastDay];
            double fromAgoClose = data.Closes[lastDay - N];
            double ratio = (((finalClose - fromAgoClose) * 100) / fromAgoClose) / N;

            Val = Math.Tanh(ratio);
        }
    }
}
