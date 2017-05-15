using Sfacg.Model.QueryModel;
using Sfacg.Model.StoreModel;
using Sfacg.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Sfacg.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CollectNovelView : Page
    {

        private ObservableCollection<Novel> collectedNovels;

        public CollectNovelView()
        {
            this.InitializeComponent();
            collectedNovels = new ObservableCollection<Novel>();
            page.ItemsSource = collectedNovels;
            getCollectedNovels();
        }

        private void getCollectedNovels()
        {
            List<Novel> novels = NovelRepositoryUtil.getList(new NovelQO());
            collectedNovels.Clear();
            if(novels.Count <= 0)
            {
                textInfo.Visibility = Visibility.Visible;
            }else
            {
                textInfo.Visibility = Visibility.Collapsed;
            }
            novels.ForEach(n => collectedNovels.Add(n));
        }

        private void sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

        }

        private void page_ItemClick(object sender, ItemClickEventArgs e)
        {
            Novel item = (Novel)e.ClickedItem;
            MainPage.secondFrame.Navigate(typeof(NovelDetail), item.novelId);
        }

        private void btn_refresh_Clicked(object sender, RoutedEventArgs e)
        {
            getCollectedNovels();
        }

        private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            right_menu.ShowAt(sender as Grid, e.GetPosition(sender as Grid));
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            right_menu.ShowAt(sender as Grid, e.GetPosition(sender as Grid));
        }

        private void menuitem_Dlete_Click(object sender, RoutedEventArgs e)
        {
            var novel = (sender as MenuFlyoutItem).DataContext as Novel;
            NovelRepositoryUtil.deleteOne(novel);

            getCollectedNovels();
        }

        private void menuitem_View_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuitem_Delete_F_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
