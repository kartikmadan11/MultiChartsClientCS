using System;
using Win32;
using MultiChartsCppWrapper;
using System.Linq;

namespace MultiChartsClientCS
{
    class Program
    {
        static void Main(string[] args)
        {
            HiPerfTimer pt = new HiPerfTimer();
            pt.Start();

            if (args.Length == 0)
            {
                Console.WriteLine("No command line arguments provided");
                Environment.Exit(0);
            }

            MultiChartsWrapper multiCharts = new MultiChartsWrapper();

            Console.WriteLine(args[0]);
            Console.WriteLine("Length of args: " + args[0].Length);
            string[] splitArgs = args[0].Split(';');
            Console.WriteLine("Command Line Args: " + splitArgs.Length);

            if (splitArgs[0] == "train")
            {

                string[] t_data_str = splitArgs[1].Split(',');
                double[] t_data = new double[t_data_str.Length];
                t_data = Array.ConvertAll(t_data_str, new Converter<string, double>(Double.Parse));

                string[] t_date_str = splitArgs[2].Split(',');
                long[] t_date = new long[t_data_str.Length];
                t_date = Array.ConvertAll(t_date_str, new Converter<string, long>(Int64.Parse));

                string fileName = splitArgs[3];
                int epochs = int.Parse(splitArgs[4]);
                double learningRate = double.Parse(splitArgs[5]);
                double momentum = double.Parse(splitArgs[6]);
                int scale = int.Parse(splitArgs[7]);
                int optimizer = int.Parse(splitArgs[8]);
                double testingPart = double.Parse(splitArgs[9]);
                double testingWeight = double.Parse(splitArgs[10]);

                Console.WriteLine("Feature Length: " + t_data.Length);
                Console.WriteLine("FileName: " + fileName);
                Console.WriteLine("Epochs: " + epochs);
                Console.WriteLine("Learning Rate: " + learningRate);
                Console.WriteLine("Momentum: " + momentum);
                Console.WriteLine("Scale: " + scale);
                Console.WriteLine("Optimizer Used: " + (optimizer == 0 ? "RMSProp" : "Adam"));
                Console.WriteLine("Testing Part : " + testingPart + '%');
                Console.WriteLine("Testing Weight : " + testingWeight + '%');

                int testingSize = (int)(testingPart / 100 * t_data.Length);
                int trainingSize = t_data.Length - testingSize;

                multiCharts.SetTrainingData(t_data.Take(trainingSize).ToArray());
                multiCharts.SetDateArrayUNIX(t_date.Take(trainingSize).ToArray());
                multiCharts.SetFileName(fileName);
                multiCharts.SetEpochs(epochs);
                multiCharts.SetLearningRate(learningRate);
                multiCharts.SetMomentum(momentum);
                multiCharts.SetScale(scale);
                multiCharts.SetOptimizer(optimizer);

                Console.WriteLine("Training the model on " + trainingSize + " elements");
                Console.WriteLine(multiCharts.TrainModel());

                multiCharts.SetTestingData(t_data.Take(testingSize).ToArray());
                multiCharts.SetTestDateArrayUNIX(t_date.Take(testingSize).ToArray());

                Console.WriteLine("Testing the trained model on " + testingSize + " elements");
                Console.WriteLine(multiCharts.TestModel());
            }

            else if(splitArgs[0] == "eval")
            {
            }

            else if(splitArgs[0] == "forecast")
            {
                int ticks = int.Parse(splitArgs[1]);
                long lastDateTime = long.Parse(splitArgs[2]);
                long dateTimeDiff = long.Parse(splitArgs[3]);
                string fileName = splitArgs[4];

                multiCharts.SetFileName(fileName);
                double[] pred = multiCharts.Predict(ticks);

                if (pred.Length == 0)
                    Console.WriteLine("Predictions not made");
                
                else
                {
                    if(pred != null)
                    {
                        for(int i = 0; i < pred.Length; i++)
                        {
                            Console.Write(new DateTime(1970, 1, 1, 5, 30, 0).AddSeconds(lastDateTime + (i+1)*dateTimeDiff));
                            Console.WriteLine(" : " + pred[i]);
                        }
                    }
                }

            }

            pt.Stop();
            Console.WriteLine("Duration : " + pt.Duration.ToString() + 's');
            Console.Write("Press any key to exit: ");
            Console.ReadKey();
        }
    }
}