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
using EasyAccount.Views.Summarize;
using Windows.Storage;
using EasyAccount.Services.Respository;

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

            CategoryRepository.initData();

            QuickUtil.mainPage = this;

            //this.frame.Navigate(typeof(Statistics));

            this.frame.Navigate(typeof(Summarize));


            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManager_BackRequested;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            this.frame.Navigated += OnNavigatedToPage;
        }


        /**
         * 返回按钮处理方案 
         */
        private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (frame.CanGoBack && !e.Handled)
            {
                frame.GoBack();
            }
            e.Handled = true;
        }


        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {

        }

        private void OnNavigatedToPage(object sender, NavigationEventArgs e)
        {
            if (frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }


        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
            mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
        }

        private void addTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            var form = new TransactionForm();

            form.ShowAsync();
        }

        /**
        * 汉堡包菜单选择事件处理
        */
        private void mainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.frame == null) return;
            ListBoxItem selectedItem = (ListBoxItem)((ListBox)sender).SelectedItem;
            if (selectedItem == null) return;
            var itemName = selectedItem.Name;
            switch (itemName)
            {
                case "summary": {
                        this.frame.Navigate(typeof(Summarize));
                        break;
                    }
                case "recentBills": {
                        this.frame.Navigate(typeof(LatelyBills));
                        break;
                    }
                case "special": {
                        this.frame.Navigate(typeof(Special));
                        break;
                    }
                case "debug": {
                        this.frame.Navigate(typeof(DebugView));
                        break;
                    }
                default: { break; }
            }
            mainSplitView.IsPaneOpen = false;
        }

        private void bottomListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.frame == null) return;
            ListBoxItem selectedItem = (ListBoxItem)((ListBox)sender).SelectedItem;
            if (selectedItem == null) return;
            var itemName = selectedItem.Name;
            switch (itemName)
            {
                case "setting":
                    {
                        this.frame.Navigate(typeof(SettingView));
                        //selectedItem.IsSelected = false;
                        break;
                    }
                default: { break; }
            }
            mainSplitView.IsPaneOpen = false;
            ((ListBox)sender).SelectedItem = null;
            mainListBox.SelectedItem = null;
        }


        /**
         * 跳转到指定页 
         */
        public void navigateToPage(Type viewClass){
            this.frame.Navigate(viewClass.GetType());
        }

    }
}
