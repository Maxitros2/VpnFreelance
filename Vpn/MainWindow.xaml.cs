using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Vpn
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon NotifyIcon = new System.Windows.Forms.NotifyIcon();
        public int status = 0;
        VpnTTCP vpnTTCP;
       
        Vpn_ _selectedVpn;       
        #region StaticVars
        private static string[][] VpnServers = new string[7][]
            {
                new string[]{"https://freevpn.me/accounts/","Amsterdam" },
                new string[]{"https://freevpn.se/accounts/","France" },
                new string[]{"https://freevpn.im/accounts/","France" },
                new string[]{"https://freevpn.it/accounts/","Italy" },
                new string[]{"https://freevpn.be/accounts/","Poland" },
                new string[]{"https://freevpn.co.uk/accounts/","Germany"},
                new string[]{"https://freevpn.eu/accounts/","Amsterdam"}
                
            };
        private static Dictionary<string, BitmapImage> flags = new Dictionary<string, BitmapImage>()
        {
            { "Amsterdam",new BitmapImage(new Uri(@"pack://siteoforigin:,,,/Resources/Netherlands.png"))},
            { "France",new BitmapImage(new Uri(@"pack://siteoforigin:,,,/Resources/France.png"))},
            { "Italy",new BitmapImage(new Uri(@"pack://siteoforigin:,,,/Resources/Italy.png"))},
            { "Poland",new BitmapImage(new Uri(@"pack://siteoforigin:,,,/Resources/Poland.png"))},
             { "Germany",new BitmapImage(new Uri(@"pack://siteoforigin:,,,/Resources/Germany.png"))},




        }
            ;
        #endregion
        public MainWindow()
        {
            try
            {
                
                InitializeComponent();
                NotifyIcon.Visible = true;
                NotifyIcon.Icon = Properties.Resources.vpn;
                this.Left = SystemParameters.WorkArea.Right - this.Width;
                this.Top = SystemParameters.WorkArea.Bottom - this.Height;
                getVpnAsync();
                LoadingAnim(true);
                Animations animations = new Animations(this);
                vpnTTCP = new VpnTTCP(this);
               
                ClosingDialog.NoButton.PreviewMouseDown += NoButton_PreviewMouseDown;
            }catch
            (Exception e)
            { MessageBox.Show(e.ToString()); }
            }

        private void NoButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           
            {
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.Completed +=
                   delegate
                   {
                       ClosingDialog.Visibility = Visibility.Hidden;
                       MainCanvas.IsEnabled = true;
                   };
                doubleAnimation.From = 1D;
                doubleAnimation.To = 0D;
                ClosingDialog.BeginAnimation(UserControl.OpacityProperty, doubleAnimation);
               
            }
            {
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = 5D;
                doubleAnimation.To = 0D;
                MainCanvas.Effect.BeginAnimation(System.Windows.Media.Effects.BlurEffect.RadiusProperty, doubleAnimation);
            }
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(status ==0)
            {
                status = 1;
                setStatus(status);
                return;
            }
            if (status == 1)
            {
                status = 0;
                setStatus(status);
                return;
            }
        }
        private void disableClick(bool state)
        {
            if (state)
                conngrid.PreviewMouseDown -= Grid_PreviewMouseDown;
            else
                conngrid.PreviewMouseDown += Grid_PreviewMouseDown;

        }
        public void setStatus(int status)
        {
            switch(status)
            {
                case 1: InfoLabel.Content = "Connecting...";
                    Status.Content = "LOADING";
                    colorSwitcher(BackGroundHeat, Colors.Connecting);
                    colorSwitcher(Status, Colors.Connecting);
                   
                    vpnTTCP.host = _selectedVpn.IP;
                    vpnTTCP.User = _selectedVpn.User;
                    vpnTTCP.Pass = _selectedVpn.Pass;
                    vpnTTCP.Connect();
                    disableClick(true);



                    break;
                case 2:
                    InfoLabel.Content = "Connected";
                    Status.Content = "ONLINE";
                    colorSwitcher(BackGroundHeat, Colors.Online);
                    colorSwitcher(Status, Colors.Online);
                    disableClick(false);
                   
                    break;
                case 0:
                    vpnTTCP.Disconnect();
                    InfoLabel.Content = "Click here to connect";
                    Status.Content = "OFFLINE";
                    colorSwitcher(BackGroundHeat, Colors.Offline);
                    colorSwitcher(Status, Colors.Offline);
                    disableClick(false);
                    break;
            }
        }
        private void colorSwitcher(object o, Brush br2)
        {
            SolidColorBrush rootElementBrush = new SolidColorBrush();
            ColorAnimation animation;

            if (o is Rectangle)
            {
                Rectangle rectangle = (Rectangle)o;
                rootElementBrush.Color = ((SolidColorBrush)rectangle.Fill).Color;
                rectangle.Fill = rootElementBrush;
            }else if(o is Label)
            {
                Label label = (Label)o;
                rootElementBrush.Color = ((SolidColorBrush)label.Foreground).Color;
                label.Foreground = rootElementBrush;
            }
                

                // Animate the brush 
                animation = new ColorAnimation();
                animation.To = ((SolidColorBrush)br2).Color;
                animation.Duration = new Duration(TimeSpan.FromSeconds(3));
                rootElementBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            
        }

        private void Grid_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            ClosingDialog.Visibility = Visibility.Visible;
            MainCanvas.IsEnabled = false;
            {
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = 0D;
                doubleAnimation.To = 1D;
                ClosingDialog.BeginAnimation(UserControl.OpacityProperty, doubleAnimation);

            }
            {
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = 0D;
                doubleAnimation.To = 5D;
                MainCanvas.Effect.BeginAnimation(System.Windows.Media.Effects.BlurEffect.RadiusProperty, doubleAnimation);
            }
        }
       
       
       
        public void RessiveOutput(int output)
        {
            MessageBox.Show(output.ToString());
           if(output==0)
            {

                status = 2;
                setStatus(status);

            }
           else
            {
                status = 0;
                setStatus(status);
            }
        }
        public void ChangeVpn(Vpn_ vpn)
        {
            _selectedVpn = vpn;
            BitmapImage bitmapImage= new BitmapImage();
            flags.TryGetValue(vpn.Country, out bitmapImage);
            SelectedRegionFlag.Source = bitmapImage;
            SelectadRegionText.Content = vpn.Country;
        }
        private void setSerstatus(bool stat)
        {
            ServerList.Visibility = stat ? Visibility.Visible : Visibility.Hidden;
        }

        private void getVpnAsync()
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.RunWorkerCompleted +=
                delegate
                {
                    LoadingAnim(false);
                    ChangeVpn(VpnList._VpnList.First());
                    
                    foreach (Vpn_ vpn in VpnList._VpnList)
                    {
                        ServerItem serverItem = new ServerItem();
                        serverItem.NameOf.Content = vpn.Country;
                        BitmapImage bitmapImage = new BitmapImage();
                        flags.TryGetValue(vpn.Country, out bitmapImage);
                        serverItem.Flag.Source = bitmapImage;
                        serverItem.vpn = vpn;
                        long repl = 0;
                        BackgroundWorker bw2 = new BackgroundWorker();
                        bw2.RunWorkerCompleted +=
                        delegate
                        {
                            if(repl>120)
                            {
                                Brush brush = Brushes.Red;
                               
                                serverItem.Ping.Foreground = brush;
                            }
                            if (repl < 120 & repl>80)
                            {
                                Brush brush = Brushes.Yellow;
                               
                                serverItem.Ping.Foreground = brush;
                            }
                            if (repl < 80)
                            {
                                Brush brush = Brushes.LimeGreen;
                               
                                serverItem.Ping.Foreground = brush;
                            }
                            serverItem.Ping.Content = repl;
                            if (repl > 0)
                            {
                                serverItem.PreviewMouseDown +=
                                delegate
                                {
                                    setSerstatus(false);
                                    ChangeVpn(serverItem.vpn);
                                };
                                StackServers.Children.Add(serverItem);
                            }

                        };
                        bw2.DoWork+=
                        delegate
                        {
                            Ping ping = new System.Net.NetworkInformation.Ping();
                            PingReply pingReply = null;
                            pingReply = ping.Send(vpn.IP);
                            
                            repl = pingReply.RoundtripTime/2;
                        };
                        bw2.RunWorkerAsync();






                    }

                };


            bw.DoWork +=
                delegate
                {
                    for (int i = 0; i < 7; i++)
                    {

                        try
                        {
                            string st = new WebClient() { Proxy = null }.DownloadString(VpnServers[i][0]);
                            st = st.Remove(st.IndexOf("<h3>L2TP/IPSec (PSK)</h3>"), st.Length - st.IndexOf("<h3>L2TP/IPSec (PSK)</h3>", 10));
                            st = st.Remove(0, st.IndexOf("<h3>PPTP</h3>"));
                            string[] s = new string[3];
                            int k2 = 0;
                            while (st.Contains("</b>"))
                            {
                                string k = st.Remove(0, st.IndexOf("</b>") + 5);
                                k = k.Remove(k.IndexOf("</li>"), k.Length - k.IndexOf("</li>"));
                                st = st.Remove(0, st.IndexOf("</li>") + 5);
                                s[k2] = k;
                                k2++;
                            }
                            Vpn_ vpn = new Vpn_
                            {
                                IP = s[0],
                                User = s[1],
                                Pass     =s[2],
                                Country  = VpnServers[i][1]
                            };
                            VpnList._VpnList.Add(vpn);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }

                };
            bw.RunWorkerAsync();



        }
        Storyboard s = new Storyboard();
        private void LoadingAnim(bool run)
        {
            if (run)
            {

                s.Stop();
                LoadingPanel.Visibility = Visibility.Visible;
                MainCanvas.IsEnabled = false;
                MainCanvas.Effect.SetValue(System.Windows.Media.Effects.BlurEffect.RadiusProperty, 10d);
                s = new Storyboard();
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = 0d;
                animation.To = 360d;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                animation.RepeatBehavior = new RepeatBehavior(int.MaxValue);
                s.Children.Add(animation);
                Storyboard.SetTarget(animation, LoadingRot);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(Grid.RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)"));
                s.Begin();
            }
            else
            {
                MainCanvas.IsEnabled = true;
                LoadingPanel.Visibility = Visibility.Hidden;
                MainCanvas.Effect.SetValue(System.Windows.Media.Effects.BlurEffect.RadiusProperty, 0d);
            }
        }
            
        private void ChengeLock(bool started)
        {
            
                
                
                    LockImage2.Visibility = Visibility.Visible;
                    DoubleAnimation doubleAnimation = new DoubleAnimation();
                    doubleAnimation.From = started?0d:1d;
                    doubleAnimation.To = !started ? 0d : 1d;
                    doubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
                    QuarticEase easingFunction = new QuarticEase();
                    easingFunction.EasingMode = EasingMode.EaseInOut;
                    doubleAnimation.EasingFunction = easingFunction;
                    LockImage2.BeginAnimation(Image.OpacityProperty, doubleAnimation);
                
            
        }

        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            ((Grid)sender).Background= (SolidColorBrush)(new BrushConverter().ConvertFrom("#87000000"));
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Grid)sender).Background = null;
        }

        private void Grid_PreviewMouseDown_2(object sender, MouseButtonEventArgs e)
        {
            setSerstatus(false);
        }

        private void Grid_PreviewMouseDown_3(object sender, MouseButtonEventArgs e)
        {
            setSerstatus(true);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            vpnTTCP.Disconnect();
            foreach (Process pr in vpnTTCP.toclose)
            {
                try
                {
                    pr.Kill();
                }
                catch (Exception) { };
            }
        }
        public void killrasdial()
        {
            if (Process.GetProcessesByName("rasdial").Count() > 0)
            {
                foreach (Process or in Process.GetProcessesByName("rasdial"))
                    or.Kill();
             }
        }
    }
}
