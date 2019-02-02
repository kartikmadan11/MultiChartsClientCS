    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Runtime.InteropServices;

    namespace MultiChartsClientCS
    {
        class Program
        {
            private const string dllAddress = "C:\\Users\\magic\\source\\repos\\MultiChartsDLL\\x64\\Debug\\MultiChartsDLL.dll";

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
            
            static void Main(string[] args)
            {
                IntPtr multiCharts = CreateMultiCharts();
                double[] result = new double[10];
                int resultSize = 10;
                InitDoubleArray(multiCharts, resultSize);
                SetDoubleArray(multiCharts, new double[] {0.01,2,112.2,11.2,12.0,1,1,24,45,560});
                IntPtr arrayPointer = GetDoubleArray(multiCharts);
                Marshal.Copy(arrayPointer, result, 0, resultSize);
                for(int i = 0; i < resultSize; i++)
                {
                    Console.WriteLine(result[i]);
                }
                
                int stringSize = 250;
                char[] charStringData = new char[stringSize];
                InitStringData(multiCharts, stringSize);
                SetStringData(multiCharts, "HelloWorld".ToCharArray());
                IntPtr stringPointer = GetStringData(multiCharts);
                Marshal.Copy(stringPointer, charStringData, 0, stringSize);
                string stringData = new string(charStringData);
                for (int i = 0; i < stringSize; i++)
                {
                    Console.Write(charStringData[i]);
                }
                Console.WriteLine("");
                Console.WriteLine(stringData);

                DisposeMultiCharts(multiCharts);
                multiCharts = IntPtr.Zero;
            }
        }
    }