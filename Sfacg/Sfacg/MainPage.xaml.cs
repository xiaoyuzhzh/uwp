using Sfacg.Model;
using Sfacg.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
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

        public MainPage()
        {
            this.InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Current = this;


            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManager_BackRequested;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            push.Navigate(typeof(PushNovelView));

            var rootFrame = (Frame)Window.Current.Content;
            rootFrame.Navigated += OnNavigatedToPage;
        }

        private void OnNavigatedToPage(object sender, NavigationEventArgs e)
        {
            var rootFrame = (Frame)Window.Current.Content;
            if (rootFrame.CanGoBack)
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
            var rootFrame = (Frame)Window.Current.Content;
            if (rootFrame.CanGoBack && !e.Handled)
            {
                rootFrame.GoBack();
            }
            e.Handled = true;
        }


    }
}
