using PackagingGenetic;

class Program
{
    static void Main()
    {
        int N1 = 2, N2 = 2, N3 = 1;
        Tuple<int, int> Answer = PackagingSolver.GeneticAlgorithm(N1, N2, N3);
        Console.WriteLine("Best square: " + Answer.Item1);
        Console.WriteLine("Number of generation: " + Answer.Item2);
    }
}