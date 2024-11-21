namespace PackagingGenetic;

using System.Diagnostics.Metrics;
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
    public Population(int Count_1x1, int Count_2x2, int Count_3x3, int PopulationSize = POPULATION_SIZE)
    {
        Guys = [];
        for (int i = 0; i < PopulationSize; i++) {
            Guys.Add(new Genotype(Count_1x1, Count_2x2, Count_3x3));
        }
    }

    public void Reproduction(int ChildrenCount)
    {
        ChildrenCount = Math.Min(ChildrenCount, Guys.Count / 2);
        var Tasks = new Task<Genotype>[ChildrenCount];

        for (int i = 0; i < ChildrenCount; i++) {
            Tasks[i] = Task.Factory.StartNew(() => new Genotype(Guys[i], Guys[Guys.Count - 1 - i]));
        }
        Task.WaitAll(Tasks);
        for (int i = 0; i < ChildrenCount; i++) {
            Genotype Child = Tasks[i].Result;
            if (Child.CheckCorrectness())
            {
                Guys.Add(Child);
            }
        }
    }

    public void ReproductionSolo(int ChildrenCount)
    {
        ChildrenCount = Math.Min(ChildrenCount, Guys.Count / 2);

        for (int i = 0; i < ChildrenCount; i++)
        {
            Genotype Child = new Genotype(Guys[i], Guys[Guys.Count - 1 - i]);
            if (Child.CheckCorrectness())
            {
                Guys.Add(Child);
            }
        }
    }

    public void Mutation(int MutantCount)
    {
        MutantCount = Math.Min(MutantCount, Guys.Count);
        var Tasks = new Task<Genotype>[MutantCount];
        for (int i = 0; i < MutantCount; i++)
        {
            Tasks[i] = Task.Factory.StartNew(() => new Genotype(Guys[i]));
        }
        Task.WaitAll(Tasks);
        for (int i = 0; i < MutantCount; i++) {
            Guys.Add(Tasks[i].Result); 
        }
    }

    public void MutationSolo(int MutantCount)
    {
        MutantCount = Math.Min(MutantCount, Guys.Count);
        for (int i = 0; i < MutantCount; i++)
        {
            Guys.Add(new Genotype(Guys[i]));
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
