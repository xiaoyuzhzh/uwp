using Sfacg.Model;
using Sfacg.Model.StoreModel;
using Sfacg.Utils;
using Sfacg.Views;
using Sfacg.Views.Controls;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Sfacg.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PushNovelView
    {
        private List<Novel> novels;

        public PushNovelView()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }


        /// <summary>
        /// 拉取推荐小说
        /// </summary>
        private async void getPushNovels()
        {
            process.IsActive = true;
            try
            {
                novels = await NovelApiUtil.getPushNovels(12);
            }
            catch (Exception)
            {
                messShow.Show("网络异常", 3000);
                process.IsActive = false;
                return;
            }

            page.ItemsSource = novels;
            process.IsActive = false;
        }

        private async void btn_refresh_Clicked(object sender, RoutedEventArgs e)
        {
            getPushNovels();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            getPushNovels();
        }


        /// <summary>
        /// 给抽象类调用，传入gridView
        /// </summary>
        /// <returns></returns>
        private GridView getPage()
        {
            return this.page;
        }

        /// <summary>
        /// 设置项高
        /// </summary>
        /// <param name="width"></param>
        private void setItemWidth(double width)
        {
            bor_Width.Width = width;
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

        private void sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

        }
    }
}
