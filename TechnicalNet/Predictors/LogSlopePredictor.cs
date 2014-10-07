﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.Predictor
{
    public class LogSlopePredictor : AbstractPredictor
    {
        public override string Name
        {
            get { return "Log slope"; }
        }

        public override double PredictValue(TechnicalNet.RealData.StockHistory stockHistory, int today, int daysInFuture)
        {
            int N = Math.Min(daysInFuture, today - 1);

            double todayCloseL = Math.Log(stockHistory.Closes[today]);
            double fromAgoCloseL = Math.Log(stockHistory.Closes[today - N]);
            double slope = (todayCloseL - fromAgoCloseL) / (double)N;

            return Math.Exp(todayCloseL + (slope * (double)daysInFuture));
        }
    }
}