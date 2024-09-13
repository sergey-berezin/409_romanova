namespace PackagingGenetic;

using static Constants;

public class Population
{
    public static int GenotypeCMP(Genotype A, Genotype B)
    {
        return A.Square.CompareTo(B.Square);
    }
    public List<Genotype> Guys { get; set; }

    public Population(Population PopCopy)
    {
        Guys = [];
        foreach(Genotype Guy in PopCopy.Guys) {
            this.Add(Guy);
        }
    }
    public Population()
    {
        Guys = [];
    }
    public Population(int Count_1x1, int Count_2x2, int Count_3x3)
    {
        Guys = [];
        for (int i = 0; i < POPULATION_SIZE; i++) {
            Guys.Add(new Genotype(Count_1x1, Count_2x2, Count_3x3));
        }
    }
    public void Reproduction(int ChildrenCount)
    {
        int i = 0;
        int StopCnt = 0;
        ChildrenCount = Math.Min(ChildrenCount, Guys.Count / 2);
        while (ChildrenCount > 0 && StopCnt < RANDOM_STOP_CNT) { 
            Genotype Child = new Genotype(Guys[i], Guys[i + 1]);
            if (Child.CheckCorrectness()) {
                Guys.Add(Child);
                i += 2;
                ChildrenCount--;
            }
            StopCnt++;
        }
    }
    public void Mutation(int MutantCount)
    {
        MutantCount = Math.Min(MutantCount, Guys.Count);
        for (int i = 0; i < MutantCount; i++) {
            Guys.Add(new Genotype(Guys[i]));
            i++;
        }
    }
    public Population Selection(int PopulationSize = POPULATION_SIZE)
    {
        Population CoolGuys = new();
        Guys.Sort(GenotypeCMP);
        for (int i = 0; i < PopulationSize; i++) {
            CoolGuys.Add(Guys[i]);
        }
        return CoolGuys;
    }
    public string StringPopulation()
    {
        string PopStr = "";
        foreach (Genotype Guy in Guys) {
            PopStr += Guy.StringGenotype();
        }
        return PopStr;
    }
    public Genotype FirstGen()
    {
        return Guys[0];
    }
    public void Add(Genotype Guy) 
    {
        Guys.Add(Guy);
    }
}
