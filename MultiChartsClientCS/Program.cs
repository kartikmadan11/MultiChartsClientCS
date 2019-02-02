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
            
            [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetStringData@MultiCharts@@QEAA?AV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@XZ")]
            public static extern string GetStringData(IntPtr multiCharts);

            [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetStringData@MultiCharts@@QEAAXV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@@Z")]
            public static extern void SetStringData(IntPtr multiCharts, string stringData);

            [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InitDoubleArray@MultiCharts@@QEAAXH@Z")]
            public static extern void InitDoubleArray(IntPtr multiCharts, int size);

            [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetDoubleArray@MultiCharts@@QEAAXPEAN@Z")]
            public static extern string SetDoubleArray(IntPtr multiCharts, double[] doubleArray);

            [DllImport(dllAddress, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetDoubleArray@MultiCharts@@QEAAPEANXZ")]
            public static extern IntPtr GetDoubleArray(IntPtr multiCharts);
            
            static void Main(string[] args)
            {
                IntPtr multiCharts = CreateMultiCharts();
                double[] result = new double[10];
                int resultSize = 10;
                InitDoubleArray(multiCharts, resultSize);
                SetDoubleArray(multiCharts, new double[] {1,2.2112,2,3,3,4,5,5,5,3});
                IntPtr pointer = GetDoubleArray(multiCharts);
                Marshal.Copy(pointer,result,0,10);
                for(int i = 0; i < resultSize; i++)
                {
                    Console.WriteLine(result[i]);
                }
                DisposeMultiCharts(multiCharts);
                multiCharts = IntPtr.Zero;
            }
        }
    }