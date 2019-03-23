using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using Win32;
using System.Threading;

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

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InitDateArray@MultiCharts@@QEAAXH@Z")]
        public static extern double InitDateArray(IntPtr multiCharts, int size);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetDateArray@MultiCharts@@QEAAXPEAD@Z")]
        public static extern void SetDateArray(IntPtr multiCharts, char[] dateArray);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?TrainModel@MultiCharts@@QEAANXZ")]
        public static extern double TrainModel(IntPtr multiCharts);

        static void Main(string[] args)
        {
            HiPerfTimer pt = new HiPerfTimer();
            pt.Start();
            IntPtr multiCharts = CreateMultiCharts();
         
            Random random = new Random();
            int resultSize = 600;
            double[] input = new double[resultSize];
            double sum = 0;
            long maximum = 132903000000;
            long minimum = 0;
            for (int i = 0; i < resultSize; i++)
            {
                input[i] = random.NextDouble() * (maximum - minimum) + minimum;
                sum += input[i];
            }
            
            InitTrainingData(multiCharts, resultSize);
            SetTrainingData(multiCharts, input);

            const int dateWidth = 20;
            int dateArraySize = resultSize* dateWidth;

            char[] dateArray = new char[dateArraySize];
            DateTime dt = DateTime.Now;
            Console.WriteLine(dt.ToString().Length);
            for (int i = 0; i < dateArraySize; i+=dateWidth)
            {
                char[] date = dt.ToString().ToCharArray();
                for(int j = 0; j < dateWidth; j++)
                {
                    dateArray[i+j] = date[j];
                }
                dt = dt.AddMilliseconds(2000);
            }
            Console.WriteLine(dt.ToString());
            Console.WriteLine(dateArraySize);
            
            InitDateArray(multiCharts, resultSize);
            SetDateArray(multiCharts, dateArray);

            Console.WriteLine(TrainModel(multiCharts));

            DisposeMultiCharts(multiCharts);
            multiCharts = IntPtr.Zero;
            pt.Stop();
            Console.WriteLine(pt.Duration);
        }
    }
}