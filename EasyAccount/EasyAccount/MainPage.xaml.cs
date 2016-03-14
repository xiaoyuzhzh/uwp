using EasyAccount.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using EasyAccount.Services;
using System.Collections.ObjectModel;
using EasyAccount.Views;
using Windows.UI.Core;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace EasyAccount
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public List<string> classOnes { get; set; }

        public List<string> classTwos { get; set; }

        public List<string> classThrees { get; set; }

        public ObservableCollection<Transaction> transactions { get; set; }

        public int i { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            AppDatabase.initTable();

            this.frame.Navigate(typeof(Statistics));

            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManager_BackRequested;

            WifiConnectionLost();
        }

        private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (frame.CanGoBack && !e.Handled)
            {
                frame.GoBack();
            }
            infoOutText.Text = "手机点击了返回："+ i++;
            e.Handled = true;
        }


        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {

        }

        private void OnNavigatedToPage(object sender, NavigationEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
