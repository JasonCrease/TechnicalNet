using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalNet.Predictor;

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

         public void Setup()
        {
            int numInput = 7;
            int numHidden = 6;
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

            double learnRate = 0.03;  // learning rate - controls the maginitude of the increase in the change in weights.
            double momentum = learnRate / 10d; // momentum - to discourage oscillation.
            DebugWrite("Setting learning rate = " + learnRate.ToString("F2") + " and momentum = " + momentum.ToString("F2"));

            int maxEpochs = 800000;
            double errorThresh = 0.01;
            DebugWrite("\nSetting max epochs = " + maxEpochs + " and error threshold = " + errorThresh.ToString("F6"));

            // Train

            int epoch = 0;
            double error = double.MaxValue;
            DebugWrite("\nBeginning training using back-propagation\n");

            while (epoch < maxEpochs) // train
            {
                double result = 0d; // TODO Set real result
                double[] predictedResult = m_Bnn.ComputeOutputs(new double[2]);

                m_Bnn.UpdateWeights(new[] { result }, learnRate, momentum);
                ++epoch;

                if (epoch % 20000 == 0)
                {
                    error = 0d; // GetTotalError(m_Bnn, true);

                    if (error < errorThresh)
                    {
                        DebugWrite("Found weights and bias values that meet the error criterion at epoch " + epoch);
                        break;
                    }
                    DebugWrite("epoch = " + epoch);
                    DebugWrite("error = " + error);
                }
            } // train loop

            double[] finalWeights = m_Bnn.GetWeights();
            DebugWrite("");
            DebugWrite("Final neural network weights and bias values are:");
            Helpers.ShowVector(finalWeights, 5, 8, true);
        }

         public override double PredictValue(RealData.StockHistory stockHistory, int today, int daysInFuture)
         {
             throw new NotImplementedException();
         }

         public override string Name
         {
             get { return "Neural"; }
         }
    }
}
