using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalNet.Predictor;
using TechnicalNet.Functors;
using TechnicalNet.RealData;

namespace TechnicalNet.Predictors
{
    public class NeuralPredictor : AbstractPredictor
    {
        private BackPropNeuralNet m_Bnn;
        private Random rnd = new Random();
        private Action<string> DebugWrite;

        public NeuralPredictor(Action<string> debugWrite)
        {
            DebugWrite = debugWrite;
        }

        public override double PredictValue(RealData.StockHistory stockHistory, int today, int daysInFuture)
        {
            throw new NotImplementedException();
        }

        public IFunctor[] Fns;
        private StockHistorySet m_StockHistorySet;

        public override string Name
        {
            get { return "Neural"; }
        }

        public void Setup(IEnumerable<IFunctor> functors, StockHistorySet stockHistorySet)
        {
            Fns = functors.ToArray();
            m_StockHistorySet = stockHistorySet;

            if (this.Fns == null) throw new ApplicationException();
            if (!Fns.Any()) throw new ApplicationException();

            int numInput = Fns.Length;
            int numHidden = 8;
            int numOutput = 1;
            int numWeights = (numInput * numHidden) + (numHidden * numOutput) + (numHidden + numOutput);

            DebugWrite("Creating a " + numInput + "-input, " + numHidden + "-hidden, " + numOutput + "-output neural network");
            DebugWrite("Using hard-coded tanh function for hidden layer activation");
            DebugWrite("Using hard-coded log-sigmoid function for output layer activation");

            m_Bnn = new BackPropNeuralNet(numInput, numHidden, numOutput);

            DebugWrite("\nGenerating random initial weights and bias values");
            double[] initWeights = new double[numWeights];
            for (int i = 0; i < initWeights.Length; ++i)
                initWeights[i] = (rnd.NextDouble() - 0.5d) * 1.0d;
            DebugWrite("Loading neural network initial weights and biases into neural network");
            m_Bnn.SetWeights(initWeights);

            double learnRate = 0.2;  // learning rate - controls the maginitude of the increase in the change in weights.
            double momentum = 0.1; // momentum - to discourage oscillation.
            DebugWrite("Setting learning rate = " + learnRate.ToString("F2") + " and momentum = " + momentum.ToString("F2"));

            int maxEpochs = 8000000;
            double errorThresh = 0.01;
            DebugWrite("\nSetting max epochs = " + maxEpochs + " and error threshold = " + errorThresh.ToString("F6"));

            // Train

            int epoch = 0;
            double error = double.MaxValue;
            DebugWrite("\nBeginning training using back-propagation\n");

            int stocksCount = stockHistorySet.AllStockHistories.Count(); ;

            while (epoch < maxEpochs) // train
            {
                int stockNum = rnd.Next(0, stocksCount);
                var stock = stockHistorySet.AllStockHistories[stockNum];

                double realValue = stock.Closes[Today + DaysInFuture] / stock.Closes[Today];
                double updateValue = Math.Tanh(realValue);

                double predictedOutput = m_Bnn.ComputeOutputs(ComputeInputs(stock))[0];
                double predictedValue = ATanh(predictedOutput) * stock.Closes[Today];

                m_Bnn.UpdateWeights(new[] { updateValue }, learnRate, momentum);
                ++epoch;

                if (epoch % 20000 == 0)
                {
                    error = GetAverageError(true);

                    if (error < errorThresh)
                    {
                        DebugWrite("Found weights and bias values that meet the error criterion at epoch " + epoch);
                        break;
                    }
                    DebugWrite("epoch = " + epoch);
                    DebugWrite(" error = " + error + "\n");
                }
            } // train loop

            double[] finalWeights = m_Bnn.GetWeights();
            DebugWrite("");
            DebugWrite("Final neural network weights and bias values are:");
            Helpers.ShowVector(finalWeights, 5, 8, true);
        }

        public static double ATanh(double x)
        {
            return (Math.Log(1 + x) - Math.Log(1 - x)) / 2;
        }

        private double[] ComputeInputs(StockHistory stock)
        {
            double[] outputs = new double[Fns.Length];

            for (int i = 0; i < Fns.Length; i++)
            {
                Fns[i].Analyse(stock, Today);
                outputs[i] = Fns[i].Val;
            }

            return outputs;
        }

        private double GetAverageError(bool printDebugOutput)
        {
            int count = 0;
            double avgErr = 0;

            foreach (StockHistory stock in m_StockHistorySet.AllStockHistories)
            {
                count++;

                double realValue = stock.Closes[Today + DaysInFuture] / stock.Closes[Today];
                double predictedOutput = m_Bnn.ComputeOutputs(ComputeInputs(stock))[0];
                double predictedValue = ATanh(predictedOutput);

                avgErr += Math.Abs(realValue - predictedValue);
            }

            return avgErr;
        }

        private int Today { get { return 150; } }
        private int DaysInFuture { get { return 50; } }
    }
}
