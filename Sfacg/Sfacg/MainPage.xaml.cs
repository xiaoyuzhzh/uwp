using Sfacg.Model;
using Sfacg.Model.StoreModel;
using Sfacg.Utils;
using Sfacg.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Sfacg
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        public static Frame secondFrame;

        private List<Category> categories;

        public MainPage()
        {
            this.InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Current = this;

            NavigationCacheMode = NavigationCacheMode.Enabled;


            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManager_BackRequested;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            if (frame.BackStackDepth == 0) frame.Navigate(typeof(BridgeView));

            if (novelDetail_frame.BackStackDepth == 0) novelDetail_frame.Navigate(typeof(BridgeView));

            var rootFrame = (Frame)Window.Current.Content;
            rootFrame.Navigated += OnNavigatedToPage;
            frame.Navigated += OnNavigatedToPage;
            novelDetail_frame.Navigated += OnNavigatedToPage;

            AppDatabaseUtil.initTable();//初始化数据库


            //初始化分类的数据
            categories = new List<Category>();
            
            categories.Add(new Category() { imageUrl = "ms-appx:///Assets/category/fenlei_rx.jpg", name = "热血类", tid = 1 });
            categories.Add(new Category() { imageUrl = "ms-appx:///Assets/category/fenlei_mx.jpg", name = "冒险类", tid = 2 });
            categories.Add(new Category() { imageUrl = "ms-appx:///Assets/category/fenlei_mh.jpg", name = "魔幻类", tid = 4 });
            categories.Add(new Category() { imageUrl = "ms-appx:///Assets/category/fenlei_xy.jpg", name = "校园类", tid = 6 });
            categories.Add(new Category() { imageUrl = "ms-appx:///Assets/category/fenlei_wx.jpg", name = "架空类", tid = 5 });
            categories.Add(new Category() { imageUrl = "ms-appx:///Assets/category/fenlei_tr.jpg", name = "同人类", tid = 7 });
            categories.Add(new Category() { imageUrl = "ms-appx:///Assets/category/fenlei_aq.jpg", name = "爱情类", tid = 9 });
            categories.Add(new Category() { imageUrl = "ms-appx:///Assets/category/fenlei_gx.jpg", name = "搞笑类", tid = 8 });
            categories.Add(new Category() { imageUrl = "ms-appx:///Assets/category/fenlei_kh.jpg", name = "科幻类", tid = 13 });
            categories.Add(new Category() { imageUrl = "ms-appx:///Assets/category/fenlei_tl.jpg", name = "推理类", tid = 11 });
            categories.Add(new Category() { imageUrl = "ms-appx:///Assets/category/fenlei_jx.jpg", name = "惊悚类", tid = 14 });
            categories.Add(new Category() { imageUrl = "ms-appx:///Assets/category/fenlei_yd.jpg", name = "运动类", tid = 3 });
            categories.Add(new Category() { imageUrl = "ms-appx:///Assets/category/fenlei_jz.jpg", name = "机战类", tid = 12 });
            categories.Add(new Category() { imageUrl = "ms-appx:///Assets/category/fenlei_wq.jpg", name = "温情类", tid = 10 });
            


            CategoryPivot.ItemsSource = categories;

            
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            secondFrame = frame;

            jpnovel.Navigate(typeof(JPNovelView));
            push.Navigate(typeof(PushNovelView));
            collections.Navigate(typeof(CollectNovelView));


            //从splashPage进入是不要显示返回按钮
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

        }

        private void OnNavigatedToPage(object sender, NavigationEventArgs e)
        {
            var rootFrame = (Frame)Window.Current.Content;
            if (frame.CanGoBack||rootFrame.BackStackDepth>1|| novelDetail_frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        /**
         * 返回按钮处理方案 
         */
        private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (novelDetail_frame.CanGoBack && !e.Handled)
            {
                novelDetail_frame.GoBack();
                e.Handled = true;
            }

            if (frame.CanGoBack && !e.Handled)
            {
                frame.GoBack();
                e.Handled = true;
            }

            var rootFrame = (Frame)Window.Current.Content;
            if (rootFrame.BackStackDepth > 1 && !e.Handled)
            {
                rootFrame.GoBack();
                e.Handled = true;
            }
        }

        private void btn_Qr_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(QueryPage));
        }

        private void frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (frame.CanGoBack)
            {
                frame.Visibility = Visibility.Visible;
            }else
            {
                frame.Visibility = Visibility.Collapsed;
            }
        }

        private void novelDetail_frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (novelDetail_frame.CanGoBack)
            {
                novelDetail_frame.Visibility = Visibility.Visible;
            }
            else
            {
                novelDetail_frame.Visibility = Visibility.Collapsed;
            }
        }


        public class Category{
            public string imageUrl { get; set; }
            public string name { get; set; }
            public int tid { get; set; }
        }

        private void CategoryPivot_ItemClick(object sender, ItemClickEventArgs e)
        {
            Category item = (Category)e.ClickedItem;
            this.frame.Navigate(typeof(CategoryPage), item);
        }
    }
}
