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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vpn
{
    /// <summary>
    /// Логика взаимодействия для ServerItem.xaml
    /// </summary>
    public partial class ServerItem : UserControl
    {
        public Vpn_ vpn;
        public ServerItem()
        {
            InitializeComponent();
        }
        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            ((Grid)sender).Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#87000000"));
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Grid)sender).Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF38342A"));
        }
    }
}
