using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Xml.Linq;

namespace WpfGenetic
{
    /// <summary>
    /// Interaction logic for SaveWindow.xaml
    /// </summary>

    public class SaveText : INotifyPropertyChanged
    {
        public int ExperimentCnt { get; set; }
        public string Name { get; set; }
  
        public event PropertyChangedEventHandler? PropertyChanged;
        public SaveText(int experimentCnt = 0, string savePath = "Experiment")
        {
            ExperimentCnt = experimentCnt;
            Name = savePath + ExperimentCnt.ToString();
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }

    public partial class SaveWindow : Window
    {
        public int PatienceCnt { get; set; }
        View ModelV {  get; set; }
        SaveText SaveData { get; set; }

        public SaveWindow(View ModelView)
        {
            SaveData = new(ModelView.NumberOfGeneration);
            ModelV = ModelView;
            this.DataContext = SaveData;
            PatienceCnt = 5;
            InitializeComponent();
        }

        public void OkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ModelV.SaveExperiment(SaveData.Name);
                this.Close();
            }
            catch (Exception)
            {

                RewriteWindow rewriteWindow = new RewriteWindow();
                rewriteWindow.ShowDialog();
                
                if (rewriteWindow.Rewrite == true) 
                {
                    rewriteWindow.Close();
                    ModelV.DeleteExperiment(SaveData.Name);
                    ModelV.SaveExperiment(SaveData.Name);
                    this.Close();
                }
            }
        }

        public void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
