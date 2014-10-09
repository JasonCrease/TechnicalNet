using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalNet
{
    public class OldNeural
    {
        private BackPropNeuralNet m_Bnn;
        private Random rnd = new Random();

        public void Configure()
        {
            int numInput = 7;
            int numHidden = 6;
            int numOutput = 1;
            int numWeights = (numInput * numHidden) + (numHidden * numOutput) + (numHidden + numOutput);

            Console.WriteLine("Creating a " + numInput + "-input, " + numHidden + "-hidden, " + numOutput + "-output neural network");
            Console.WriteLine("Using hard-coded tanh function for hidden layer activation");
            Console.WriteLine("Using hard-coded log-sigmoid function for output layer activation");

            m_Bnn = new BackPropNeuralNet(numInput, numHidden, numOutput);

            Console.WriteLine("\nGenerating random initial weights and bias values");
            double[] initWeights = new double[numWeights];
            for (int i = 0; i < initWeights.Length; ++i)
                initWeights[i] = (rnd.NextDouble() - 0.5d) * 0.1d;

            //Console.WriteLine("\nInitial weights and biases are:");
            //Helpers.ShowVector(initWeights, 3, 8, true);

            Console.WriteLine("Loading neural network initial weights and biases into neural network");
            m_Bnn.SetWeights(initWeights);

            double learnRate = 0.03;  // learning rate - controls the maginitude of the increase in the change in weights.
            double momentum = learnRate / 10d; // momentum - to discourage oscillation.
            Console.WriteLine("Setting learning rate = " + learnRate.ToString("F2") + " and momentum = " + momentum.ToString("F2"));

            int maxEpochs = 8000000;
            double errorThresh = 0.01;
            Console.WriteLine("\nSetting max epochs = " + maxEpochs + " and error threshold = " + errorThresh.ToString("F6"));

            int epoch = 0;
            double error = double.MaxValue;
            Console.WriteLine("\nBeginning training using back-propagation\n");

            while (epoch < maxEpochs) // train
            {
                double result = 0d; // GetGameResult(gameNum);
                double[] predictedResult = m_Bnn.ComputeOutputs(new double[2]);

                m_Bnn.UpdateWeights(new[] { result }, learnRate, momentum);
                ++epoch;

                if (epoch % 20000 == 0)
                {
                    error = 0d; // GetTotalError(m_Bnn, true);

                    if (error < errorThresh)
                    {
                        Console.WriteLine("Found weights and bias values that meet the error criterion at epoch " + epoch);
                        break;
                    }
                    Console.WriteLine("epoch = " + epoch);
                    Console.WriteLine("error = " + error);
                }
            } // train loop

            double[] finalWeights = m_Bnn.GetWeights();
            Console.WriteLine("");
            //Console.WriteLine("Final neural network weights and bias values are:");
            //Helpers.ShowVector(finalWeights, 5, 8, true);
        }


        public double Predict()
        {
            double predictedResult = 0; // CsvParser.RevConv(m_Bnn.ComputeOutputs(whoPlayed)[0]);

            return predictedResult;
        }

    } 

}
