using Sfacg.Model;
using Sfacg.Utils;
using Sfacg.Views;
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
    public sealed partial class PushNovelView : Page
    {
        private List<NovelsVOData> novels;

        public PushNovelView()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            
            getPushNovels();
        }

        private async System.Threading.Tasks.Task getPushNovels()
        {
            process.IsActive = true;
            try
            {
                novels = await NovelUtil.getPushNovels(10);
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

        private void page_ItemClick(object sender, ItemClickEventArgs e)
        {
            NovelsVOData item = (NovelsVOData)e.ClickedItem;
            MainPage.secondFrame.Navigate(typeof(NovelDetail), item.novelId);
        }


        private void sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sv.VerticalOffset == sv.ScrollableHeight)
            {
                //TODO 滑动到底部了，准备加载新的内容
            }
        }

        private void Border_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {

        }

        private void Border_Holding(object sender, HoldingRoutedEventArgs e)
        {

        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int d = Convert.ToInt32(this.ActualWidth / 160);
            if (d > 10)
            {
                d = 10;
            }

            bor_Width.Width = this.ActualWidth / d - 15;
        }

        private async void btn_refresh_Clicked(object sender, RoutedEventArgs e)
        {
            getPushNovels();
        }
    }
}
