﻿using LXM_DS.BUTTON;
using LXM_DS.MYSQL;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace LXM_DS
{
    /// <summary>
    /// Interaction logic for ChoosePrinterWindow.xaml
    /// </summary>
    public partial class ChoosePrinterWindow : Window
    {
        private string _printerMT;
        private string _printerSN;
        private int _testid;
        Managers _managers;
        MySQLManager _mysqlManager;
        private string _login;
        ButtonListManager _buttonListManager;
        public ChoosePrinterWindow(String Login)
        {
            InitializeComponent();
            _managers = Managers.CreateManagers();
            _mysqlManager = _managers.GetMySQLManager();
            _buttonListManager = _managers.GetButtonListManager();
            _login = Login;
            txtLabel.Focus();
        }

        private void btnWybierz_Click(object sender, RoutedEventArgs e)
        {
            _testid = _mysqlManager.GetTestIDFromTestBySN(_printerSN);
            _buttonListManager.ClearList();
            if (_testid == 0)
            {
                this.lblInfo.Content = "BLAD: Drukarka nie zostala jeszcze przetestowana!";
            }
            else
            {
                _mysqlManager.SetDismantled(_testid);
                //ComponentWindowThumbnails _componentWindowThumbnails = new ComponentWindowThumbnails(_testid, _mysqlManager.GetMTFromPrintersWherePrinterID(_mysqlManager.GetPrinterIDFromTestBySN(_printerSN)), _mysqlManager.GetTestStatusFromTestBySN(_printerSN), _login);
                ComponentWindowThumbnails _componentWindowThumbnails = new ComponentWindowThumbnails(_testid, _mysqlManager.GetMTFromPrintersWherePrinterID(_mysqlManager.GetPrinterIDFromTestBySN(_printerSN)), _mysqlManager.GetPrinterIDFromTestBySN(_printerSN), _mysqlManager.GetTestStatusFromTestBySN(_printerSN), _login);
                this.Close();
                _componentWindowThumbnails.Show();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.txtLabel.Text.Length == 14 || this.txtLabel.Text.Length == 12)
                {
                    ParseTextBoxContent(this.txtLabel.Text.ToString());
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }

        private void ParseTextBoxContent(string Content)
        {
            _printerMT = Content.Substring(1, 4);
            _printerSN = Content;
            //if (_printerMT == "7014")
            //{
            //    _printerSN = Content.Substring(5, 7);
            //}
            //else
            //{
            //    _printerSN = Content.Substring(5, 9);
            //}
            ChangePrinterFoto();
        }

        private void ChangePrinterFoto()
        {
            if(File.Exists(@"..\..\FILES\" + _printerMT.ToString() + ".png"))
            {
                this.imgFoto.Source = new ImageSourceConverter().ConvertFromString(@"..\..\FILES\" + _printerMT.ToString() + ".png") as ImageSource;
            }
            else
            {
                this.imgFoto.Source = new ImageSourceConverter().ConvertFromString(@"..\..\FILES\lexmark.png") as ImageSource;
            }
        }

        private void btnKeyboard_Click(object sender, RoutedEventArgs e)
        {
            string KeyboardPath = @"C:\Program Files\Common Files\Microsoft Shared\Ink\TabTip.exe";
            System.Diagnostics.Process.Start(KeyboardPath);
        }
    }
}
