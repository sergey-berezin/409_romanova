﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.Timers;

namespace WpfGenetic
{

    public partial class MainWindow : Window
    {
        public View ModelView;
        public static System.Windows.Threading.DispatcherTimer SlowTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.4) };
        public static System.Windows.Threading.DispatcherTimer FastTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.08) };

        public MainWindow()
        {
            InitializeComponent();
            ModelView = new View();
            this.DataContext = ModelView;
        }
        private void CreateClick(object sender, RoutedEventArgs e)
        {
            ModelView.CreatePopulation();
            DrawPicture();
            SlowTimer.Stop();
            FastTimer.Stop();
            StartStop.Content = "Start";
            SpeedButton.Content = "Faster";
        }
        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            ModelView.NextGeneration();
            DrawPicture();
        }
        private void MoreNextButtonClick(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                ModelView.NextGeneration();
            }
            DrawPicture();
        }
        private void StartStopClick(object sender, RoutedEventArgs e)
        {
            SlowTimer.Tick += (sender, args) =>
            {
                NextButtonClick(sender, e);
            };
            FastTimer.Tick += (sender, args) =>
            {
                NextButtonClick(sender, e);
            };
            if (SlowTimer.IsEnabled || FastTimer.IsEnabled)
            {
                SlowTimer.Stop();
                FastTimer.Stop();
                StartStop.Content = "Start";
            } else
            {
                SlowTimer.Start();
                StartStop.Content = "Stop";
            }
            
        }
        private void SpeedButtonClick(object sender, RoutedEventArgs e)
        {
            if (!SlowTimer.IsEnabled && !FastTimer.IsEnabled)
            {
                return;
            }
            if (FastTimer.IsEnabled)
            {
                FastTimer.Stop();
                SlowTimer.Start();
                SpeedButton.Content = "Faster";
            }
            else
            {
                SlowTimer.Stop();
                FastTimer.Start();
                SpeedButton.Content = "Slower";
            }
        }
        private void DrawPicture()
        {
            Canvas Template_canvas1 = ModelView.DrawPopulation(SquareCanvas.Width, SquareCanvas.Height);
            var childrenList = Template_canvas1.Children.Cast<UIElement>().ToArray();
            SquareCanvas.Children.Clear();
            foreach (var c in childrenList)
            {
                Template_canvas1.Children.Remove(c);
                SquareCanvas.Children.Add(c);
            }
        }
    }
}