using DotRas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Vpn
{
    public class VpnTTCP
    {
        MainWindow mainWindow;
        public string host, User, Pass;
        public VpnTTCP(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }
        public List<Process> toclose = new List<Process>();
        private static string FolderPath => string.Concat(Directory.GetCurrentDirectory(),
            "\\VPN");
        public void Disconnect()
        {



            RasCon = RasConnection.GetActiveConnectionByName(Pass, RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User));
            if (dialer.IsBusy)

                dialer.DialAsyncCancel();
            else
            {


                if (RasCon != null)

                    RasCon.HangUp();

            }
            try
            {
                Process.GetProcessesByName("ping").First().Kill();
            }
            catch (Exception ee)
            { };



            Boolean isBusy = dialer.IsBusy;


            dialer.DialAsyncCancel();
            isBusy = dialer.IsBusy;
            if (isBusy)
                dialer.DialAsyncCancel();
        }
        public static RasDialer dialer = new RasDialer();
        public static RasConnection RasCon;
        public static void dialVPN(String entryName, String bookPath, NetworkCredential credentials)
        {
            dialer.EntryName = entryName;
            dialer.PhoneBookPath = bookPath;
            dialer.Credentials = credentials;

            dialer.Dial();



        }
        public void Connect()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork +=
                delegate
                {
                    try
                    {
                        System.Net.NetworkCredential creds;
                        RasEntry pptpEntry;
                        RasPhoneBook RasPhoneBook1 = new RasPhoneBook();
                        string path = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User);
                        RasPhoneBook1.Open(path);
                        creds = new NetworkCredential(User, Pass);
                        Console.WriteLine(creds.Password);
                        bool flag = true;
                        foreach (RasEntry ras in RasPhoneBook1.Entries.ToList())
                        {
                            if (ras.Name == creds.Password)
                            {
                                dialVPN(creds.Password, RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User), creds);
                                flag = false;
                            }
                        }
                        if (flag)
                        {
                            pptpEntry = RasEntry.CreateVpnEntry(creds.Password, host, RasVpnStrategy.PptpOnly, RasDevice.GetDeviceByName("(PPTP)", RasDeviceType.Vpn, false));

                            RasPhoneBook1.Entries.Add(pptpEntry);

                            dialVPN(creds.Password, RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User), creds);

                        }
                        mainWindow.Dispatcher.BeginInvoke
                        (new Action(() =>
                        {
                            mainWindow.RessiveOutput(0);
                        }));
                       

                    }
                    catch
                    (Exception ex)
                    {
                        mainWindow.Dispatcher.BeginInvoke
                          (new Action(() =>
                          {
                              mainWindow.status=0;
                              mainWindow.setStatus(0);

                          }));
                    }
                };
            backgroundWorker.RunWorkerAsync();
          






        }
    }
}
