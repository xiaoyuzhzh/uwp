using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tool.Pages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Tool
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            //SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManager_BackRequested;

            //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            //this.frame.Navigated += OnNavigatedToPage;
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {

            }
            else
            {
                var menu = args.SelectedItem as NavigationViewItem;
                switch (menu.Tag.ToString())
                {
                    case "codeChange":
                        contentFrame.Navigate(typeof(CodeConvert));
                        NavView.Header = "编码转换";
                        break;
                    case "timestampTranslate":
                        contentFrame.Navigate(typeof(TimestampConvert));
                        NavView.Header = "时间戳转换";
                        break;
                }
            }
        }

        //private void NavigateButton_Click(object sender, RoutedEventArgs e)
        //{
        //    mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
        //}

        //private void mainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (this.frame == null) return;
        //    ListBox box = (ListBox)sender;
        //    ListBoxItem selectedItem = (ListBoxItem)(box).SelectedItem;
        //    if (selectedItem == null) return;
        //    var itemName = selectedItem.Name;
        //    switch (itemName)
        //    {
        //        case "codeConvert":
        //            {
        //                this.frame.Navigate(typeof(CodeConvert));
        //                break;
        //            }
        //        case "timestampConvert":
        //            {
        //                this.frame.Navigate(typeof(TimestampConvert));
        //                break;
        //            }
        //        default: { break; }
        //    }
        //    mainSplitView.IsPaneOpen = false;
        //    ((ListBox)sender).SelectedItem = null;
        //}

        //private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        //{

        //}

        //private void OnNavigatedToPage(object sender, NavigationEventArgs e)
        //{
        //    if (frame.CanGoBack)
        //    {
        //        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        //    }
        //    else
        //    {
        //        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        //    }
        //}

        //private void bottomListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    mainSplitView.IsPaneOpen = false;
        //    ((ListBox)sender).SelectedItem = null;
        //}

        ///**
        // * 返回按钮处理方案 
        // */
        //private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        //{
        //    if (frame.CanGoBack && !e.Handled)
        //    {
        //        frame.GoBack();
        //    }
        //    e.Handled = true;
        //}
    }
}
