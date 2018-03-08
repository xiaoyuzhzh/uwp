using Sfacg.Model;
using Sfacg.Model.ApiVO;
using Sfacg.Model.QueryModel;
using Sfacg.Model.StoreModel;
using Sfacg.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Sfacg.Views
{

    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NovelCatalogView : Page
    {
        private ObservableCollection<Volume> volumes;
        private ObservableCollection<Bookmark> bookmarks;

        private string novelId;

        public NovelCatalogView()
        {
            this.InitializeComponent();
            volumes = new ObservableCollection<Volume>();
            bookmarks = new ObservableCollection<Bookmark>();
            //NavigationCacheMode = NavigationCacheMode.Enabled;

            process.IsActive = true;

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            //初始化界面
            VisualStateManager.GoToState(this, SingleSelectionState.Name, true);

            Bookmark bookmark = null;
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
                bookmark = BaseUtil.getSetting<Bookmark>(BaseUtil.READ_POINT_PREFIX + novelId, true);

                List<Volume> volumeList;
                try
                {
                    volumeList = await NovelApiUtil.getNovelCatalog(novelId,false);
                    volumes.Clear();
                    StorageFolder novelFolder = await NovelApiUtil.folder.CreateFolderAsync(novelId, CreationCollisionOption.OpenIfExists);
                    List<StorageFile> files = (await novelFolder.GetFilesAsync()).ToList();
                    volumeList.ForEach(v =>
                    {
                        if (files.Count>0&&v.chapters != null && v.chapters.Count > 0)
                        {
                            v.chapters.ForEach(c => {
                                //更新已缓存标记
                                files.ForEach(f => {
                                    if (f.Name.Equals(c.chapId))
                                    {
                                        c.cached = "已缓存";
                                    }
                                });
                                //更新阅读进度
                                if (bookmark != null && bookmark.chapId.Equals(c.chapId))
                                {
                                    c.title = c.title + "  " + bookmark.progress + "%";
                                }
                            });
                        }

                        volumes.Add(v);
                    });

                    if (bookmark != null)
                    {
                        var item = getItem(bookmark);
                        if (item == null)
                        {
                            return;
                        }
                        CatalogListView.SelectedItem = item;
                        CatalogListView.ScrollIntoView(item, ScrollIntoViewAlignment.Leading);

                    }

                    process.IsActive = false;
                }
                catch (Exception)
                {
                    messShow.Show("网络异常", 3000);
                    process.IsActive = false;
                    return;
                }
            }
            else
            {

            }

        }


        private int getIndex(Bookmark bookmark)
        {
            int index = 0;
            var array = CatalogListView.Items.ToArray();
            for (; index < array.Length; index++)
            {
                if (((ChapterList)array[index]).chapId.Equals(bookmark.chapId))
                {
                    return index;
                }
            }

            return 0;

        }

        private Object getItem(Bookmark bookmark)
        {
            int index = 0;
            var array = CatalogListView.Items.ToArray();
            for (; index < array.Length; index++)
            {
                if (((Chapter)array[index]).chapId.Equals(bookmark.chapId))
                {
                    return array[index];
                }
            }
            return null;
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
            var charpter = (Chapter)e.ClickedItem;

            try
            {
                CatalogListView.PrepareConnectedAnimation("chapterName", charpter, "ChapterName");
            }
            catch (Exception)
            {
            }

            var bookmark = BaseUtil.getSetting<Bookmark>(BaseUtil.READ_POINT_PREFIX + novelId, true);
            if (bookmark != null&&charpter.chapId.Equals(bookmark.chapId))
            {
                charpter.itemId = bookmark.itemId;
                charpter.itemContainerHeight = bookmark.itemContainerHeight;
                charpter.listPosition = bookmark.listPosition;
            }
            this.Frame.Navigate(typeof(NovelReadV2), charpter);

        }


        private void bookmark_ItemClick(object sender, ItemClickEventArgs e)
        {
            var bookmark = (Bookmark)e.ClickedItem;
            var charpter = new Chapter() { novelId = bookmark.novelId,chapId = bookmark.chapId ,title = bookmark.chapName,itemId= bookmark.itemId, listPosition = bookmark.listPosition, itemContainerHeight = bookmark.itemContainerHeight};
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

        private void SelectItems(object sender, RoutedEventArgs e)
        {
            if (CatalogListView.Items.Count > 0)
            {
                VisualStateManager.GoToState(this, MultipleSelectionState.Name, true);
            }
        }

        private void CancelSelection(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this,SingleSelectionState.Name, true);
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
