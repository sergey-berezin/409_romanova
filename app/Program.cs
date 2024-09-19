using PackagingGenetic;

using static PackagingGenetic.Constants;


class Program
{

    static bool TimeToStop = false;

    static void PackagingSolver(int Count_1x1, int Count_2x2, int Count_3x3, int PopulationSize = POPULATION_SIZE, int GenerationCount = MAX_GENERATION_COUNT)
    {
        Console.CancelKeyPress += (sender, args) => 
        {
            TimeToStop = true;
            args.Cancel = true;
        };
        int i = 0;
        Population StartPop = new(Count_1x1, Count_2x2, Count_3x3, PopulationSize);
        while (!TimeToStop)
        {
            i++;
            StartPop.Reproduction(PopulationSize / 2);
            StartPop.Mutation(PopulationSize / 3);
            Population NewPop = StartPop.Selection(PopulationSize);
            StartPop = new Population(NewPop);
            int BestSquare = StartPop.FirstGen().Square;
            Console.WriteLine(StartPop.FirstGen().StringGenotype());
            Console.WriteLine(StartPop.FirstGen().Square);
            Console.WriteLine("Press any key to generate new generation or Ctrl+C to stop");
            Console.ReadKey();
            if (i == GenerationCount)
            {
                TimeToStop = true;
            }
        }
    }
    static void Main()
    {
        int Count_1x1 = 2, Count_2x2 = 2, Count_3x3 = 1;
        PackagingSolver(Count_1x1, Count_2x2, Count_3x3);
    }
}

