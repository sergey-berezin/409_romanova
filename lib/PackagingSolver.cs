using System;

namespace PackagingGenetic;

using static Constants;

public class PackagingSolver
{
    static bool TimeToStop = false;   
    public static Tuple<int, int> GeneticAlgorithm(int Count_1x1, int Count_2x2, int Count_3x3)
    {
        Console.CancelKeyPress += new ConsoleCancelEventHandler(StopHandler);
        int BestSquare = -1;
        int i = 0;
        Population StartPop = new(Count_1x1, Count_2x2, Count_3x3);
        while (true) {
            i++;
            StartPop.Reproduction(POPULATION_SIZE / 2); //magic const
            StartPop.Mutation(POPULATION_SIZE / 3); //magic const
            Population NewPop = StartPop.Selection(POPULATION_SIZE);
            StartPop = new Population(NewPop);
            BestSquare = StartPop.FirstGen().Square;
            if (TimeToStop) {
                break;
            }
        }
        return new Tuple<int, int>(BestSquare, i);
    }
    protected static void StopHandler(object sender, ConsoleCancelEventArgs args)
    {
        TimeToStop = true;
        args.Cancel = true;
    }
    
}
