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

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace ComplicatedApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            FinancialListBoxItem.IsSelected = true;
            ContFrame.Navigate(typeof(Financial));
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            RootSplitView.IsPaneOpen = !RootSplitView.IsPaneOpen;
        }

        private void IconMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FinancialListBoxItem.IsSelected)
            {
                ContFrame.Navigate(typeof(Financial));
                TitleText.Text = "Financial";
                BackButton.Visibility = Visibility.Collapsed;
            }
            else if (FoodListBoxItem.IsSelected)
            {
                TitleText.Text = "Food";
                ContFrame.Navigate(typeof(Food));
                BackButton.Visibility = Visibility.Visible;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            //            ContFrame.Navigate(typeof(Financial));
            //            TitleText.Text = "Financial";
            //           Back.Visibility = Visibility.Collapsed;
            if (ContFrame.CanGoBack)
            {
                ContFrame.GoBack();
                FinancialListBoxItem.IsSelected = true;
            }
        }
    }
}
