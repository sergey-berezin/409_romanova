using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using PackagingGenetic;
using static PackagingGenetic.Constants;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Text.Json;
using System.Xml.Linq;
using System.IO;
using static System.Formats.Asn1.AsnWriter;


namespace WpfGenetic
{
    public class View : INotifyPropertyChanged
    {
        public int Count1x1 {  get; set; }
        public int Count2x2 { get; set; }
        public int Count3x3 { get; set; }
        public int Square { get; set; }
        public int NumberOfGeneration { get; set; }
        public int PopulationSize { get; set; }
        public string DirPath { get; set; }
        public string LoadDirPath { get; set; }
        public string MainFile = "\\Experiments.json";
        public string FileEnding = "_Data.json";
        Population MainPopulation { get; set; }

        public View()
        {
            DirPath = "nope";
            Count1x1 = 5;
            Count2x2 = 0;
            Count3x3 = 0;
            NumberOfGeneration = 0;
            PopulationSize = POPULATION_SIZE;    
            MainPopulation = new Population(Count1x1, Count2x2, Count3x3);
            Square = 0;
        }

        public void CreatePopulation()
        {
            MainPopulation = new Population(Count1x1, Count2x2, Count3x3, PopulationSize);
            NumberOfGeneration = 0;
            Square = MainPopulation.FirstGen().Square;
            OnPropertyChanged("Square");
            OnPropertyChanged("NumberOfGeneration");
        }

        public void NextGeneration()
        {
            MainPopulation.Reproduction(PopulationSize / 2);
            MainPopulation.Mutation(PopulationSize / 3);
            Population NewPop = MainPopulation.Selection(PopulationSize);
            MainPopulation = new Population(NewPop);
            Square = MainPopulation.FirstGen().Square;
            NumberOfGeneration++;
            OnPropertyChanged("Square");
            OnPropertyChanged("NumberOfGeneration");
        }

        public Canvas DrawPopulation(double CanvasWidth, double CanvasHeight)
        {
            Canvas ViewCanvas = new Canvas();
            Genotype BestGenotype = MainPopulation.FirstGen();
            int Scale = (int)CanvasHeight / (BestGenotype.X.Length * MAX_FIELD_MULT);
            var BestList = BestGenotype.SquareList();
            foreach (Tuple<int,int,int> Gen in BestList) 
            {
                int X = Gen.Item1;
                int Y = Gen.Item2;
                int Size = Gen.Item3;
                Rectangle rectangle = new Rectangle();
                rectangle.Width = Size * Scale;
                rectangle.Height = Size * Scale;
                Canvas.SetLeft(rectangle, (int)(CanvasWidth - CanvasHeight) / 2 + X * Scale);
                Canvas.SetBottom(rectangle, Y * Scale);
                rectangle.Fill = Brushes.Navy;
                ViewCanvas.Children.Add(rectangle);
            }
            return ViewCanvas;
        }

        

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public static JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        public void DeleteExperiment(string Name)
        {
            string[] Experiments = File.ReadAllLines(DirPath + MainFile);
            File.WriteAllText(DirPath + MainFile, "");
            foreach (var Str in Experiments)
            {
                if (Str.LastIndexOf(Name + FileEnding) == -1)
                {
                    File.AppendAllText(DirPath + MainFile, Str + "\n");
                }
            }
            File.Delete(DirPath + "\\" + Name + FileEnding);
        }
        public void SaveExperiment(string Name)
        {
            if (File.Exists(DirPath + MainFile))
            {
                string[] Experiments = File.ReadAllLines(DirPath + MainFile);
                bool ok = true;
                foreach (var Str in Experiments)
                {
                    if (Str.LastIndexOf(Name + FileEnding) != -1)
                    {
                        ok = false;
                        break;
                    }
                }
                if (!ok)
                {
                    throw new Exception("");
                }

            }
            string ExperimentPath = DirPath + "\\" + Name + FileEnding;
            File.AppendAllText(DirPath + MainFile, NumberOfGeneration.ToString() + " " + Name + " " + Name + FileEnding + "\n"); 
            File.AppendAllText(ExperimentPath, JsonSerializer.Serialize(MainPopulation, options));
        }

        public List<string> LoadExperiments {  get; set; }
        public bool MainFileExists()
        {
            if (File.Exists(LoadDirPath + MainFile))
            {
                LoadExperiments = new();
                string[] Strs = File.ReadAllLines(LoadDirPath + MainFile);
                foreach (var Str in Strs)
                {
                    string[] Words = Str.Split(' ');
                    LoadExperiments.Add(Words[1]);
                }
                OnPropertyChanged("LoadExperiments");
                return true;
            } else
            {
                return false;
            }
        }
        public void LoadExperiment(string Name)
        {
            string[] Experiments = File.ReadAllLines(LoadDirPath + MainFile);
            string ExpName = "";
            foreach (var Str in Experiments)
            {
                if (Str.LastIndexOf(Name + FileEnding) != -1)
                {
                    ExpName = Str.Remove(0, Str.LastIndexOf(Name));
                    string[] Words = Str.Split(' ');
                    NumberOfGeneration = int.Parse(Words[0]);
                }
            }
            string Exp = File.ReadAllText(LoadDirPath + "\\" + ExpName);
            MainPopulation = JsonSerializer.Deserialize<Population>(Exp);
            PopulationSize = MainPopulation.Guys.Count;
            Square = MainPopulation.FirstGen().Square;
            Count1x1 = 0;
            Count2x2 = 0;
            Count3x3 = 0;
            var BestList = MainPopulation.FirstGen().SquareList();
            foreach (Tuple<int, int, int> Gen in BestList)
            {
                if (Gen.Item3 == 1)
                {
                    Count1x1++;
                }
                if (Gen.Item3 == 2)
                {
                    Count2x2++;
                }
                if (Gen.Item3 == 3)
                {
                    Count3x3++;
                }
            }
            
            OnPropertyChanged("Count1x1");
            OnPropertyChanged("Count2x2");
            OnPropertyChanged("Count3x3");
            OnPropertyChanged("PopulationSize");
            OnPropertyChanged("Square");
            OnPropertyChanged("NumberOfGeneration");
        }
    }
}
