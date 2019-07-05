using MultiChartsCppWrapper;
using System;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using Win32;

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

            MultiChartsWrapper multiChartsWrapper = new MultiChartsWrapper();

            string json;
            using(var rd = new StreamReader(args[0] + ".json"))
            {
                json = rd.ReadToEnd();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            MultiCharts multiCharts = serializer.Deserialize<MultiCharts>(json);

            if(multiCharts.action == "train")
            {
                Console.WriteLine("Feature Length: " + multiCharts.data.Length);
                Console.WriteLine("FileName: " + multiCharts.fileName);
                Console.WriteLine("Epochs: " + multiCharts.epochs);
                Console.WriteLine("Learning Rate: " + multiCharts.learningRate);
                Console.WriteLine("Momentum: " + multiCharts.momentum);
                Console.WriteLine("Scale: " + multiCharts.scale);
                Console.WriteLine("Optimizer Used: " + (multiCharts.optimizer == 0 ? "RMSProp" : "Adam"));
                Console.WriteLine("Testing Part : " + multiCharts.testingPart + '%');
                Console.WriteLine("Testing Weight : " + multiCharts.testingWeight + '%');

                int testingSize = (int)(multiCharts.testingPart / 100 * multiCharts.data.Length);
                int trainingSize = multiCharts.data.Length - testingSize;

                multiChartsWrapper.SetTrainingData(multiCharts.data.Take(trainingSize).ToArray());
                multiChartsWrapper.SetDateArrayUNIX(multiCharts.date.Take(trainingSize).ToArray());
                multiChartsWrapper.SetFileName(multiCharts.fileName);
                multiChartsWrapper.SetEpochs(multiCharts.epochs);
                multiChartsWrapper.SetLearningRate(multiCharts.learningRate);
                multiChartsWrapper.SetMomentum(multiCharts.momentum);
                multiChartsWrapper.SetScale(multiCharts.scale);
                multiChartsWrapper.SetOptimizer(multiCharts.optimizer);

                Console.WriteLine("Training the model on " + trainingSize + " elements");
                Console.WriteLine(multiChartsWrapper.TrainModel());

                multiChartsWrapper.SetTestingData(multiCharts.data.Skip(trainingSize).Take(testingSize).ToArray());
                multiChartsWrapper.SetTestDateArrayUNIX(multiCharts.date.Skip(trainingSize).Take(testingSize).ToArray());

                Console.WriteLine("Testing the trained model on " + testingSize + " elements");
                Console.WriteLine(multiChartsWrapper.TestModel());
            }

            else if(multiCharts.action == "evaluate")
            {

            }

            Console.WriteLine("Length of args: " + args[0].Length);           
            
            pt.Stop();
            Console.WriteLine("Duration : " + pt.Duration.ToString() + 's');
            Console.Write("Press any key to exit: ");
            Console.ReadKey();
        }
    }
}