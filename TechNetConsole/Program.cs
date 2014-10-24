using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalNet.Functors;

namespace TechNetConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TechnicalNet.RealData.StockHistorySet trainingData = new TechnicalNet.RealData.SpnShareHistory(30);
            IFunctor[] functorSet = {
                                       new TenDaysTangentFunctor(),
                                       new HundredDaysTangentFunctor(),
                                       new EmaFunctor()
                                   };
            TechnicalNet.Predictors.NeuralPredictor neuralPredictor = new TechnicalNet.Predictors.NeuralPredictor(x => Console.Write(x));
            neuralPredictor.Setup(functorSet, trainingData);

            Console.WriteLine("Done. Press any key to continue.");
            Console.ReadLine();
        }
    }
}
