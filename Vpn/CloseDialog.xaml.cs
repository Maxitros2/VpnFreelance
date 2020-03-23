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
    /// Логика взаимодействия для CloseDialog.xaml
    /// </summary>
    public partial class CloseDialog : UserControl
    {
        public CloseDialog()
        {
            InitializeComponent();
        }
        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Label)sender).Foreground = Colors.Offline;
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Label)sender).Foreground = Brushes.White;
        }

        private void Label_MouseEnter_1(object sender, MouseEventArgs e)
        {

            ((Label)sender).Foreground = Colors.Online;
        }
    }
}
