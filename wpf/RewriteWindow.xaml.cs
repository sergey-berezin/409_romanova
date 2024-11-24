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
    /// Interaction logic for RewriteWindow.xaml
    /// </summary>
    public partial class RewriteWindow : Window
    {
        public bool Rewrite {  get; set; }
        public RewriteWindow()
        {
            InitializeComponent();
        }
        public void YesClick(object sender, RoutedEventArgs e)
        {
            Rewrite = true;
            this.Close();
        }
        public void NoClick(object sender, RoutedEventArgs e)
        {
            Rewrite = false;
            this.Close();
        }
    }
}
