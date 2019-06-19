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
            MultiChartsWrapper multiCharts = new MultiChartsWrapper();

            if (args.Length == 0)
            {
                Console.WriteLine("No command line arguments provided");
                Environment.Exit(0);
            }

            Console.WriteLine("Length of args: " + args[0].Length);
            string[] splitArgs = args[0].Split(';');
            Console.WriteLine("Command Line Args: " + splitArgs.Length);

            string[] t_data_str = splitArgs[0].Split(',');
            double[] t_data = new double[t_data_str.Length];
            t_data = Array.ConvertAll(t_data_str, new Converter<string, double>(Double.Parse));
            
            string[] t_date_str = splitArgs[1].Split(',');
            long[] t_date = new long[t_data_str.Length];
            t_date = Array.ConvertAll(t_date_str, new Converter<string, long>(Int64.Parse));

            string fileName = splitArgs[2];
            int epochs = int.Parse(splitArgs[3]);
            double learningRate = double.Parse(splitArgs[4]);
            int momentum = int.Parse(splitArgs[5]);
            int scale = int.Parse(splitArgs[6]);    
            int optimizer = int.Parse(splitArgs[7]);
            double testingPart = double.Parse(splitArgs[8]);
            double testingWeight = double.Parse(splitArgs[9]);

            Console.WriteLine("Feature Length: " + t_data.Length);
            Console.WriteLine("FileName: " + fileName);
            Console.WriteLine("Epochs: " + epochs);
            Console.WriteLine("Learning Rate: " + learningRate);
            Console.WriteLine("Momentum: " + momentum);
            Console.WriteLine("Scale: " + scale);
            Console.WriteLine("Optimizer Used: " + (optimizer==0?"RMSProp":"Adam"));
            Console.WriteLine("Testing Part : " + testingPart);
            Console.WriteLine("Testing Weight : " + testingWeight);

            int trainingSize = (int)(testingPart / 100 * t_data.Length);
            int testingSize = t_data.Length - trainingSize;

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

            Console.WriteLine("Testing the trained model on " + testingSize + "elements");
            Console.WriteLine(multiCharts.TestModel());

            pt.Stop();
            Console.WriteLine("Duration : " +  pt.Duration.ToString() + 's');
            Console.Write("Press any key to exit: ");
            Console.ReadKey();
        }
    }
}