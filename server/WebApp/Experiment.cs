using System.Text.Json.Serialization;
using System.Xml.Linq;
using PackagingGenetic;

namespace WebApp
{
    public class Experiment
    {
        public int GenerationNumber { get; set; }
        public int Square { get; set; }
        public int Cnt1x1 { get; set; }
        public int Cnt2x2 { get; set; }
        public int Cnt3x3 { get; set; }
        public int PopulationSize { get; set; }
        public string ExName { get; set; }

        [JsonIgnore]
        public Population ExPopulation { get; set; }
        public Experiment(int generationNumber, int square, int cnt1x1, int cnt2x2,
            int cnt3x3, int populationSize, string exName, Population exPop)
        {
            GenerationNumber = generationNumber;
            Square = square;
            Cnt1x1 = cnt1x1;
            Cnt2x2 = cnt2x2;
            Cnt3x3 = cnt3x3;
            PopulationSize = populationSize;
            ExName = exName;
            ExPopulation = new(exPop);
        }
        public Experiment(int cnt1, int cnt2, int cnt3, int popSize, List<string>? AllNames)
        {
            PopulationSize = popSize;
            Cnt1x1 = cnt1;
            Cnt2x2 = cnt2;
            Cnt3x3 = cnt3;
            GenerationNumber = 0;
            ExName = "ex" + Number(AllNames);
            ExPopulation = new(cnt1, cnt2, cnt3, popSize);
            Square = ExPopulation.FirstGen().Square;
        }
        public Experiment()
        {
            ExName = "default";
            ExPopulation = new();
        }
        public void Step()
        {
            ExPopulation.Reproduction(PopulationSize / 2);
            ExPopulation.Mutation(PopulationSize / 3);
            Population NewPop = ExPopulation.Selection(PopulationSize);
            ExPopulation = new Population(NewPop);
            Square = ExPopulation.FirstGen().Square;
            GenerationNumber++;
        }
        public int Number(List<string>? AllNames)
        {
            if (AllNames == null)
            {
                return 0;
            }
            int i = 0;
            while (AllNames.Contains("ex" + i.ToString())) { i++; }
            return i;
        }
    }
}
