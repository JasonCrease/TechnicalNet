﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet.Metrics
{
    public class RandomMetric : IMetric
    {
        public double Val { get; set; }

        public void Analyse(HistoricalData data)
        {
            Val = (new System.Random()).NextDouble() - 0.5D;
        }

        public void Render(Graph g)
        {
            throw new NotImplementedException();
        }
    }
}
