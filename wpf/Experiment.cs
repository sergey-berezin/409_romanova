using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfGenetic
{
    public class Experiment(int iterCnt = -1, int count1x1 = -1, int count2x2 = -1, int count3x3 = -1, int square = -1,
        int populationSize = -1, string exName = "", string exFileName = "")
    {
        public int IterCnt { get; set; } = iterCnt;
        public int Count1x1 { get; set; } = count1x1;
        public int Count2x2 { get; set; } = count2x2;
        public int Count3x3 { get; set; } = count3x3;
        public int Square { get; set; } = square;
        public int PopulationSize { get; set; } = populationSize;
        public string ExName { get; set; } = exName;
        public string ExFileName { get; set; } = exFileName;
    }
}
