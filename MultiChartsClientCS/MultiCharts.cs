using System;

namespace MultiChartsClientCS
{
    public class MultiCharts
    {
        public string action { get; set; }
        public string fileName { get; set; }
        public double[] data { get; set; }
        public double testingPart { get; set; }
        public double testingWeight { get; set; }
        public long[] date { get; set; }
        public bool gpu { get; set; }
        public double learningRate { get; set; }
        public double momentum { get; set; }
        public int optimizer { get; set; }
        public int scale { get; set; }
        public int epochs { get; set; }
        public int ticks { get; set; }
        public long lastDateTime { get; set; }
        public long dateTimeDiff { get; set; }
    }
}
