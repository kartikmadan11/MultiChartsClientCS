using System;
using Win32;
using MultiChartsCppWrapper;

namespace MultiChartsClientCS
{
    class Program
    {
        static void Main(string[] args)
        {
            
            HiPerfTimer pt = new HiPerfTimer();
            pt.Start();
            MultiChartsWrapper multiCharts = new MultiChartsWrapper();

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

            Console.WriteLine("Feature Length: " + t_data.Length);
            Console.WriteLine("FileName: " + fileName);
            Console.WriteLine("Epochs: " + epochs);
            Console.WriteLine("Learning Rate: " + learningRate);
            Console.WriteLine("Momentum: " + momentum);
            Console.WriteLine("Scale: " + scale);
            Console.WriteLine("Optimizer Used: " + (optimizer==0?"RMSProp":"Adam"));

            multiCharts.SetTrainingData(t_data);
            multiCharts.SetDateArrayUNIX(t_date);
            multiCharts.SetFileName(fileName);
            multiCharts.SetEpochs(epochs);
            multiCharts.SetLearningRate(learningRate);
            multiCharts.SetMomentum(momentum);
            multiCharts.SetScale(scale);
            multiCharts.SetOptimizer(optimizer);

            Console.WriteLine("Training the model");
            Console.WriteLine(multiCharts.TrainModel());

            pt.Stop();
            Console.WriteLine("Duration : " +  pt.Duration.ToString() + 's');
            Console.Write("Press any key to exit: ");
            Console.ReadKey();
        }
    }
}