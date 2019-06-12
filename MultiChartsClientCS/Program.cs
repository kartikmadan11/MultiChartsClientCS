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

            Console.WriteLine("Length of args: " + args[0].Length);
            string[] splitArgs = args[0].Split(';');
            Console.WriteLine("Command Line Args: " + splitArgs.Length);
        }
    }
}