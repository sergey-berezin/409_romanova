using System;
using System.Collections;

namespace PackagingGenetic;

using static Constants;

public class Square
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Size { get; set; }
    public Square(int x, int y, int size)
    {
        this.X = x;
        this.Y = y;
        this.Size = size;
    }
}

public class Genotype //: IEnumerable
{
    public int[] X { get; set; }
    public int[] Y { get; set; }
    public int[] Size { get; set; }
    public int ItemCount { get; set; }
    public int Square { get; set; }
    public Genotype() { }
    //public Genotype(Square S)
    //{
    //    int n = 10;
    //    X = new int[n];
    //    Y = new int[n];
    //    Size = new int[n];
    //}
    //public Genotype(List<Square> SquareList)
    //{
    //    int n = SquareList.Count;
    //    X = new int[n];
    //    Y = new int[n];
    //    Size = new int[n];
    //}
    public string StringGenotype() 
    {
        string GenStr = "";
        for (int i = 0; i < X.Length; i++) {
            GenStr += string.Format($"X: {X[i], -5} Y: {Y[i], -5} Size: {Size[i]}\n");
        }
        GenStr += "Square: " + Square + "\n";
        return GenStr;
    }
    int CalcSquare()
    {
        int Inf = MAX_FIELD_MULT * ItemCount + MAX_ITEM_SIZE;
        int MinCoordX = Inf, MaxCoordX = 0;
        int MinCoordY = Inf, MaxCoordY = 0;
        for (int i = 0; i < X.Length; i++) {
            MinCoordX = Math.Min(MinCoordX, X[i]);
            MaxCoordX = Math.Max(MaxCoordX, X[i] + Size[i]);
            MinCoordY = Math.Min(MinCoordY, Y[i]);
            MaxCoordY = Math.Max(MaxCoordY, Y[i] + Size[i]);
        }
        return (MaxCoordX - MinCoordX) * (MaxCoordY - MinCoordY);
    }
    public bool CheckCorrectness()
    {
        for (int i = 0; i < ItemCount - 1; i++) {
            for (int j = i + 1; j < ItemCount; j++) {
                int MinCoordX = Math.Min(X[i], X[j]);
                int MinCoordY = Math.Min(Y[i], Y[j]);
                int MaxCoordX = Math.Max(X[i] + Size[i], X[j] + Size[j]);
                int MaxCoordY = Math.Max(Y[i] + Size[i], Y[j] + Size[j]);
                if (MaxCoordX - MinCoordX < Size[i] + Size[j] && MaxCoordY - MinCoordY < Size[i] + Size[j]) {
                    return false;
                }
            }
        }
        return true;
    }

    //public IEnumerator GetEnumerator() => new GenotypeEnumerator(X, Y, Size);

    public List<Tuple<int, int, int>> SquareList()
    {
        List<Tuple<int, int, int>> ListSquare = new();
        for (int i = 0; i < ItemCount; i++)
        {
            ListSquare.Add(new Tuple<int, int, int>(X[i], Y[i], Size[i]));
        }
        return ListSquare;
    }
    public Genotype(int Count_1x1, int Count_2x2, int Count_3x3)
    {
        ItemCount = Count_1x1 + Count_2x2 + Count_3x3;
        Size = new int[ItemCount];
        for (int i = 0; i < Count_1x1; i++) { Size[i] = 1; }
        for (int i = 0; i < Count_2x2; i++) { Size[Count_1x1 + i] = 2; }
        for (int i = 0; i < Count_3x3; i++) { Size[Count_1x1 + Count_2x2 + i] = 3; }
        X = new int[ItemCount];
        Y = new int[ItemCount];
        bool Good = false;
        while (!Good) {
            Random Rnd = new();
            for (int i = 0; i < ItemCount; i++) {
                X[i] = Rnd.Next(0, MAX_FIELD_MULT * ItemCount);
                Y[i] = Rnd.Next(0, MAX_FIELD_MULT * ItemCount);
            }
            Good = CheckCorrectness();
        }
        Square = CalcSquare();
    }
    public Genotype(Genotype Mother, Genotype Father)
    {
        ItemCount = Mother.ItemCount;
        X = new int[ItemCount];
        Y = new int[ItemCount];
        Size = Mother.Size;
        for (int i = 0; i < ItemCount; i++) {
            if (i % 2 == 0) {
                X[i] = Mother.X[i];
                Y[i] = Mother.Y[i];
            } else {
                X[i] = Father.X[i];
                Y[i] = Father.Y[i];
            }
        }
        Square = CalcSquare();
    }
    public Genotype(Genotype Mutant)
    {
        ItemCount = Mutant.ItemCount;
        X = new int[ItemCount];
        Y = new int[ItemCount];
        Size = Mutant.Size;
        for (int i = 0; i < ItemCount; i++) {
            X[i] = Mutant.X[i];
            Y[i] = Mutant.Y[i];
        }
        bool Good = false;
        int StopCnt = 0;
        while (!Good && StopCnt < RANDOM_STOP_CNT) { 
            StopCnt++;
            Random Rnd = new();
            int i = Rnd.Next(0, ItemCount); 
            X[i] = Rnd.Next(0, MAX_FIELD_MULT * ItemCount);
            Y[i] = Rnd.Next(0, MAX_FIELD_MULT * ItemCount);
            Good = CheckCorrectness();
            if (!Good) {
                X[i] = Mutant.X[i];
                Y[i] = Mutant.Y[i];
            }
        }
        Square = CalcSquare();
    }
}

//public class GenotypeEnumerator : IEnumerator
//{
//    readonly int[] X;
//    readonly int[] Y;
//    readonly int[] Size;
//    int i = -1;

//    public GenotypeEnumerator(int[] x, int[] y, int[] size)
//    {
//        X = x;
//        Y = y;
//        Size = size;
//    }
//    public object Current
//    {
//        get
//        {
//            if (i == -1 || i >= X.Length)
//                throw new ArgumentException();
//            return new Square(X[i], Y[i], Size[i]);
//        }
//    }
//    public bool MoveNext()
//    {
//        if (i < X.Length - 1)
//        {
//            i++;
//            return true;
//        }
//        else
//            return false;
//    }
//    public void Reset() => i = -1;
//}
