using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Win32;

namespace MultiChartsClientCS
{
    class Program
    {
        private const string dllAddress = "C:\\Users\\magic\\source\\repos\\MultiChartsDLL\\x64\\Release\\MultiChartsDLL.dll";

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0MultiCharts@@QEAA@XZ")]
        public static extern IntPtr CreateMultiCharts();

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "??1MultiCharts@@QEAA@XZ")]
        public static extern void DisposeMultiCharts(IntPtr multiCharts);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InitTrainingData@MultiCharts@@QEAAXH@Z")]
        public static extern void InitTrainingData(IntPtr multiCharts, int size);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetTrainingData@MultiCharts@@QEAAXPEAN@Z")]
        public static extern void SetTrainingData(IntPtr multiCharts, double[] trainingData);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InitTestingData@MultiCharts@@QEAAXH@Z")]
        public static extern void InitTestingData(IntPtr multiCharts, int size);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetTestingData@MultiCharts@@QEAAXPEAN@Z")]
        public static extern void SetTestingData(IntPtr multiCharts, double[] testingData);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InitDateArray@MultiCharts@@QEAAXH@Z")]
        public static extern double InitDateArray(IntPtr multiCharts, int size);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetDateArray@MultiCharts@@QEAAXPEAD@Z")]
        public static extern void SetDateArray(IntPtr multiCharts, char[] dateArray);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InitTestDateArray@MultiCharts@@QEAAXH@Z")]
        public static extern double InitTestDateArray(IntPtr multiCharts, int size);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetTestDateArray@MultiCharts@@QEAAXPEAD@Z")]
        public static extern void SetTestDateArray(IntPtr multiCharts, char[] testDateArray);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InitFileName@MultiCharts@@QEAAXH@Z")]
        public static extern double InitFileName(IntPtr multiCharts, int size);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetFileName@MultiCharts@@QEAAXPEAD@Z")]
        public static extern void SetFileName(IntPtr multiCharts, char[] fileName);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetEpochs@MultiCharts@@QEAAXH@Z")]
        public static extern void SetEpochs(IntPtr multiCharts, int epochs);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetScale@MultiCharts@@QEAAXH@Z")]
        public static extern void SetScale(IntPtr multiCharts, int scale);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetLearningRate@MultiCharts@@QEAAXN@Z")]
        public static extern void SetLearningRate(IntPtr multiCharts, double learningRate);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetOptimizer@MultiCharts@@QEAAXH@Z")]
        public static extern void SetOptimizer(IntPtr multiCharts, int optimizer);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetMomentum@MultiCharts@@QEAAXH@Z")]
        public static extern void SetMomentum(IntPtr multiCharts, int momentum);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetTestingWeight@MultiCharts@@QEAAXN@Z")]
        public static extern void SetTestingWeight(IntPtr multiCharts, double testingWeight);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?TrainModel@MultiCharts@@QEAANXZ")]
        public static extern double TrainModel(IntPtr multiCharts);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?TestModel@MultiCharts@@QEAANXZ")]
        public static extern double TestModel(IntPtr multiCharts);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Predict@MultiCharts@@QEAAPEANH@Z")]
        public static extern IntPtr Predict(IntPtr multiCharts, int ticks);

        static void Main(string[] args)
        {
            
            HiPerfTimer pt = new HiPerfTimer();
            pt.Start();
            IntPtr multiCharts = CreateMultiCharts();

            var dateList = new List<string>();
            var dataList = new List<string>();
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

            int resultSize = 2000; // must be greater than rnn window(60)
            double[] input = Array.ConvertAll(dataList.Take(resultSize).ToArray(), new Converter<string, double>(Double.Parse));

            InitTrainingData(multiCharts, resultSize);
            SetTrainingData(multiCharts, input);

            const int dateWidth = 10;
            int dateArraySize = resultSize* dateWidth;

            char[] dateArray = new char[dateArraySize];
            string[] dateArrayString = dateList.Take(resultSize).ToArray();

            for (int i = 0; i < dateArraySize; i += dateWidth)
            {
                char[] date = dateArrayString[i/dateWidth].ToCharArray();
                for (int j = 0; j < dateWidth; j++)
                {
                    dateArray[i + j] = date[j];
                }
            }

            InitDateArray(multiCharts, resultSize);
            SetDateArray(multiCharts, dateArray);
            
            char[] fileName = "modelLSTM".ToCharArray();
            InitFileName(multiCharts, fileName.Length);
            Console.WriteLine(fileName);
            SetFileName(multiCharts, fileName);

            SetEpochs(multiCharts, 20);
            SetLearningRate(multiCharts, 0.001);
            SetScale(multiCharts, 100);
            SetOptimizer(multiCharts, 0);
            SetMomentum(multiCharts, 10);

            Console.WriteLine("TRAIN");
            Console.WriteLine(TrainModel(multiCharts));
            
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

            DisposeMultiCharts(multiCharts);
            multiCharts = IntPtr.Zero;
            pt.Stop();

            Console.WriteLine("Duration : " +  pt.Duration.ToString() + 's');
        }
    }
}