using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfGenetic
{
    /// <summary>
    /// Interaction logic for LoadWindow.xaml
    /// </summary>
    public partial class LoadWindow : Window
    {
        View ModelV {  get; set; }

        public LoadWindow(View modelV)
        {
            ModelV = modelV;
            this.DataContext = modelV;
            InitializeComponent();
        }

        public void SelectClick(object sender, RoutedEventArgs e)
        {
            if (LoadList.SelectedValue != null)
            {
                ModelV.LoadExperiment(LoadList.SelectedValue.ToString());
                this.Close();
            }
        }
        public void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
