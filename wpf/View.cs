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

        public string MainFile = "\\Experiments.json";
        public string FileEnding = "_Data.json";
        public List<Experiment> ExList { get; set; }
        public List<string> LoadExperiments { get; set; }
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
            ExList = new();
            LoadExperiments = new();
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
        public bool InitDir(string Name, string Mode)
        {
            DirPath = Name;
            ExList = new();
            LoadExperiments = new();
            if (File.Exists(DirPath + MainFile))
            {
                string Exp = File.ReadAllText(DirPath + MainFile);
                ExList = JsonSerializer.Deserialize<List<Experiment>>(Exp);
                foreach (var Ex in ExList)
                {
                    LoadExperiments.Add(Ex.ExName);
                }
                OnPropertyChanged("LoadExperiments");
            } else
            {
                if (Mode == "write")
                {
                    return false;
                }
            }
            return true;
        }
        public void DeleteExperiment(string Name)
        {
            Experiment DeleteEx = new();
            foreach (var Ex in ExList)
            {
                if (Ex.ExName == Name)
                {
                    DeleteEx = Ex;
                }
            }
            File.Delete(DeleteEx.ExFileName);
            ExList.Remove(DeleteEx);
            File.WriteAllText(DirPath + MainFile, JsonSerializer.Serialize(ExList, options));
        }
        public void SaveExperiment(string Name)
        {
            string ExperimentPath = DirPath + "\\" + Name + FileEnding;
            if (File.Exists(ExperimentPath))
            {
                throw new Exception("");
            }
            ExList.Add(new(NumberOfGeneration, Count1x1, Count2x2, Count3x3,
                Square, PopulationSize, Name, ExperimentPath));
            File.WriteAllText(DirPath + MainFile, JsonSerializer.Serialize(ExList, options));
            File.AppendAllText(ExperimentPath, JsonSerializer.Serialize(MainPopulation, options));
        }


        public void LoadExperiment(string Name)
        {
            foreach (var Ex in ExList)
            {
                if (Ex.ExName == Name)
                {
                    Count1x1 = Ex.Count1x1;
                    Count2x2 = Ex.Count2x2;
                    Count3x3 = Ex.Count3x3;
                    PopulationSize = Ex.PopulationSize;
                    Square = Ex.Square;
                    NumberOfGeneration = Ex.IterCnt;
                    MainPopulation = JsonSerializer.Deserialize<Population>
                        (File.ReadAllText(Ex.ExFileName));
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
