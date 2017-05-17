using Sfacg.Model;
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
    public sealed partial class NovelCatalogView : Page
    {
        private ObservableCollection<VolumeList> volumes;
        private ObservableCollection<Bookmark> bookmarks;

        private string novelId;

        public NovelCatalogView()
        {
            this.InitializeComponent();
            volumes = new ObservableCollection<VolumeList>();
            bookmarks = new ObservableCollection<Bookmark>();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            process.IsActive = true;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            

            if (e.Parameter is string)
            {
                var novelIdNew = (string)e.Parameter;

                //书签要每次刷新
                RefreshBookmark(novelIdNew);

                if (!string.IsNullOrEmpty(novelId) && novelId.Equals(novelIdNew))
                {
                    return;
                }

                novelId = novelIdNew;

                List<VolumeList> volumeList;
                try
                {
                    volumeList = await NovelUtil.getNovelCatalog(novelId);
                }
                catch (Exception)
                {
                    messShow.Show("网络异常", 3000);
                    process.IsActive = false;
                    return;
                }
                volumes.Clear();
                volumeList.ForEach(v => volumes.Add(v));

                process.IsActive = false;
            }
            else
            {

            }
        }

        private void RefreshBookmark(string novelId)
        {
            BookmarkQO qo = new BookmarkQO() { novelId = novelId };
            var bookmarkList = BookmarkRepositoryUtil.getList(qo);
            bookmarks.Clear();
            if (bookmarkList.Count <= 0)
            {
                textInfo.Visibility = Visibility.Visible;
            }
            else
            {
                textInfo.Visibility = Visibility.Collapsed;
            }
            bookmarkList.ForEach(b => bookmarks.Add(b));
        }

        private void Chapter_ItemClick(object sender, ItemClickEventArgs e)
        {
            var charpter = (ChapterList)e.ClickedItem;

            //var rootFrame = (Frame)Window.Current.Content;

            //this.Frame.Navigate(typeof(NovelReadView), charpter);
            this.Frame.Navigate(typeof(NovelReadV2), charpter);

        }


        private void bookmark_ItemClick(object sender, ItemClickEventArgs e)
        {
            var bookmark = (Bookmark)e.ClickedItem;
            var charpter = new ChapterList() { novelId = bookmark.novelId,chapId = bookmark.chapId ,title = bookmark.chapName};
            //this.Frame.Navigate(typeof(NovelReadView), charpter);
            this.Frame.Navigate(typeof(NovelReadV2), charpter);
        }

        private void menuitem_Dlete_Click(object sender, RoutedEventArgs e)
        {
            var bookmark = (sender as MenuFlyoutItem).DataContext as Bookmark;
            BookmarkRepositoryUtil.deleteOne(bookmark);

            //书签要每次刷新
            RefreshBookmark(novelId);
        }

        private void menuitem_View_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuitem_Delete_F_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            right_menu.ShowAt(sender as Grid, e.GetPosition(sender as Grid));
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            right_menu.ShowAt(sender as Grid, e.GetPosition(sender as Grid));
        }
    }
}
