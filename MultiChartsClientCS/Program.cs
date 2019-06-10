using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            List<string> dateList = new List<string>();
            List<string> dataList = new List<string>();
            using (var rd = new StreamReader("C:\\Users\\magic\\Jupyter Notebooks\\MultiCharts\\input\\abc_k.csv"))
            {
                while (!rd.EndOfStream)
                {
                    var splits = rd.ReadLine().Split(',');
                    dateList.Add(splits[0]);
                    dataList.Add(splits[1]);
                }
            }

            dateList.RemoveAt(0);
            dataList.RemoveAt(0);

            TextWriter tw = new StreamWriter("export.txt");
            foreach (String s in dataList)
                tw.WriteLine(s);
            tw.Close();

            int resultSize = 1000; // must be greater than rnn window(60)
            double[] input = Array.ConvertAll(dataList.Take(resultSize).ToArray(), new Converter<string, double>(Double.Parse));

            multiCharts.SetTrainingData(input, input.Length);
            const int dateWidth = 10;
            int dateArraySize = resultSize* dateWidth;

            char[] dateArray = new char[dateArraySize];
            string[] dateArrayString = dateList.Take(resultSize).ToArray();

            long[] unixDateArray = new long[dateArrayString.Length];
            for(int i = 0; i < dateArrayString.Length; i++)
            {
                unixDateArray[i] = (Int64)(DateTime.Parse(dateArrayString[i]).Subtract(new DateTime(1970,1,1,5,30,0))).TotalSeconds;
            }

            Console.WriteLine(unixDateArray.Length);
            Console.WriteLine(unixDateArray[0]);
            Console.WriteLine(input[1]);
            Console.WriteLine(input.Length);
            Console.WriteLine(dateArrayString[0].Length);
            multiCharts.SetDateArrayUNIX(unixDateArray);

            String fileName = "modelLSTM";
            Console.WriteLine(fileName);        
            multiCharts.SetFileName(fileName);

            multiCharts.SetEpochs(2);
            multiCharts.SetLearningRate(0.001);
            multiCharts.SetScale(100);
            multiCharts.SetOptimizer(0);
            multiCharts.SetMomentum(10);

            Console.WriteLine("TRAIN");
            double res = multiCharts.TrainModel();
            Console.WriteLine(res);

            /*
            SetTestingWeight(multiCharts, 0.3);
            
            int testSize = 100; // must be greater than rnn window(60)
            double[] testSet = Array.ConvertAll(dataList.Skip(resultSize).Take(testSize).ToArray(), new Converter<string, double>(double.Parse));
            
            InitTestingData(multiCharts, testSize);
            SetTestingData(multiCharts, testSet);

            int testDateArraySize = testSize * dateWidth;

            char[] testDateArray = new char[testDateArraySize];
            string[] testDateArrayString = dateList.Skip(resultSize).Take(testSize).ToArray();
            
            for (int i = 0; i < testDateArraySize; i += dateWidth)
            {
                char[] date = testDateArrayString[i / dateWidth].ToCharArray();
                for (int j = 0; j < dateWidth; j++)
                {
                    testDateArray[i + j] = date[j];
                }
            }
            
            InitTestDateArray(multiCharts, testSize);
            SetTestDateArray(multiCharts, testDateArray);

            Console.WriteLine("TEST");
            Console.WriteLine(TestModel(multiCharts));
            
            int ticks = 5;
            double[] predictions = new double[ticks];

            Console.WriteLine("PREDICT");
            IntPtr forecastPointer = Predict(multiCharts, ticks);
            if (Predict(multiCharts, ticks) != null)
            {
                Marshal.Copy(forecastPointer, predictions, 0, ticks);
            }

            for(int i = 0; i < ticks; i++)
            {
                Console.WriteLine(predictions[i]);
            }
            */
            //multiCharts = IntPtr.Zero;
            pt.Stop();

            Console.WriteLine("Duration : " +  pt.Duration.ToString() + 's');
        }
    }
}