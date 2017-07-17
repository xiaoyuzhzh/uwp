using Sfacg.Model;
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
    public sealed partial class JPNovelView : Page
    {
        private ObservableCollection<JPNovelData> novels;

        private bool loading = false;

        private int pageIndex = 1;

        private bool noData = false;

        private bool inited = false;

        public JPNovelView()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            novels = new ObservableCollection<JPNovelData>();
            page.ItemsSource = novels;
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!inited)
            {
                loadNextPage(2,true);//首次进入加载两页
                inited = true;
            }
            
        }

        private void page_ItemClick(object sender, ItemClickEventArgs e)
        {
            JPNovelData item = (JPNovelData)e.ClickedItem;

            MainPage.secondFrame.Navigate(typeof(NovelDetail), item.NovelID);
        }

        private void sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sv.VerticalOffset == sv.ScrollableHeight)
            {
                loadNextPage(2,false);
            }
        }

        private void btn_LoadMore_Click(object sender, RoutedEventArgs e)
        {
            loadNextPage(2,false);
        }

        private async void loadNextPage(int pageNum,bool init)
        {
            if (!loading&&!noData)
            {
                loading = true;
                process.IsActive = true;

                if (init)
                {
                    pageIndex = 1;
                    novels.Clear();
                }

                for (int i = 0; i< pageNum; i++)
                {
                    List<JPNovelData> nextPageNovels;
                    try
                    {
                        nextPageNovels = await NovelUtil.getJPNovels(pageIndex, ListType.latest);
                    }
                    catch (Exception)
                    {
                        messShow.Show("网络异常", 3000);
                        process.IsActive = false;
                        loading = false;
                        return;
                    }
                    pageIndex++;
                    if (nextPageNovels.Count == 0)
                    {
                        noData = true;
                        messShow.Show("没有更多数据了", 3000);
                    }else
                    {
                        nextPageNovels.ForEach(n =>
                        {
                            novels.Add(n);
                        });
                    }
                }
                loading = false;
                process.IsActive = false;
            }
            
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int d = Convert.ToInt32(this.ActualWidth / 120);
            if (d > 10)
            {
                d = 10;
            }

            bor_Width.Width = (this.ActualWidth- 10*d) / d -3;
        }

        private async void btn_refresh_Clicked(object sender, RoutedEventArgs e)
        {
            loadNextPage(2,true);
            inited = true;
        }
    }

    
}
