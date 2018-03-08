using Sfacg.Model;
using Sfacg.Model.StoreModel;
using Sfacg.Utils;
using Sfacg.Views.Controls;
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
    public sealed partial class JPNovelView
    {
        private ObservableCollection<Novel> novels;

        private bool loading = false;

        private int pageIndex = 1;

        private bool noData = false;

        private bool inited = false;

        public JPNovelView()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            novels = new ObservableCollection<Novel>();
            page.ItemsSource = novels;
        }


        private void sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sv.VerticalOffset == sv.ScrollableHeight)
            {
                loadNextPage(1,false);
            }
        }

        private void btn_LoadMore_Click(object sender, RoutedEventArgs e)
        {
            loadNextPage(1,false);
        }

        private async void loadNextPage(int pageNum,bool init)
        {
            if (!loading&&!noData)
            {
                loading = true;
                process.IsActive = true;

                if (init)
                {
                    pageIndex = 0;
                    novels.Clear();
                }

                for (int i = 0; i< pageNum; i++)
                {
                    List<Novel> novelsInPage;
                    try
                    {
                        novelsInPage = await NovelApiUtil.getJPNovels(pageIndex, 24, ListType.latest);
                    }
                    catch (Exception)
                    {
                        messShow.Show("网络异常", 3000);
                        process.IsActive = false;
                        loading = false;
                        return;
                    }
                    pageIndex++;
                    if (novelsInPage.Count == 0)
                    {
                        noData = true;
                        messShow.Show("没有更多数据了", 3000);
                    }else
                    {
                        novelsInPage.ForEach(n =>
                        {
                            novels.Add(n);
                        });
                    }
                }
                
                loading = false;
                process.IsActive = false;
            }
            
        }



        private async void btn_refresh_Clicked(object sender, RoutedEventArgs e)
        {
            loadNextPage(1,true);
            inited = true;
        }

        protected GridView getPage()
        {
            return this.page;
        }

        protected void setItemWidth(double width)
        {
            bor_Width.Width = width;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!inited)
            {
                loadNextPage(1, true);//首次进入加载两页
                inited = true;
            }
        }

        /// <summary>
        /// 列表对象被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void page_ItemClick(object sender, ItemClickEventArgs e)
        {

            Novel item = (Novel)e.ClickedItem;
            try
            {
                getPage().PrepareConnectedAnimation("novelCover", item, "NovelCoverImage");
                getPage().PrepareConnectedAnimation("novelName", item, "NovelName");
            }
            catch (Exception)
            {

            }
            MainPage.secondFrame.Navigate(typeof(NovelDetail), item);
        }

        /// <summary>
        /// 页面大小改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int d = Convert.ToInt32(this.ActualWidth / 120);
            if (d > 10)
            {
                d = 10;
            }

            setItemWidth((this.ActualWidth - 10 * d) / d - 3);
        }
    }

    
}
