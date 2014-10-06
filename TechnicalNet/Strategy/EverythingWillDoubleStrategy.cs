﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.Strategy
{
    public class EverythingWillDoubleStrategy : AbstractStrategy
    {
        public override string Name
        {
            get { return "Everything doubles"; }
        }

        public override double PredictValue(TechnicalNet.RealData.StockHistory stockHistory, int today, int daysInFuture)
        {
            double todayClose = stockHistory.Closes[today];

            return todayClose * 2;
        }
    }
}
