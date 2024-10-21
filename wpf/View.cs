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

        Population MainPopulation { get; set; }

        public View()
        {
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
            MainPopulation = new Population(Count1x1, Count2x2, Count3x3);
            NumberOfGeneration = 0;
            Square = MainPopulation.FirstGen().Square;
            OnPropertyChanged("Square");
            OnPropertyChanged("NumberOfGeneration");
        }

        public void NextGeneration()
        {
            MainPopulation.Reproduction(POPULATION_SIZE / 2);
            MainPopulation.Mutation(POPULATION_SIZE / 3);
            Population NewPop = MainPopulation.Selection(POPULATION_SIZE);
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
            foreach (Tuple<int,int,int> Gen in BestGenotype) 
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
    }
}
