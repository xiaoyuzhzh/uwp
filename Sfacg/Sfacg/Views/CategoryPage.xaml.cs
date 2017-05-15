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

        private ObservableCollection<CategoryNovelVO> latestNovels;

        private ObservableCollection<CategoryNovelVO> hotNovels;

        private ObservableCollection<CategoryNovelVO> finishedNovels;

        private ListType currentListType;


        public CategoryPage()
        {
            this.InitializeComponent();

            latestNovels = new ObservableCollection<CategoryNovelVO>();
            latest_Page.ItemsSource = latestNovels;

            hotNovels = new ObservableCollection<CategoryNovelVO>();
            hot_Page.ItemsSource = hotNovels;

            finishedNovels = new ObservableCollection<CategoryNovelVO>();
            finished_Page.ItemsSource = finishedNovels;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Category category = e.Parameter as Category;
            tid = category.tid;
            currentListType = ListType.latest;
            loadNextPage(2, true);
        }

        private void sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var sv = getScrollViewer();
            if (sv.VerticalOffset == sv.ScrollableHeight)
            {
                loadNextPage(2, false);
            }
        }

        private ScrollViewer getScrollViewer()
        {
            switch (currentListType)
            {
                case ListType.latest:
                    {
                        return latest_Sv;
                    }
                case ListType.hot:
                    {
                        return hot_Sv;
                    }
                case ListType.recommend:
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
                    List<CategoryNovelVO> nextPageNovels;
                    try
                    {
                        nextPageNovels = await NovelUtil.queryCategoryNovels(tid, currentListType, getPageIndex());
                    }
                    catch (Exception)
                    {
                        messShow.Show("网络异常", 3000);
                        setProcessActive(false);
                        setLoading(false);
                        return;
                    }
                    setPageIndex(getPageIndex()+1);
                    if (nextPageNovels.Count == 0)
                    {
                        setNoData(true);
                        messShow.Show("没有更多数据了", 3000);
                    }
                    else
                    {
                        nextPageNovels.ForEach(n =>
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
            switch (currentListType)
            {
                case ListType.latest:
                    {
                        latestProcess.IsActive = v;
                        break;
                    }
                case ListType.hot:
                    {
                        hotProcess.IsActive = v;
                        break;
                    }
                case ListType.recommend:
                    {
                        finishedProcess.IsActive = v;
                        break;
                    }
            }
        }

        private bool getNoData()
        {
            switch (currentListType)
            {
                case ListType.latest:
                    {
                        return latestNoData;
                    }
                case ListType.hot:
                    {
                        return hotNoData;
                    }
                case ListType.recommend:
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
            switch (currentListType)
            {
                case ListType.latest:
                    {
                        latestNoData = v;
                        break;
                    }
                case ListType.hot:
                    {
                        hotNoData = v;
                        break;
                    }
                case ListType.recommend:
                    {
                        finishedNoData = v;
                        break;
                    }
            }
        }

        private void setLoading(bool v)
        {
            switch (currentListType)
            {
                case ListType.latest:
                    {
                        latestLoading = v;
                        break;
                    }
                case ListType.hot:
                    {
                        hotLoading = v;
                        break;
                    }
                case ListType.recommend:
                    {
                        finishedLoading = v;
                        break;
                    }
            }
        }

        private bool getLoading()
        {
            switch (currentListType)
            {
                case ListType.latest:
                    {
                        return latestLoading;
                    }
                case ListType.hot:
                    {
                        return hotLoading;
                    }
                case ListType.recommend:
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
            switch (currentListType)
            {
                case ListType.latest:
                    {
                        return latestPageIndex ;
                    }
                case ListType.hot:
                    {
                        return hotPageIndex;
                    }
                case ListType.recommend:
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
            switch (currentListType)
            {
                case ListType.latest:
                    {
                        latestPageIndex = v;
                        break;
                    }
                case ListType.hot:
                    {
                        hotPageIndex = v;
                        break;
                    }
                case ListType.recommend:
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

        private ObservableCollection<CategoryNovelVO> getNovels()
        {
            switch (currentListType)
            {
                case ListType.latest:
                    {
                        return latestNovels;
                    }
                case ListType.hot:
                    {
                        return hotNovels;
                    }
                case ListType.recommend:
                    {
                        return finishedNovels;
                    }
                default: {
                        return null;
                    }
            }
        }

        private void page_ItemClick(object sender, ItemClickEventArgs e)
        {
            CategoryNovelVO item = (CategoryNovelVO)e.ClickedItem;

            MainPage.secondFrame.Navigate(typeof(NovelDetail), item.NovelID.ToString());
        }

        private void btn_LoadMore_Click(object sender, RoutedEventArgs e)
        {
            loadNextPage(2, false);
        }

        private void btn_refresh_Clicked(object sender, RoutedEventArgs e)
        {
            loadNextPage(2, true);
            setInited(true);
        }


        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int d = Convert.ToInt32(this.ActualWidth / 160);
            if (d > 10)
            {
                d = 10;
            }

            bor_Width.Width = this.ActualWidth / d - 25;
        }


        private void setInited(bool v)
        {
            switch (currentListType)
            {
                case ListType.latest:
                    {
                        latestedInited = v;
                        break;
                    }
                case ListType.hot:
                    {
                        hotInited = v;
                        break;
                    }
                case ListType.recommend:
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
                        currentListType = ListType.latest;
                        break;
                    }
                case 1:
                    {
                        currentListType = ListType.hot;
                        break;
                    }
                case 2:
                    {
                        currentListType = ListType.recommend;
                        break;
                    }
            }

            if (!getInited())
            {
                loadNextPage(2, true);
                setInited(true);
            }
        }

        private bool getInited()
        {
            switch (currentListType)
            {
                case ListType.latest:
                    {
                        return latestedInited;
                    }
                case ListType.hot:
                    {
                        return hotInited;
                    }
                case ListType.recommend:
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
