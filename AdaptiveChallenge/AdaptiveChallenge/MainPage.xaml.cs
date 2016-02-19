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
using AdaptiveChallenge.Model;
using System.Collections.ObjectModel;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace AdaptiveChallenge
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private ObservableCollection<NewsItem> newsItems;

        public MainPage()
        {
            this.InitializeComponent();
            newsItems = new ObservableCollection<NewsItem>();
            FinancialBoxItem.IsSelected = true;
            showFinancial();
        }

        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
            RootSplitView.IsPaneOpen = !RootSplitView.IsPaneOpen;
        }

        private void NavigateListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FinancialBoxItem.IsSelected)
            {
                showFinancial();
            }

            if (FoodListBoxItem.IsSelected)
            {
                showFood();
            }
        }

        private void showFinancial() {
            newsItems.Clear();
            List<NewsItem> items = Factory.getNewsFinancial();
            foreach(var item in items)
            {
                newsItems.Add(item);
            }
        }

        private void showFood() {
            newsItems.Clear();
            List<NewsItem> items = Factory.getNewsFood();
            //foreach (var item in items)
            //{
            //    newsItems.Add(item);
            //}
            items.ForEach(p => newsItems.Add(p));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FinancialBoxItem.IsSelected = true;
        }
    }
}
