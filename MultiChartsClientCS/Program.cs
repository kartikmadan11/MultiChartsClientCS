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

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetIntNumber@MultiCharts@@QEAAXH@Z")]
        public static extern void SetIntNumber(IntPtr multiCharts, int intNumber);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetIntNumber@MultiCharts@@QEAAHXZ")]
        public static extern int GetIntNumber(IntPtr multiCharts);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetDoubleNumber@MultiCharts@@QEAANXZ")]
        public static extern double GetDoubleNumber(IntPtr multiCharts);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetDoubleNumber@MultiCharts@@QEAAXN@Z")]
        public static extern void SetDoubleNumber(IntPtr multiCharts, double doubleNumber);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InitStringData@MultiCharts@@QEAAXH@Z")]
        public static extern void InitStringData(IntPtr multiCharts, int size);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetStringData@MultiCharts@@QEAAPEADXZ")]
        public static extern IntPtr GetStringData(IntPtr multiCharts);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetStringData@MultiCharts@@QEAAXPEAD@Z")]
        public static extern void SetStringData(IntPtr multiCharts, char[] stringData);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InitDoubleArray@MultiCharts@@QEAAXH@Z")]
        public static extern void InitDoubleArray(IntPtr multiCharts, int size);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetDoubleArray@MultiCharts@@QEAAXPEAN@Z")]
        public static extern void SetDoubleArray(IntPtr multiCharts, double[] doubleArray);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetDoubleArray@MultiCharts@@QEAAPEANXZ")]
        public static extern IntPtr GetDoubleArray(IntPtr multiCharts);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetPythonStringData@MultiCharts@@QEAAPEADXZ")]
        public static extern IntPtr GetPythonStringData(IntPtr multiCharts);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InitPythonStringData@MultiCharts@@QEAAXH@Z")]
        public static extern void InitPythonStringData(IntPtr multiCharts, int size);

        [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetPythonStringData@MultiCharts@@QEAAXPEAD@Z")]
        public static extern void SetPythonStringData(IntPtr multiCharts, char[] pythonStringData);

        static void Main(string[] args)
        {
            HiPerfTimer pt = new HiPerfTimer();
            pt.Start();
            IntPtr multiCharts = CreateMultiCharts();
            pt.Stop();
            Console.WriteLine(pt.Duration);
            Random random = new Random();
            int resultSize = 510000;
            double[] result = new double[resultSize];
            double[] input = new double[resultSize];
            long maximum = 132903000000;
            long minimum = 0;
            for(int i = 0; i < resultSize; i++)
            {
                input[i] = random.NextDouble() * (maximum - minimum) + minimum;
            }
            InitDoubleArray(multiCharts, resultSize);
            SetDoubleArray(multiCharts, input);
            IntPtr arrayPointer = GetDoubleArray(multiCharts);
            Marshal.Copy(arrayPointer, result, 0, resultSize);
            using(StreamWriter sr = new StreamWriter("arrayDataFile.txt"))
            {
                int i = 0;
                foreach(var arrayElement in result)
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

            DisposeMultiCharts(multiCharts);
            multiCharts = IntPtr.Zero;
        }
    }
}