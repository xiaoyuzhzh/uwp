using Sfacg.Model;
using Sfacg.Model.ApiVO;
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
using static Sfacg.MainPage;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Sfacg.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CategoryPage : Page
    {

        private bool latestLoading = false;//最新是否正在加载

        private bool hotLoading = false;//热门是否正在加载

        private bool finishedLoading = false;//完结是否正在加载

        private int latestPageIndex = 0;//最新页码

        private int hotPageIndex = 0;//热门页码

        private int finishedPageIndex = 0;//完结页码

        private bool latestNoData = false;//是否没有数据了

        private bool hotNoData = false;//是否没有数据了

        private bool finishedNoData = false;//是否没有数据了

        private bool latestedInited = false;//是否初始化加载

        private bool hotInited = false;//是否初始化加载

        private bool finishedInited = false;//是否初始化加载

        private int tid = 0;

        private ObservableCollection<Novel> latestNovels;

        private ObservableCollection<Novel> hotNovels;

        private ObservableCollection<Novel> finishedNovels;

        private FilterType currentFilterType;


        public CategoryPage()
        {
            this.InitializeComponent();

            latestNovels = new ObservableCollection<Novel>();
            latest_Page.ItemsSource = latestNovels;

            hotNovels = new ObservableCollection<Novel>();
            hot_Page.ItemsSource = hotNovels;

            finishedNovels = new ObservableCollection<Novel>();
            finished_Page.ItemsSource = finishedNovels;

            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Category category = e.Parameter as Category;
            if(tid == category.tid)
            {
                return;
            }
            tid = category.tid;
            currentFilterType = FilterType.all;
            loadNextPage(1, true);
        }

        private void sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var sv = getScrollViewer();
            if (sv.VerticalOffset == sv.ScrollableHeight)
            {
                loadNextPage(1, false);
            }
        }

        private ScrollViewer getScrollViewer()
        {
            switch (currentFilterType)
            {
                case FilterType.all:
                    {
                        return latest_Sv;
                    }
                case FilterType.vip:
                    {
                        return hot_Sv;
                    }
                case FilterType.finish:
                    {
                        return finished_Sv;
                    }
                default:
                    {
                        return latest_Sv;
                    }
            }
        }

        private async void loadNextPage(int pageNum, bool init)
        {
            if (init)
            {
                setNoData(false);
            }

            if (getNoData())
            {
                messShow.Show("没有更多数据了", 3000);
                return;
            }

            if (!getLoading())
            {
                setLoading(true);
                setProcessActive(true);

                if (init)
                {
                    setPageIndex(0);
                    clearNovels();
                }

                for (int i = 0; i < pageNum; i++)
                {
                    List<Novel> novelsInPage;
                    try
                    {
                        novelsInPage = await NovelApiUtil.queryCategoryNovels(tid,getPageIndex(),20,currentFilterType);
                    }
                    catch (Exception)
                    {
                        messShow.Show("网络异常", 3000);
                        setProcessActive(false);
                        setLoading(false);
                        return;
                    }
                    setPageIndex(getPageIndex()+1);
                    if (novelsInPage.Count == 0)
                    {
                        setNoData(true);
                        messShow.Show("没有更多数据了", 3000);
                    }
                    else
                    {
                        novelsInPage.ForEach(n =>
                        {
                            var novels = getNovels();
                            novels.Add(n);
                        });
                    }
                }
                setLoading(false);
                setProcessActive(false);
            }

        }

        private void setProcessActive(bool v)
        {
            switch (currentFilterType)
            {
                case FilterType.all:
                    {
                        latestProcess.IsActive = v;
                        break;
                    }
                case FilterType.vip:
                    {
                        hotProcess.IsActive = v;
                        break;
                    }
                case FilterType.finish:
                    {
                        finishedProcess.IsActive = v;
                        break;
                    }
            }
        }

        private bool getNoData()
        {
            switch (currentFilterType)
            {
                case FilterType.all:
                    {
                        return latestNoData;
                    }
                case FilterType.vip:
                    {
                        return hotNoData;
                    }
                case FilterType.finish:
                    {
                        return finishedNoData;
                    }
                default:
                    {
                        return true;
                    }
            }
        }

        private void setNoData(bool v)
        {
            switch (currentFilterType)
            {
                case FilterType.all:
                    {
                        latestNoData = v;
                        break;
                    }
                case FilterType.vip:
                    {
                        hotNoData = v;
                        break;
                    }
                case FilterType.finish:
                    {
                        finishedNoData = v;
                        break;
                    }
            }
        }

        private void setLoading(bool v)
        {
            switch (currentFilterType)
            {
                case FilterType.all:
                    {
                        latestLoading = v;
                        break;
                    }
                case FilterType.vip:
                    {
                        hotLoading = v;
                        break;
                    }
                case FilterType.finish:
                    {
                        finishedLoading = v;
                        break;
                    }
            }
        }

        private bool getLoading()
        {
            switch (currentFilterType)
            {
                case FilterType.all:
                    {
                        return latestLoading;
                    }
                case FilterType.vip:
                    {
                        return hotLoading;
                    }
                case FilterType.finish:
                    {
                        return finishedLoading;
                    }
                default:
                    {
                        return true;
                    }
            }
        }

        private int getPageIndex()
        {
            switch (currentFilterType)
            {
                case FilterType.all:
                    {
                        return latestPageIndex ;
                    }
                case FilterType.vip:
                    {
                        return hotPageIndex;
                    }
                case FilterType.finish:
                    {
                        return finishedPageIndex;
                    }
                default: {
                        return 0;
                    }
            }
        }

        private void setPageIndex(int v)
        {
            switch (currentFilterType)
            {
                case FilterType.all:
                    {
                        latestPageIndex = v;
                        break;
                    }
                case FilterType.vip:
                    {
                        hotPageIndex = v;
                        break;
                    }
                case FilterType.finish:
                    {
                        finishedPageIndex = v;
                        break;
                    }
            }
        }

        private void clearNovels()
        {
            var novels = getNovels();
            novels.Clear();
        }

        private ObservableCollection<Novel> getNovels()
        {
            switch (currentFilterType)
            {
                case FilterType.all:
                    {
                        return latestNovels;
                    }
                case FilterType.vip:
                    {
                        return hotNovels;
                    }
                case FilterType.finish:
                    {
                        return finishedNovels;
                    }
                default: {
                        return null;
                    }
            }
        }

        private GridView getGridView()
        {
            switch (currentFilterType)
            {
                case FilterType.all:
                    {
                        return this.latest_Page;
                    }
                case FilterType.vip:
                    {
                        return this.hot_Page;
                    }
                case FilterType.finish:
                    {
                        return this.finished_Page;
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        private void page_ItemClick(object sender, ItemClickEventArgs e)
        {
            Novel item = (Novel)e.ClickedItem;

            getGridView().PrepareConnectedAnimation("novelCover", item, "NovelCoverImage");
            getGridView().PrepareConnectedAnimation("novelName", item, "NovelName");
            MainPage.secondFrame.Navigate(typeof(NovelDetail), item);
        }

        private void btn_LoadMore_Click(object sender, RoutedEventArgs e)
        {
            loadNextPage(1, false);
        }

        private void btn_refresh_Clicked(object sender, RoutedEventArgs e)
        {
            loadNextPage(1, true);
            setInited(true);
        }


        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int d = Convert.ToInt32(this.ActualWidth / 120);
            if (d > 10)
            {
                d = 10;
            }

            bor_Width.Width = (this.ActualWidth - 10 * d - 20) / d - 3;
        }


        private void setInited(bool v)
        {
            switch (currentFilterType)
            {
                case FilterType.all:
                    {
                        latestedInited = v;
                        break;
                    }
                case FilterType.vip:
                    {
                        hotInited = v;
                        break;
                    }
                case FilterType.finish:
                    {
                        finishedInited = v;
                        break;
                    }
            }
        }


        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pivot = sender as Pivot;
            switch (pivot.SelectedIndex)
            {
                case 0: {
                        currentFilterType = FilterType.all;
                        break;
                    }
                case 1:
                    {
                        currentFilterType = FilterType.vip;
                        break;
                    }
                case 2:
                    {
                        currentFilterType = FilterType.finish;
                        break;
                    }
            }

            if (!getInited())
            {
                loadNextPage(1, true);
                setInited(true);
            }
        }

        private bool getInited()
        {
            switch (currentFilterType)
            {
                case FilterType.all:
                    {
                        return latestedInited;
                    }
                case FilterType.vip:
                    {
                        return hotInited;
                    }
                case FilterType.finish:
                    {
                        return finishedInited;
                    }
                default:
                    {
                        return true;
                    }
            }
        }
    }
}
