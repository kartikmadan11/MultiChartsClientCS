using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
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

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?TrainModel@MultiCharts@@QEAANXZ")]
        public static extern double TrainModel(IntPtr multiCharts);

        static void Main(string[] args)
        {
            HiPerfTimer pt = new HiPerfTimer();
            pt.Start();
            IntPtr multiCharts = CreateMultiCharts();
            /*
            Random random = new Random();
            int resultSize = 510000;
            double[] result = new double[resultSize];
            double[] input = new double[resultSize];
            long maximum = 132903000000;
            long minimum = 0;
            for (int i = 0; i < resultSize; i++)
            {
                input[i] = random.NextDouble() * (maximum - minimum) + minimum;
            }
            InitDoubleArray(multiCharts, resultSize);
            SetDoubleArray(multiCharts, input);
            IntPtr arrayPointer = GetDoubleArray(multiCharts);
            Marshal.Copy(arrayPointer, result, 0, resultSize);
            using (StreamWriter sr = new StreamWriter("arrayDataFile.txt"))
            {
                int i = 0;
                foreach (var arrayElement in result)
                {
                    sr.Write(++i);
                    sr.Write(" ");
                    sr.WriteLine(arrayElement);
                }
            }

            int stringSize = 10;
            char[] charStringData = new char[stringSize];
            InitStringData(multiCharts, stringSize);
            SetStringData(multiCharts, "Helloworld".ToCharArray());
            IntPtr stringDataPointer = GetStringData(multiCharts);
            string stringData = Marshal.PtrToStringAnsi(stringDataPointer, stringSize);
            Console.WriteLine(stringData);

            int pythonStringSize = 5;
            char[] pythonCharStringData = new char[pythonStringSize];
            InitPythonStringData(multiCharts, pythonStringSize);
            SetPythonStringData(multiCharts, "0000".ToCharArray());
            IntPtr pythonStringDataPointer = GetPythonStringData(multiCharts);
            string pythonStringData = Marshal.PtrToStringAnsi(pythonStringDataPointer, pythonStringSize);
            Console.WriteLine(pythonStringData);
            */

            InitTrainingData(multiCharts, 10);
            SetTrainingData(multiCharts, new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10.012 });

            Console.WriteLine(TrainModel(multiCharts));

            //DisposeMultiCharts(multiCharts);
            multiCharts = IntPtr.Zero;
            pt.Stop();
            Console.WriteLine(pt.Duration);
        }
    }
}