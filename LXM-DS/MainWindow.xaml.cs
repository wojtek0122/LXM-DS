﻿using System;
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
using System.Windows.Threading;

namespace LXM_DS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Managers _managers;
        MYSQL.MySQLManager _mysqlManager;
        AUTOUPDATE.AutoUpdate _autoUpdate;

        private string _login;
        DispatcherTimer _timer;
        public MainWindow(string Login, int Permission)
        {
            InitializeComponent();
            _managers = Managers.CreateManagers();
            _managers.InitializePrinters();
            _mysqlManager = MYSQL.MySQLManager.CreateManager();
            _autoUpdate = AUTOUPDATE.AutoUpdate.CreateAutoUpdate();

            lblName.Content = _login = Login;
            if(Permission == 9)
            {
                gboxDismantle.Visibility = System.Windows.Visibility.Visible;
            }

            lblVersion.Content = "version: " + _autoUpdate.GetCurrentVersion().ToString();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += timer_Tick;
            _timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            lblName.Content = _login + " " + DateTime.Now.ToString("HH:mm:ss");
            if (_login != "admin")
            {
                if ((DateTime.Now.Hour == 16) && (DateTime.Now.Minute == 1))
                {
                    _timer.Stop();
                    LoginWindow _loginWindow = new LoginWindow();
                    _loginWindow.Topmost = true;
                    _loginWindow.Show();
                    this.Close();
                }
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            _managers.ReinitializePrinters();
            TestWindow _testWindow = new TestWindow(_login);
            _testWindow.Topmost = true;
            _testWindow.Show();
        }

        private void btnDismantle_Click(object sender, RoutedEventArgs e)
        {
            _managers.ReinitializePrinters();
            ChoosePrinterWindow _choosePrinter = new ChoosePrinterWindow(_login);
            //_choosePrinter.Topmost = true;
            _choosePrinter.Show();
        }

        private void btnWyloguj_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow _loginWindow = new LoginWindow();
            _loginWindow.Topmost = true;
            _loginWindow.Show();
            this.Close();
        }

        private void btnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            NewOrderWindow _newOrderWindow = new NewOrderWindow();
            _newOrderWindow.Topmost = true;
            _newOrderWindow.Show();
        }

        private void btnSupermarket_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
